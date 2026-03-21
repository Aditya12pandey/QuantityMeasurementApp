using System;
using System.Collections.Generic;
using QuantityMeasurementAppBusiness;
using QuantityMeasurementAppBusiness.Exceptions;
using QuantityMeasurementAppEntity.DTOs;
using QuantityMeasurementAppEntity.Entity;
using QuantityMeasurementAppRepository.Interfaces;

namespace QuantityMeasurementApp.Controller
{
    public class QuantityMeasurementController
    {
        private readonly IQuantityMeasurementService    _service;
        private readonly IQuantityMeasurementRepository _repository;

        public QuantityMeasurementController(
            IQuantityMeasurementService    service,
            IQuantityMeasurementRepository repository)
        {
            _service    = service
                ?? throw new ArgumentNullException(nameof(service));
            _repository = repository
                ?? throw new ArgumentNullException(nameof(repository));
        }

        // POST /api/quantity/compare
        public bool PerformComparison(QuantityDTO quantity1, QuantityDTO quantity2)
        {
            try
            {
                bool result = _service.Compare(quantity1, quantity2);
                Console.WriteLine(result);
                return result;
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine($"[Compare Error] {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Unexpected Error] {ex.Message}");
                return false;
            }
        }

        // POST /api/quantity/convert
        public QuantityDTO PerformConversion(QuantityDTO quantity,
                                             QuantityDTO targetUnitDto)
        {
            try
            {
                QuantityDTO result = _service.Convert(quantity, targetUnitDto);
                Console.WriteLine(result);
                return result;
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine($"[Convert Error] {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Unexpected Error] {ex.Message}");
                return null;
            }
        }

        // POST /api/quantity/add
        public QuantityDTO PerformAddition(QuantityDTO quantity1,
                                           QuantityDTO quantity2,
                                           QuantityDTO targetUnitDto)
        {
            try
            {
                QuantityDTO result = _service.Add(quantity1, quantity2, targetUnitDto);
                Console.WriteLine(result);
                return result;
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Add caught: {ex.Message}");
                return null;
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine($"[Add Error] {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Unexpected Error] {ex.Message}");
                return null;
            }
        }

        // POST /api/quantity/subtract
        public QuantityDTO PerformSubtraction(QuantityDTO quantity1,
                                              QuantityDTO quantity2,
                                              QuantityDTO targetUnitDto)
        {
            try
            {
                QuantityDTO result = _service.Subtract(quantity1, quantity2,
                                                       targetUnitDto);
                Console.WriteLine($"{quantity1} - {quantity2} = {result}");
                return result;
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Subtract caught: {ex.Message}");
                return null;
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine($"[Subtract Error] {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Unexpected Error] {ex.Message}");
                return null;
            }
        }

        // POST /api/quantity/divide
        public double PerformDivision(QuantityDTO quantity1, QuantityDTO quantity2)
        {
            try
            {
                double result = _service.Divide(quantity1, quantity2);
                Console.WriteLine($"{quantity1} / {quantity2} = {result}");
                return result;
            }
            catch (NotSupportedException ex)
            {
                Console.WriteLine($"Divide caught: {ex.Message}");
                return double.NaN;
            }
            catch (QuantityMeasurementException ex)
            {
                Console.WriteLine($"[Divide Error] {ex.Message}");
                return double.NaN;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Unexpected Error] {ex.Message}");
                return double.NaN;
            }
        }

        // GET /api/quantity/history
        public void ShowHistory()
        {
            List<QuantityEntity> history = _repository.GetAllMeasurements();

            if (history.Count == 0)
            {
                Console.WriteLine("  No operations recorded yet.");
                return;
            }

            Console.WriteLine($"  Total operations recorded: {history.Count}");
            Console.WriteLine(new string('-', 60));

            foreach (QuantityEntity entity in history)
            {
                if (entity.IsError)
                {
                    Console.WriteLine(
                        $"  [{entity.Timestamp:yyyy-MM-dd HH:mm:ss}] " +
                        $"{entity.OperationType,-10} ERROR: {entity.ErrorMessage}");
                }
                else
                {
                    string op2 = entity.Operand2Value.HasValue
                        ? $" | {entity.Operand2Value} {entity.Operand2Unit}"
                        : string.Empty;

                    Console.WriteLine(
                        $"  [{entity.Timestamp:yyyy-MM-dd HH:mm:ss}] " +
                        $"{entity.OperationType,-10} " +
                        $"{entity.Operand1Value} {entity.Operand1Unit}" +
                        $"{op2} => {entity.ResultValue} {entity.ResultUnit}");
                }
            }

            Console.WriteLine(new string('-', 60));
            Console.WriteLine("  History file: measurement_history.json");
        }
    }
}