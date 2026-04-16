using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace QuantityMeasurementAppRepository.Config
{
    public class ApplicationConfig
    {
        private readonly Dictionary<string, string> _properties
            = new Dictionary<string, string>();

        private static ApplicationConfig _instance;
        private static readonly object _lock = new object();

        public static ApplicationConfig Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new ApplicationConfig();
                    return _instance;
                }
            }
        }

        private ApplicationConfig()
        {
            LoadDefaults();
            LoadFromAppProperties();
            LoadFromAppSettings();
            LoadFromEnvironment();
        }

        private void LoadDefaults()
        {
            _properties["db.url"] =
                "Server=(localdb)\\MSSQLLocalDB;" +
                "Database=QuantityMeasurementDB;" +
                "Trusted_Connection=True;" +
                "TrustServerCertificate=True;";
            _properties["db.pool.size"]    = "5";
            _properties["repository.type"] = "database";
            _properties["db.schema.auto"]  = "false";
        }

        private void LoadFromAppProperties()
        {
            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "app.properties");
            if (!File.Exists(path)) return;

            foreach (string line in File.ReadAllLines(path))
            {
                if (string.IsNullOrWhiteSpace(line) ||
                    line.TrimStart().StartsWith("#")) continue;
                int eq = line.IndexOf('=');
                if (eq < 0) continue;
                _properties[line.Substring(0, eq).Trim()] =
                    line.Substring(eq + 1).Trim();
            }
        }

        private void LoadFromAppSettings()
        {
            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
            if (!File.Exists(path)) return;

            try
            {
                IConfiguration cfg = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: true)
                    .Build();

                string conn = cfg.GetConnectionString("DefaultConnection");
                if (!string.IsNullOrWhiteSpace(conn))
                    _properties["db.url"] = conn;

                string repo = cfg["AppSettings:RepositoryType"];
                if (!string.IsNullOrWhiteSpace(repo))
                    _properties["repository.type"] = repo;

                string pool = cfg["AppSettings:PoolSize"];
                if (!string.IsNullOrWhiteSpace(pool))
                    _properties["db.pool.size"] = pool;

                string schema = cfg["AppSettings:SchemaAutoCreate"];
                if (!string.IsNullOrWhiteSpace(schema))
                    _properties["db.schema.auto"] = schema;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(
                    $"[Config] appsettings.json load failed: {ex.Message}");
            }
        }

        private void LoadFromEnvironment()
        {
            foreach (var key in new List<string>(_properties.Keys))
            {
                string env = Environment.GetEnvironmentVariable(
                    key.Replace(".", "_").ToUpperInvariant());
                if (!string.IsNullOrEmpty(env))
                    _properties[key] = env;
            }
        }

        public string Get(string key, string def = "")
            => _properties.TryGetValue(key, out string v) ? v : def;

        public int  GetInt(string key, int def = 0)
            => int.TryParse(Get(key), out int v) ? v : def;

        public bool GetBool(string key, bool def = false)
            => bool.TryParse(Get(key), out bool v) ? v : def;

        public string ConnectionString => Get("db.url");
        public int    PoolSize         => GetInt("db.pool.size", 5);
        public string RepositoryType   => Get("repository.type", "cache");
        public bool   AutoCreateSchema => GetBool("db.schema.auto", false);

        public void PrintConfig()
        {
            Console.WriteLine("[Config] Loaded settings:");
            Console.WriteLine($"  Repository  : {RepositoryType}");
            Console.WriteLine($"  Pool size   : {PoolSize}");
            Console.WriteLine($"  Connection  : {ConnectionString}");
        }
    }
}