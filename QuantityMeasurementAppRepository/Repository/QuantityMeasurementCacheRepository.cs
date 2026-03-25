using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using QuantityMeasurementAppEntity.Entity;
using QuantityMeasurementAppRepository.Interfaces;

namespace QuantityMeasurementAppRepository.Repository
{
    public sealed class QuantityMeasurementCacheRepository : IQuantityMeasurementRepository

    {
        private static QuantityMeasurementCacheRepository _instance;
        private static readonly object _lock = new object();

        public static QuantityMeasurementCacheRepository Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new QuantityMeasurementCacheRepository();
                    return _instance;
                }
            }
        }

        private readonly List<QuantityEntity> _cache;
        private readonly string _filePath;

        private static readonly JsonSerializerOptions _jsonOptions =
            new JsonSerializerOptions { WriteIndented = true };

        private QuantityMeasurementCacheRepository()
        {
            _filePath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "measurement_history.json");
            _cache = new List<QuantityEntity>();
            LoadFromDisk();
        }

        public void Save(QuantityEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            lock (_lock) { _cache.Add(entity); SaveToDisk(); }
        }

        public List<QuantityEntity> GetAll()
        {
            lock (_lock) { return new List<QuantityEntity>(_cache); }
        }

        public List<QuantityEntity> GetAllMeasurements()
        {
            return GetAll();
        }

        public QuantityEntity GetById(string operationId)
        {
            lock (_lock)
            {
                for (int i = 0; i < _cache.Count; i++)
                    if (_cache[i].OperationId == operationId)
                        return _cache[i];
                return null;
            }
        }

        public List<QuantityEntity> GetByOperationType(string operationType)
        {
            lock (_lock)
            {
                var result = new List<QuantityEntity>();
                for (int i = 0; i < _cache.Count; i++)
                    if (_cache[i].OperationType == operationType)
                        result.Add(_cache[i]);
                return result;
            }
        }

        public int GetCount()
        {
            lock (_lock) { return _cache.Count; }
        }

        public void Clear()
        {
            lock (_lock)
            {
                _cache.Clear();
                if (File.Exists(_filePath)) File.Delete(_filePath);
            }
        }

        private void SaveToDisk()
        {
            try
            {
                string json = JsonSerializer.Serialize(_cache, _jsonOptions);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Repository] Could not save: {ex.Message}");
            }
        }

        private void LoadFromDisk()
        {
            try
            {
                if (!File.Exists(_filePath)) return;
                string json = File.ReadAllText(_filePath);
                var saved = JsonSerializer.Deserialize<List<QuantityEntity>>(json, _jsonOptions);
                if (saved != null)
                    for (int i = 0; i < saved.Count; i++)
                        _cache.Add(saved[i]);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[Repository] Could not load: {ex.Message}");
            }
        }
        public List<QuantityEntity> GetByMeasurementType(string measurementType)
        {
            lock (_lock)
            {
                var result = new List<QuantityEntity>();
                for (int i = 0; i < _cache.Count; i++)
                    if (_cache[i].Operand1Measurement == measurementType)
                        result.Add(_cache[i]);
                return result;
            }
        }
    }
}