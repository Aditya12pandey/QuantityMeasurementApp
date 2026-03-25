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

        public bool PerformComparison(QuantityDTO q1, QuantityDTO q2)
        {
            try
            {
                bool result = _service.Compare(q1, q2);
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

        public QuantityDTO? PerformConversion(QuantityDTO quantity,
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

        public QuantityDTO? PerformAddition(QuantityDTO q1, QuantityDTO q2,
                                            QuantityDTO targetUnitDto)
        {
            try
            {
                QuantityDTO result = _service.Add(q1, q2, targetUnitDto);
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

        public QuantityDTO? PerformSubtraction(QuantityDTO q1, QuantityDTO q2,
                                               QuantityDTO targetUnitDto)
        {
            try
            {
                QuantityDTO result = _service.Subtract(q1, q2, targetUnitDto);
                Console.WriteLine($"{q1} - {q2} = {result}");
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

        public double PerformDivision(QuantityDTO q1, QuantityDTO q2)
        {
            try
            {
                double result = _service.Divide(q1, q2);
                Console.WriteLine($"{q1} / {q2} = {result}");
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

            foreach (QuantityEntity e in history)
            {
                if (e.IsError)
                {
                    Console.WriteLine(
                        $"  [{e.Timestamp:yyyy-MM-dd HH:mm:ss}] " +
                        $"{e.OperationType,-10} ERROR: {e.ErrorMessage}");
                }
                else
                {
                    string op2 = e.Operand2Value.HasValue
                        ? $" | {e.Operand2Value} {e.Operand2Unit}"
                        : string.Empty;
                    Console.WriteLine(
                        $"  [{e.Timestamp:yyyy-MM-dd HH:mm:ss}] " +
                        $"{e.OperationType,-10} " +
                        $"{e.Operand1Value} {e.Operand1Unit}" +
                        $"{op2} => {e.ResultValue} {e.ResultUnit}");
                }
            }

            Console.WriteLine(new string('-', 60));
            Console.WriteLine("  History persisted to SQL Server.");
        }
    }
}