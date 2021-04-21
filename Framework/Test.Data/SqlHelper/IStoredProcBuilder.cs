﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Test.Data.SqlHelper
{
    /// <summary>
    /// Stored procedure builder.
    /// </summary>
    public interface IStoredProcBuilder : IDisposable
    {
        List<Type> ResultSets { get; set; }
        IStoredProcBuilder StoredProc(string name);
        IStoredProcBuilder ClearResult();
        IStoredProcBuilder ClearParam();
        /// <summary>
        /// Add input parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="val">Value of the parameter.</param>
        IStoredProcBuilder AddParam<T>(string name, T val);

        /// <summary>
        /// Add input/output parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="val">Value of the parameter.</param>
        /// <param name="outParam">Created parameter. Value will be populated after calling <see cref="Exec(Action{DbDataReader})"/>.</param>
        IStoredProcBuilder AddParam<T>(string name, T val, out IOutParam<T> outParam);

        /// <summary>
        /// Add input/output parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="val">Value of the parameter.</param>
        /// <param name="outParam">Created parameter. Value will be populated after calling <see cref="Exec(Action{DbDataReader})"/>.</param>
        /// <param name="size">Number of decimal places to which <see cref="IOutParam{T}.Value"/> is resolved.</param>
        /// <param name="precision">Number of digits used to represent the <see cref="IOutParam{T}.Value"/> property.</param>
        /// <param name="scale">Maximum size, in bytes, of the data within the column.</param>
        IStoredProcBuilder AddParam<T>(string name, T val, out IOutParam<T> outParam, int size = 0, byte precision = 0, byte scale = 0);

        /// <summary>
        /// Add output parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="outParam">Created parameter. Value will be populated after calling <see cref="Exec(Action{DbDataReader})"/>.</param>
        IStoredProcBuilder AddParam<T>(string name, out IOutParam<T> outParam);

        /// <summary>
        /// Add output parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="outParam">Created parameter. Value will be populated after calling <see cref="Exec(Action{DbDataReader})"/>.</param>
        /// <param name="size">Number of decimal places to which <see cref="IOutParam{T}.Value"/> is resolved.</param>
        /// <param name="precision">Number of digits used to represent the <see cref="IOutParam{T}.Value"/> property.</param>
        /// <param name="scale">Maximum size, in bytes, of the data within the column.</param>
        IStoredProcBuilder AddParam<T>(string name, out IOutParam<T> outParam, int size = 0, byte precision = 0, byte scale = 0);

        /// <summary>
        /// Add pre configured DB query execution parameter directly command.
        /// </summary>
        /// <param name="parameter">DB query execution parameter <see cref="DbParameter"/>.</param>
        IStoredProcBuilder AddParam(DbParameter parameter);

        /// <summary>
        /// Add return value parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="retParam">Created parameter. Value will be populated after calling <see cref="Exec(Action{DbDataReader})"/>.</param>
        IStoredProcBuilder ReturnValue<T>(out IOutParam<T> retParam);

        /// <summary>
        /// Set the wait time before terminating the attempt to execute the stored procedure and generating an error.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IStoredProcBuilder SetTimeout(int timeout);

        /// <summary>
        /// Execute the stored procedure.
        /// </summary>
        /// <param name="action">Actions to do with the result sets.</param>
        void Exec(Action<DbDataReader> action);

        /// <summary>
        /// Execute the stored procedure with multiple result sets.
        /// </summary>
        /// <returns></returns>
        List<IEnumerable> ExecMulti();
        /// <summary>
        /// Execute the stored procedure
        /// </summary>
        /// <param name="action">Actions to do with the result sets.</param>
        Task ExecAsync(Func<DbDataReader, Task> action);

        /// <summary>
        /// Execute the stored procedure.
        /// </summary>
        /// <param name="action">Actions to do with the result sets.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="TaskCanceledException">When <paramref name="cancellationToken"/> was cancelled.</exception>
        Task ExecAsync(Func<DbDataReader, Task> action, CancellationToken cancellationToken);

        /// <summary>
        /// Execute the stored procedure.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        int ExecNonQuery();

        /// <summary>
        /// Execute the stored procedure.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        Task<int> ExecNonQueryAsync();

        /// <summary>
        /// Execute the stored procedure.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="TaskCanceledException">When <paramref name="cancellationToken"/> was cancelled.</exception>
        /// <returns>The number of rows affected.</returns>
        Task<int> ExecNonQueryAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Execute the stored procedure and return the first column of the first row.
        /// </summary>
        /// <typeparam name="T">Type of the scalar value.</typeparam>
        /// <param name="val">The first column of the first row in the result set.</param>
        void ExecScalar<T>(out T val);

        /// <summary>
        /// Execute the stored procedure and return the first column of the first row.
        /// </summary>
        /// <typeparam name="T">Type of the scalar value.</typeparam>
        /// <param name="action">Action applied to the first column of the first row in the result set.</param>
        Task ExecScalarAsync<T>(Action<T> action);

        /// <summary>
        /// Execute the stored procedure and return the first column of the first row.
        /// </summary>
        /// <typeparam name="T">Type of the scalar value.</typeparam>.
        /// <param name="action">Action applied to the first column of the first row in the result set.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="TaskCanceledException">When <paramref name="cancellationToken"/> was cancelled.</exception>
        Task ExecScalarAsync<T>(Action<T> action, CancellationToken cancellationToken);

    }
}
