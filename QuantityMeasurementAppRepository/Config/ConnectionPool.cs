using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace QuantityMeasurementAppRepository.Config
{
    public sealed class ConnectionPool : IDisposable
    {
        private readonly string _connectionString;
        private readonly int _maxSize;
        private readonly Stack<SqlConnection> _available
            = new Stack<SqlConnection>();
        private int _totalCreated = 0;
        private bool _disposed = false;
        private readonly object _lock = new object();

        private static ConnectionPool _instance;
        private static readonly object _instanceLock = new object();

        public static ConnectionPool Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null || _instance._disposed)
                    {
                        var cfg = ApplicationConfig.Instance;
                        _instance = new ConnectionPool(
                            cfg.ConnectionString, cfg.PoolSize);
                    }
                    return _instance;
                }
            }
        }

        public ConnectionPool(string connectionString, int maxSize = 5)
        {
            _connectionString = connectionString;
            _maxSize = maxSize;
            WarmUp();
        }

        private void WarmUp()
        {
            for (int i = 0; i < _maxSize; i++)
                _available.Push(CreateConnection());
        }

        private SqlConnection CreateConnection()
        {
            var conn = new SqlConnection(_connectionString);
            conn.Open();
            _totalCreated++;
            return conn;
        }

        public SqlConnection Acquire()
        {
            lock (_lock)
            {
                if (_available.Count > 0)
                    return _available.Pop();
                if (_totalCreated < _maxSize * 2)
                    return CreateConnection();
                throw new InvalidOperationException(
                    "Connection pool exhausted.");
            }
        }

        public void Release(SqlConnection conn)
        {
            if (conn == null) return;
            lock (_lock)
            {
                if (_available.Count < _maxSize)
                    _available.Push(conn);
                else
                {
                    conn.Close();
                    conn.Dispose();
                    _totalCreated--;
                }
            }
        }

        public string GetStatistics()
        {
            lock (_lock)
            {
                return $"Pool[available={_available.Count}, " +
                       $"total={_totalCreated}, max={_maxSize}]";
            }
        }

        public void Dispose()
        {
            lock (_lock)
            {
                if (_disposed) return;
                while (_available.Count > 0)
                {
                    var c = _available.Pop();
                    c.Close();
                    c.Dispose();
                }
                _disposed = true;
            }
        }
    }
}