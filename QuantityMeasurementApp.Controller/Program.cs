using System;
using QuantityMeasurementAppBusiness;
using QuantityMeasurementAppRepository.Interfaces;
using QuantityMeasurementAppRepository.Repository;

namespace QuantityMeasurementApp.Controller
{
    public sealed class QuantityMeasurementApp
    {
        private static QuantityMeasurementApp _instance;
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
            IQuantityMeasurementRepository repository = QuantityMeasurementCacheRepository.Instance;

            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repository);

            QuantityMeasurementController controller = new QuantityMeasurementController(service, repository);
            Menu.RunAllDemonstrations(controller);

            Console.WriteLine();
            Console.WriteLine($"[Repository] {repository.GetAllMeasurements().Count} " + $"operation(s) recorded.");
        }
    }
}