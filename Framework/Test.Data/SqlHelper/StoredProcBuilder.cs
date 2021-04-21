using Test.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Test.Data.SqlHelper
{
    public class StoredProcBuilder : IStoredProcBuilder
    {
        private const string RetParamName = "_retParam";
        private readonly DbCommand _cmd;
        private readonly IDataContext _context;
        private List<Type> _resultSets;
        public List<Type> ResultSets { get => _resultSets; set { _resultSets = value; } }
        public StoredProcBuilder(IDataContext context)
        {
            _context = context;
            DbCommand cmd = _context.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Transaction = _context.Database.CurrentTransaction?.GetDbTransaction();
            cmd.CommandTimeout = _context.Database.GetCommandTimeout().GetValueOrDefault(cmd.CommandTimeout);
            _cmd = cmd;
            _resultSets = new List<Type>();
        }
        public IStoredProcBuilder StoredProc(string name)
        {
            ClearParam();
            ClearResult();
            _cmd.CommandText = name;
            return this;
        }
        public IStoredProcBuilder ClearResult()
        {
            _resultSets.Clear();
            return this;
        }
        public IStoredProcBuilder ClearParam()
        {
            _cmd.Parameters.Clear();
            return this;
        }
        public IStoredProcBuilder AddResult<TResult>()
        {
            _resultSets.Add(typeof(TResult));

            return this;
        }

        public IStoredProcBuilder AddParam<T>(string name, T val)
        {
            AddParamInner(name, val, ParameterDirection.Input);
            return this;
        }

        public IStoredProcBuilder AddParam<T>(string name, out IOutParam<T> outParam)
        {
            outParam = AddOutputParamInner(name, default(T), ParameterDirection.Output);
            return this;
        }
        public IStoredProcBuilder AddParam<T>(string name, T val, out IOutParam<T> outParam, int size = 0, byte precision = 0, byte scale = 0)
        {
            outParam = AddOutputParamInner(name, val, ParameterDirection.InputOutput, size, precision, scale);
            return this;
        }

        public IStoredProcBuilder AddParam<T>(string name, out IOutParam<T> outParam, int size = 0, byte precision = 0, byte scale = 0)
        {
            outParam = AddOutputParamInner(name, default(T), ParameterDirection.Output, size, precision, scale);
            return this;
        }

        public IStoredProcBuilder AddParam<T>(string name, T val, out IOutParam<T> outParam)
        {
            outParam = AddOutputParamInner(name, val, ParameterDirection.InputOutput);
            return this;
        }

        public IStoredProcBuilder AddParam(DbParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            _cmd.Parameters.Add(parameter);
            return this;
        }


        public IStoredProcBuilder ReturnValue<T>(out IOutParam<T> retParam)
        {
            retParam = AddOutputParamInner(RetParamName, default(T), ParameterDirection.ReturnValue);
            return this;
        }

        public IStoredProcBuilder SetTimeout(int timeout)
        {
            _cmd.CommandTimeout = timeout;
            return this;
        }

        public void Exec(Action<DbDataReader> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            try
            {
                OpenConnection();
                using (DbDataReader r = _cmd.ExecuteReader())
                {
                    action(r);
                }
            }
            finally
            {
                Dispose();
            }
        }

        public List<IEnumerable> ExecMulti()
        {
            try
            {
                List<IEnumerable> results = new List<IEnumerable>();
                int counter = 0;
                OpenConnection();
                using (DbDataReader reader = _cmd.ExecuteReader())
                {
                    do
                    {
                        if (counter > _resultSets.Count - 1) { break; }
                        Type TResult = _resultSets[counter];
                        var innerResults = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(TResult)); ;
                        while (reader.Read())
                        {
                            var item = Activator.CreateInstance(TResult);

                            for (int inc = 0; inc < reader.FieldCount; inc++)
                            {
                                Type type = item.GetType();
                                string name = reader.GetName(inc);
                                PropertyInfo property = type.GetProperty(name);

                                if (property != null && name == property.Name)
                                {
                                    var value = reader.GetValue(inc);
                                    if (value != null && value != DBNull.Value)
                                    {
                                        property.SetValue(item, Convert.ChangeType(value, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType), null);
                                    }
                                }
                            }
                            innerResults.Add(item);
                        }
                        results.Add(innerResults);
                        counter++;
                    }
                    while (reader.NextResult());
                    reader.Close();
                }
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Dispose();
            }

        }
        public Task ExecAsync(Func<DbDataReader, Task> action)
        {
            if (string.IsNullOrEmpty(_cmd.CommandText))
                throw new ArgumentNullException(nameof(_cmd.CommandText));
            return ExecAsync(action, CancellationToken.None);
        }

        public async Task ExecAsync(Func<DbDataReader, Task> action, CancellationToken cancellationToken)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            if (string.IsNullOrEmpty(_cmd.CommandText))
                throw new ArgumentNullException(nameof(_cmd.CommandText));
            try
            {
                await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
                using (DbDataReader r = await _cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                {
                    try
                    {
                        await action(r).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        // In case the action bombs out, cancel the command and rethrow to propagate the actual action
                        // exception. If we don't cancel the command, we will be stuck on disposing of the reader until
                        // the sproc completes, even though the action has already thrown an exception. This is also the
                        // case when the cancellation token is cancelled after the action exception but before the sproc
                        // completes: we will still be stuck on disposing of the reader until the sproc completes. This
                        // is caused by the fact that DbDataReader.Dispose does not react to cancellations and simply
                        // waits for the sproc to complete. // The only way to cancel the execution when the reader has
                        // been engaged and the action has thrown, is to cancel the command.
                        _cmd.Cancel();
                        throw;
                    }
                }
            }
            finally
            {
                Dispose();
            }
        }

        public int ExecNonQuery()
        {
            try
            {
                if (string.IsNullOrEmpty(_cmd.CommandText))
                    throw new ArgumentNullException(nameof(_cmd.CommandText));
                OpenConnection();
                return _cmd.ExecuteNonQuery();
            }
            finally
            {
                Dispose();
            }
        }

        public Task<int> ExecNonQueryAsync()
        {
            if (string.IsNullOrEmpty(_cmd.CommandText))
                throw new ArgumentNullException(nameof(_cmd.CommandText));
            return ExecNonQueryAsync(CancellationToken.None);
        }


        public async Task<int> ExecNonQueryAsync(CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(_cmd.CommandText))
                    throw new ArgumentNullException(nameof(_cmd.CommandText));
                await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
                return await _cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                Dispose();
            }
        }

        public void ExecScalar<T>(out T val)
        {
            try
            {
                if (string.IsNullOrEmpty(_cmd.CommandText))
                    throw new ArgumentNullException(nameof(_cmd.CommandText));
                OpenConnection();
                object scalar = _cmd.ExecuteScalar();
                val = DefaultIfDBNull<T>(scalar);
            }
            finally
            {
                Dispose();
            }
        }

        public Task ExecScalarAsync<T>(Action<T> action)
        {
            if (string.IsNullOrEmpty(_cmd.CommandText))
                throw new ArgumentNullException(nameof(_cmd.CommandText));
            return ExecScalarAsync(action, CancellationToken.None);
        }

        public async Task ExecScalarAsync<T>(Action<T> action, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrEmpty(_cmd.CommandText))
                    throw new ArgumentNullException(nameof(_cmd.CommandText));
                await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
                object scalar = await _cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
                T val = DefaultIfDBNull<T>(scalar);
                action(val);
            }
            finally
            {
                Dispose();
            }
        }

        public void Dispose()
        {
            if (_cmd.Transaction == null && _cmd.Connection != null)
                _cmd.Connection.Close();
            _cmd.Dispose();
        }

        private OutputParam<T> AddOutputParamInner<T>(string name, T val, ParameterDirection direction, int size = 0, byte precision = 0, byte scale = 0)
        {
            DbParameter param = AddParamInner(name, val, direction, size, precision, scale);
            return new OutputParam<T>(param);
        }

        private DbParameter AddParamInner<T>(string name, T val, ParameterDirection direction, int size = 0, byte precision = 0, byte scale = 0)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            DbParameter param = _cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = (object)val ?? DBNull.Value;
            param.Direction = direction;
            param.DbType = DbTypeConverter.ConvertToDbType<T>();
            param.Size = size;
            param.Precision = precision;
            param.Scale = scale;

            _cmd.Parameters.Add(param);
            return param;
        }

        private void OpenConnection()
        {
            if (_cmd.Connection.State == ConnectionState.Closed)
            {
                _cmd.Connection.Open();
            }
        }

        private Task OpenConnectionAsync(CancellationToken cancellationToken)
        {
            return _cmd.Connection.State == ConnectionState.Closed ? _cmd.Connection.OpenAsync(cancellationToken) : Task.CompletedTask;
        }

        private static T DefaultIfDBNull<T>(object o)
        {
            return o == DBNull.Value ? default(T) : (T)o;
        }
    }
}
