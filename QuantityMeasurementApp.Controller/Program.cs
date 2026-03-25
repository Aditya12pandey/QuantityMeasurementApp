using System;
using QuantityMeasurementAppBusiness;
using QuantityMeasurementAppRepository.Config;
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

            IQuantityMeasurementRepository repository =
                config.RepositoryType.Equals("database",
                    StringComparison.OrdinalIgnoreCase)
                    ? (IQuantityMeasurementRepository)
                      new QuantityMeasurementDatabaseRepository()
                    :  QuantityMeasurementCacheRepository.Instance;

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
