using Test.Core.Configuration;
using Test.Entities.Setting;
using Generic.Core.Services;
using Generic.Core.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Npgsql;

namespace Test.Data.Context
{
    public class DBContextFactory : IDBContextFactory
    {
        private readonly IConfigurationManager _configurationManager;
        private readonly IEncryptionService _encryptionService;
        public string ConnectionString { get; }
        public string ServerID { get; }
        public DBContextFactory(IConfigurationManager configurationManager, String ConnectionID, IEncryptionService encryptionService)
        {
            try
            {
                _configurationManager = configurationManager;
                _encryptionService = encryptionService;
                ServerID = _configurationManager.ServerID;
                if (!string.IsNullOrWhiteSpace(ConnectionID))
                    ServerID = ConnectionID;
                DBConnectionPath clsMultiPath = new DBConnectionPath(_encryptionService);
                var hostConfig = _configurationManager.AppSetting.HostConfig[0];
                string _ServerFileName = Convert.ToString(hostConfig.DBServerFile);
                string ODBCconnection = (string)clsMultiPath.Connect(ServerID, _ServerFileName);

                ConnectionString = _ODBCToSQLServerConnection(ODBCconnection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IDataContext GetInstance()
        {
            try
            {
                String _connectionString = ConnectionString;
                var opt = new DbContextOptionsBuilder<HRISContext>();
                opt.UseNpgsql(_connectionString);

                return new HRISContext(opt.Options);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private string _ODBCToSQLServerConnection(string ODBCConnection)
        {
            try
            {
                NpgsqlConnectionStringBuilder builder = new NpgsqlConnectionStringBuilder()
                {
                    Multiplexing = false,
                    PersistSecurityInfo = true,
                    ApplicationName = _configurationManager.AppSetting.AppCode,
                    Port = 5432

                };
                string[] arrODBCConn = ODBCConnection.Split(';');
                if (Array.Exists(arrODBCConn, E => E.ToLower().Contains("driver")))
                    arrODBCConn[Array.FindIndex(arrODBCConn, row => row.ToLower().Contains("driver"))] = "";
                if (Array.Exists(arrODBCConn, E => E.ToLower().Contains("server")))
                {
                    string DataSource = arrODBCConn[Array.FindIndex(arrODBCConn, row => row.ToLower().Contains("server"))];
                    DataSource = DataSource.Remove(0, DataSource.IndexOf("=") + 1);
                    builder.Host = DataSource;
                }
                if (Array.Exists(arrODBCConn, E => E.ToLower().Contains("database")))
                {
                    string database = arrODBCConn[Array.FindIndex(arrODBCConn, row => row.ToLower().Contains("database"))];
                    database = database.Remove(0, database.IndexOf("=") + 1);
                    builder.Database = database;
                }
                if (Array.Exists(arrODBCConn, E => E.ToLower().Contains("uid=")))
                {
                    string uid = arrODBCConn[Array.FindIndex(arrODBCConn, row => row.ToLower().Contains("uid="))];
                    uid = uid.Remove(0, uid.IndexOf("=") + 1);
                    builder.Username = uid;
                    builder.IntegratedSecurity = false;
                }
                if (Array.Exists(arrODBCConn, E => E.ToLower().Contains("pwd=")))
                {
                    string pwd = arrODBCConn[Array.FindIndex(arrODBCConn, row => row.ToLower().Contains("pwd="))];
                    pwd = pwd.Remove(0, pwd.IndexOf("=") + 1);
                    builder.Password = pwd;
                    builder.IntegratedSecurity = false;
                }
                if (Array.Exists(arrODBCConn, E => E.ToLower().Contains("password=")))
                {
                    string pwd = arrODBCConn[Array.FindIndex(arrODBCConn, row => row.ToLower().Contains("password="))];
                    pwd = pwd.Remove(0, pwd.IndexOf("=") + 1);
                    builder.Password = pwd;
                    builder.IntegratedSecurity = false;
                }
                if (Array.Exists(arrODBCConn, E => E.ToLower().Contains("trusted_connection=yes")))
                {
                    builder.IntegratedSecurity = true;
                }
                return builder.ConnectionString;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
