using System;
using Microsoft.EntityFrameworkCore;
using QuantityMeasurementAppBusiness;
using QuantityMeasurementAppRepository.Config;
using QuantityMeasurementAppRepository.Data;
using QuantityMeasurementAppRepository.Interfaces;
using QuantityMeasurementAppRepository.Repository;

namespace QuantityMeasurementApp.Controller
{
    public sealed class QuantityMeasurementApp
    {
        private static QuantityMeasurementApp? _instance;
        private static readonly object _lock = new object();

        public static QuantityMeasurementApp Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new QuantityMeasurementApp();
                    return _instance;
                }
            }
        }

        private QuantityMeasurementApp() { }

        public static void Main(string[] args)
        {
            var config = ApplicationConfig.Instance;
            config.PrintConfig();

            if (config.RepositoryType.Equals("database",
                    StringComparison.OrdinalIgnoreCase))
            {
                var options = new DbContextOptionsBuilder<QuantityMeasurementDbContext>()
                    .UseSqlServer(config.ConnectionString)
                    .Options;

                using var dbContext = new QuantityMeasurementDbContext(options);
                var repository = new QuantityMeasurementEfRepository(dbContext);
                RunWithRepository(config, repository);
            }
            else
            {
                IQuantityMeasurementRepository repository =
                    QuantityMeasurementCacheRepository.Instance;
                RunWithRepository(config, repository);
            }
        }

        private static void RunWithRepository(
            ApplicationConfig config,
            IQuantityMeasurementRepository repository)
        {
            Console.WriteLine(
                $"[App] Repository : {repository.GetType().Name}");
            Console.WriteLine(
                $"[App] Pool stats : {repository.GetPoolStatistics()}");

            IQuantityMeasurementService service =
                new QuantityMeasurementServiceImpl(repository);

            QuantityMeasurementController controller =
                new QuantityMeasurementController(service, repository);

            Menu.RunAllDemonstrations(controller);

            Console.WriteLine();
            Console.WriteLine(
                $"[Repository] {repository.GetAllMeasurements().Count} " +
                $"operation(s) recorded.");
            Console.WriteLine(
                $"[Pool] {repository.GetPoolStatistics()}");

            repository.ReleaseResources();
        }
    }
}
