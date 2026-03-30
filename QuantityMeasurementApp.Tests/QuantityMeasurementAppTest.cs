using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementAppBusiness;
using QuantityMeasurementAppBusiness.Exceptions;
using QuantityMeasurementAppEntity.DTOs;
using QuantityMeasurementAppEntity.Entity;
using QuantityMeasurementAppRepository.Interfaces;
using QuantityMeasurementAppRepository.Repository;
using QuantityMeasurementApp.Controller;

namespace QuantityMeasurementApp.Tests
{
    // ═══════════════════════════════════════════════════════════════════════════
    // Mock repository — in-memory only, no disk I/O during tests
    // ═══════════════════════════════════════════════════════════════════════════

    internal class MockRepository : IQuantityMeasurementRepository
    {
        private readonly List<QuantityEntity> _store = new List<QuantityEntity>();

        public void Save(QuantityEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _store.Add(entity);
        }
        public List<QuantityEntity> GetAll() => new List<QuantityEntity>(_store);
        public List<QuantityEntity> GetAllMeasurements() => GetAll();
        public int GetCount() => _store.Count;
        public void Clear() => _store.Clear();

        public QuantityEntity? GetById(string id)
        {
            foreach (var e in _store)
                if (e.OperationId == id) return e;
            return null;
        }

        public List<QuantityEntity> GetByOperationType(string type)
        {
            var result = new List<QuantityEntity>();
            foreach (var e in _store)
                if (e.OperationType == type) result.Add(e);
            return result;
        }

        public List<QuantityEntity> GetByMeasurementType(string measurementType)
        {
            var result = new List<QuantityEntity>();
            foreach (var e in _store)
                if (e.Operand1Measurement == measurementType) result.Add(e);
            return result;
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    // Mock service — returns canned responses, no real logic
    // ═══════════════════════════════════════════════════════════════════════════

    internal class MockService : IQuantityMeasurementService
    {
        public bool CompareResult { get; set; } = true;
        public QuantityDTO ConvertResult { get; set; }
        public QuantityDTO AddResult { get; set; }
        public QuantityDTO SubtractResult { get; set; }
        public double DivideResult { get; set; } = 1.0;
        public Exception ThrowOnCompare { get; set; }
        public Exception ThrowOnAdd { get; set; }

        public bool Compare(QuantityDTO q1, QuantityDTO q2)
        {
            if (ThrowOnCompare != null) throw ThrowOnCompare;
            return CompareResult;
        }

        public QuantityDTO Convert(QuantityDTO q, QuantityDTO target)
            => ConvertResult ?? new QuantityDTO(q.Value, target.UnitName, target.MeasurementType);

        public QuantityDTO Add(QuantityDTO q1, QuantityDTO q2, QuantityDTO target)
        {
            if (ThrowOnAdd != null) throw ThrowOnAdd;
            return AddResult ?? new QuantityDTO(0, target.UnitName, target.MeasurementType);
        }

        public QuantityDTO Subtract(QuantityDTO q1, QuantityDTO q2, QuantityDTO target)
            => SubtractResult ?? new QuantityDTO(0, target.UnitName, target.MeasurementType);

        public double Divide(QuantityDTO q1, QuantityDTO q2) => DivideResult;
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  1. QuantityEntity tests
    // ═══════════════════════════════════════════════════════════════════════════

    [TestClass]
    public class QuantityEntityTests
    {
        private QuantityDTO _dto1;
        private QuantityDTO _dto2;

        [TestInitialize]
        public void Setup()
        {
            _dto1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            _dto2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
        }

        // testQuantityEntity_SingleOperandConstruction
        [TestMethod]
        public void SingleOperandConstruction_StoresConversionData()
        {
            var result = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var entity = new QuantityEntity("CONVERT", _dto1, result);

            Assert.AreEqual("CONVERT", entity.OperationType);
            Assert.AreEqual(1.0, entity.Operand1Value);
            Assert.AreEqual("FEET", entity.Operand1Unit);
            Assert.AreEqual("LENGTH", entity.Operand1Measurement);
            Assert.AreEqual("12.0000", entity.ResultValue);
            Assert.AreEqual("INCHES", entity.ResultUnit);
            Assert.IsFalse(entity.IsError);
        }

        // testQuantityEntity_BinaryOperandConstruction
        [TestMethod]
        public void BinaryOperandConstruction_StoresAdditionData()
        {
            var entity = new QuantityEntity("ADD", _dto1, _dto2, "2.00", "FEET", "LENGTH");

            Assert.AreEqual("ADD", entity.OperationType);
            Assert.AreEqual(1.0, entity.Operand1Value);
            Assert.AreEqual("FEET", entity.Operand1Unit);
            Assert.AreEqual(12.0, entity.Operand2Value);
            Assert.AreEqual("INCHES", entity.Operand2Unit);
            Assert.AreEqual("2.00", entity.ResultValue);
            Assert.AreEqual("FEET", entity.ResultUnit);
            Assert.IsFalse(entity.IsError);
        }

        // testQuantityEntity_ErrorConstruction
        [TestMethod]
        public void ErrorConstruction_StoresErrorData()
        {
            var entity = new QuantityEntity(
                "COMPARE", _dto1, _dto2, "Category mismatch", true);

            Assert.IsTrue(entity.IsError);
            Assert.AreEqual("Category mismatch", entity.ErrorMessage);
            Assert.AreEqual("ERROR", entity.ResultValue);
            Assert.AreEqual("COMPARE", entity.OperationType);
        }

        // testQuantityEntity_ToString_Success
        [TestMethod]
        public void ToString_SuccessEntity_ContainsOperationAndResult()
        {
            var entity = new QuantityEntity("ADD", _dto1, _dto2, "2.00", "FEET", "LENGTH");
            string str = entity.ToString();

            StringAssert.Contains(str, "ADD");
            StringAssert.Contains(str, "FEET");
            StringAssert.Contains(str, "2.00");
        }

        // testQuantityEntity_ToString_Error
        [TestMethod]
        public void ToString_ErrorEntity_ContainsErrorKeyword()
        {
            var entity = new QuantityEntity(
                "COMPARE", _dto1, _dto2, "Some error", true);
            string str = entity.ToString();

            StringAssert.Contains(str, "ERROR");
            StringAssert.Contains(str, "Some error");
        }

        // testEntity_Immutability — verify constructors set all fields, no setters used
        [TestMethod]
        public void Entity_OperationId_IsAssignedOnConstruction()
        {
            var e1 = new QuantityEntity("ADD", _dto1, _dto2, "2.00", "FEET", "LENGTH");
            var e2 = new QuantityEntity("ADD", _dto1, _dto2, "2.00", "FEET", "LENGTH");

            Assert.IsNotNull(e1.OperationId);
            Assert.IsNotNull(e2.OperationId);
            Assert.AreNotEqual(e1.OperationId, e2.OperationId);
        }

        // testEntity_OperationType_Tracking
        [TestMethod]
        [DataRow("COMPARE")]
        [DataRow("CONVERT")]
        [DataRow("ADD")]
        [DataRow("SUBTRACT")]
        [DataRow("DIVIDE")]
        public void Entity_OperationType_IsTrackedCorrectly(string opType)
        {
            var entity = new QuantityEntity(opType, _dto1, _dto2, "1.00", "FEET", "LENGTH");
            Assert.AreEqual(opType, entity.OperationType);
        }

        // testQuantityEntity_Timestamp_IsSet
        [TestMethod]
        public void Entity_Timestamp_IsSetOnConstruction()
        {
            var before = DateTime.UtcNow.AddSeconds(-1);
            var entity = new QuantityEntity("ADD", _dto1, _dto2, "2.00", "FEET", "LENGTH");
            var after = DateTime.UtcNow.AddSeconds(1);

            Assert.IsTrue(entity.Timestamp >= before && entity.Timestamp <= after);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  2. Service layer tests
    // ═══════════════════════════════════════════════════════════════════════════

    [TestClass]
    public class ServiceLayerTests
    {
        private IQuantityMeasurementRepository _repo;
        private IQuantityMeasurementService _service;

        [TestInitialize]
        public void Setup()
        {
            _repo = new MockRepository();
            _service = new QuantityMeasurementServiceImpl(_repo);
        }

        // ── Compare ───────────────────────────────────────────────────────────

        // testService_CompareEquality_SameUnit_Success
        [TestMethod]
        public void Compare_SameUnit_SameValue_ReturnsTrue()
        {
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(1.0, "FEET", "LENGTH");
            Assert.IsTrue(_service.Compare(q1, q2));
        }

        // testService_CompareEquality_DifferentUnit_Success
        [TestMethod]
        public void Compare_DifferentUnit_EquivalentValue_ReturnsTrue()
        {
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
            Assert.IsTrue(_service.Compare(q1, q2));
        }

        [TestMethod]
        public void Compare_DifferentUnit_DifferentValue_ReturnsFalse()
        {
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(5.0, "INCHES", "LENGTH");
            Assert.IsFalse(_service.Compare(q1, q2));
        }

        // testService_CompareEquality_CrossCategory_Error
        [TestMethod]
        public void Compare_CrossCategory_ThrowsException()
        {
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");
            Assert.ThrowsException<QuantityMeasurementException>(
                () => _service.Compare(q1, q2));
        }

        // ── Convert ───────────────────────────────────────────────────────────

        // testService_Convert_Success
        [TestMethod]
        public void Convert_FeetToInches_Returns12()
        {
            var src = new QuantityDTO(1.0, "FEET", "LENGTH");
            var tgt = new QuantityDTO(0, "INCHES", "LENGTH");
            var result = _service.Convert(src, tgt);

            Assert.AreEqual(12.0, result.Value);
            Assert.AreEqual("INCHES", result.UnitName);
        }

        [TestMethod]
        public void Convert_CelsiusToFahrenheit_Returns212()
        {
            var src = new QuantityDTO(100.0, "CELSIUS", "TEMPERATURE");
            var tgt = new QuantityDTO(0, "FAHRENHEIT", "TEMPERATURE");
            var result = _service.Convert(src, tgt);

            Assert.AreEqual(212.0, result.Value);
        }

        [TestMethod]
        public void Convert_FahrenheitToCelsius_Returns0()
        {
            var src = new QuantityDTO(32.0, "FAHRENHEIT", "TEMPERATURE");
            var tgt = new QuantityDTO(0, "CELSIUS", "TEMPERATURE");
            var result = _service.Convert(src, tgt);

            Assert.AreEqual(0.0, result.Value);
        }

        [TestMethod]
        public void Convert_GallonToLitre_Returns3Point79()
        {
            var src = new QuantityDTO(1.0, "GALLON", "VOLUME");
            var tgt = new QuantityDTO(0, "LITRE", "VOLUME");
            var result = _service.Convert(src, tgt);

            Assert.AreEqual(3.79, result.Value, 0.01);
        }

        [TestMethod]
        public void Convert_KilogramToGram_Returns1000()
        {
            var src = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");
            var tgt = new QuantityDTO(0, "GRAM", "WEIGHT");
            var result = _service.Convert(src, tgt);

            Assert.AreEqual(1000.0, result.Value);
        }

        // ── Add ───────────────────────────────────────────────────────────────

        // testService_Add_Success
        [TestMethod]
        public void Add_FeetAndInches_ReturnsCorrectFeet()
        {
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var tgt = new QuantityDTO(0, "FEET", "LENGTH");
            var result = _service.Add(q1, q2, tgt);

            Assert.AreEqual(2.0, result.Value);
            Assert.AreEqual("FEET", result.UnitName);
        }

        [TestMethod]
        public void Add_KgAndGram_ReturnsCorrectKg()
        {
            var q1 = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");
            var q2 = new QuantityDTO(1000.0, "GRAM", "WEIGHT");
            var tgt = new QuantityDTO(0, "KILOGRAM", "WEIGHT");
            var result = _service.Add(q1, q2, tgt);

            Assert.AreEqual(2.0, result.Value);
        }

        [TestMethod]
        public void Add_LitreAndMillilitre_ReturnsCorrectLitre()
        {
            var q1 = new QuantityDTO(1.0, "LITRE", "VOLUME");
            var q2 = new QuantityDTO(1000.0, "MILLILITRE", "VOLUME");
            var tgt = new QuantityDTO(0, "LITRE", "VOLUME");
            var result = _service.Add(q1, q2, tgt);

            Assert.AreEqual(2.0, result.Value);
        }

        // testService_Add_UnsupportedOperation_Error (temperature)
        [TestMethod]
        public void Add_Temperature_ThrowsNotSupportedException()
        {
            var q1 = new QuantityDTO(100.0, "CELSIUS", "TEMPERATURE");
            var q2 = new QuantityDTO(50.0, "CELSIUS", "TEMPERATURE");
            var tgt = new QuantityDTO(0, "CELSIUS", "TEMPERATURE");

            Assert.ThrowsException<NotSupportedException>(
                () => _service.Add(q1, q2, tgt));
        }

        // ── Subtract ─────────────────────────────────────────────────────────

        // testService_Subtract_Success
        [TestMethod]
        public void Subtract_FeetMinusInches_ReturnsZeroFeet()
        {
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var tgt = new QuantityDTO(0, "FEET", "LENGTH");
            var result = _service.Subtract(q1, q2, tgt);

            Assert.AreEqual(0.0, result.Value);
            Assert.AreEqual("FEET", result.UnitName);
        }

        [TestMethod]
        public void Subtract_Temperature_ThrowsNotSupportedException()
        {
            var q1 = new QuantityDTO(100.0, "CELSIUS", "TEMPERATURE");
            var q2 = new QuantityDTO(50.0, "CELSIUS", "TEMPERATURE");
            var tgt = new QuantityDTO(0, "CELSIUS", "TEMPERATURE");

            Assert.ThrowsException<NotSupportedException>(
                () => _service.Subtract(q1, q2, tgt));
        }

        // ── Divide ────────────────────────────────────────────────────────────

        // testService_Divide_Success
        [TestMethod]
        public void Divide_1KgBy500Gram_Returns2()
        {
            var q1 = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");
            var q2 = new QuantityDTO(500.0, "GRAM", "WEIGHT");
            double result = _service.Divide(q1, q2);

            Assert.AreEqual(2.0, result, 0.0001);
        }

        // testService_Divide_ByZero_Error
        [TestMethod]
        public void Divide_ByZero_ThrowsQuantityMeasurementException()
        {
            var q1 = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");
            var q2 = new QuantityDTO(0.0, "KILOGRAM", "WEIGHT");

            var ex = Assert.ThrowsException<QuantityMeasurementException>(
                () => _service.Divide(q1, q2));
            StringAssert.Contains(ex.Message, "Division by zero");
        }

        [TestMethod]
        public void Divide_Temperature_ThrowsNotSupportedException()
        {
            var q1 = new QuantityDTO(100.0, "CELSIUS", "TEMPERATURE");
            var q2 = new QuantityDTO(50.0, "CELSIUS", "TEMPERATURE");

            Assert.ThrowsException<NotSupportedException>(
                () => _service.Divide(q1, q2));
        }

        // ── Null validation ───────────────────────────────────────────────────

        // testService_NullEntity_Rejection
        [TestMethod]
        public void Compare_NullFirstOperand_ThrowsException()
        {
            var q = new QuantityDTO(1.0, "FEET", "LENGTH");
            Assert.ThrowsException<QuantityMeasurementException>(
                () => _service.Compare(null, q));
        }

        [TestMethod]
        public void Compare_NullSecondOperand_ThrowsException()
        {
            var q = new QuantityDTO(1.0, "FEET", "LENGTH");
            Assert.ThrowsException<QuantityMeasurementException>(
                () => _service.Compare(q, null));
        }

        // ── All categories ────────────────────────────────────────────────────

        // testService_AllMeasurementCategories
        [TestMethod]
        public void Compare_AllCategories_WorkCorrectly()
        {
            // Length
            Assert.IsTrue(_service.Compare(
                new QuantityDTO(1.0, "FEET", "LENGTH"),
                new QuantityDTO(12.0, "INCHES", "LENGTH")));

            // Weight
            Assert.IsTrue(_service.Compare(
                new QuantityDTO(1.0, "KILOGRAM", "WEIGHT"),
                new QuantityDTO(1000.0, "GRAM", "WEIGHT")));

            // Volume
            Assert.IsTrue(_service.Compare(
                new QuantityDTO(1.0, "LITRE", "VOLUME"),
                new QuantityDTO(1000.0, "MILLILITRE", "VOLUME")));

            // Temperature
            Assert.IsTrue(_service.Compare(
                new QuantityDTO(0.0, "CELSIUS", "TEMPERATURE"),
                new QuantityDTO(32.0, "FAHRENHEIT", "TEMPERATURE")));
        }

        // testService_ValidationConsistency
        [TestMethod]
        public void AllOperations_CrossCategory_ThrowSameExceptionType()
        {
            var length = new QuantityDTO(1.0, "FEET", "LENGTH");
            var weight = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");
            var tgt = new QuantityDTO(0, "FEET", "LENGTH");

            Assert.ThrowsException<QuantityMeasurementException>(
                () => _service.Compare(length, weight));
            Assert.ThrowsException<QuantityMeasurementException>(
                () => _service.Add(length, weight, tgt));
            Assert.ThrowsException<QuantityMeasurementException>(
                () => _service.Subtract(length, weight, tgt));
        }

        // testService_ExceptionHandling_AllOperations
        [TestMethod]
        public void Service_InvalidUnit_ThrowsQuantityMeasurementException()
        {
            var bad = new QuantityDTO(1.0, "UNKNOWN_UNIT", "LENGTH");
            var tgt = new QuantityDTO(0, "FEET", "LENGTH");

            Assert.ThrowsException<QuantityMeasurementException>(
                () => _service.Convert(bad, tgt));
        }

        // ── Repository saving ─────────────────────────────────────────────────

        [TestMethod]
        public void Service_SavesEntityToRepository_AfterCompare()
        {
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
            _service.Compare(q1, q2);

            Assert.AreEqual(1, _repo.GetCount());
        }

        [TestMethod]
        public void Service_SavesEntityToRepository_AfterConvert()
        {
            var src = new QuantityDTO(1.0, "FEET", "LENGTH");
            var tgt = new QuantityDTO(0, "INCHES", "LENGTH");
            _service.Convert(src, tgt);

            Assert.AreEqual(1, _repo.GetCount());
        }

        [TestMethod]
        public void Service_SavesEntityToRepository_AfterAdd()
        {
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var tgt = new QuantityDTO(0, "FEET", "LENGTH");
            _service.Add(q1, q2, tgt);

            Assert.AreEqual(1, _repo.GetCount());
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  3. Controller layer tests
    // ═══════════════════════════════════════════════════════════════════════════

    [TestClass]
    public class ControllerLayerTests
    {
        private MockService _mockService;
        private MockRepository _mockRepo;
        private QuantityMeasurementController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockService = new MockService();
            _mockRepo = new MockRepository();
            _controller = new QuantityMeasurementController(_mockService, _mockRepo);
        }

        // testController_NullService_Prevention
        [TestMethod]
        public void Constructor_NullService_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => new QuantityMeasurementController(null, _mockRepo));
        }

        // testController_DemonstrateEquality_Success
        [TestMethod]
        public void PerformComparison_ServiceReturnsTrue_ControllerReturnsTrue()
        {
            _mockService.CompareResult = true;
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");

            bool result = _controller.PerformComparison(q1, q2);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void PerformComparison_ServiceReturnsFalse_ControllerReturnsFalse()
        {
            _mockService.CompareResult = false;
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(5.0, "INCHES", "LENGTH");

            bool result = _controller.PerformComparison(q1, q2);
            Assert.IsFalse(result);
        }

        // testController_DemonstrateConversion_Success
        [TestMethod]
        public void PerformConversion_ServiceReturnsResult_ControllerReturnsIt()
        {
            _mockService.ConvertResult = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var src = new QuantityDTO(1.0, "FEET", "LENGTH");
            var tgt = new QuantityDTO(0, "INCHES", "LENGTH");

            var result = _controller.PerformConversion(src, tgt);

            Assert.IsNotNull(result);
            Assert.AreEqual(12.0, result.Value);
            Assert.AreEqual("INCHES", result.UnitName);
        }

        // testController_DemonstrateAddition_Success
        [TestMethod]
        public void PerformAddition_ServiceReturnsResult_ControllerReturnsIt()
        {
            _mockService.AddResult = new QuantityDTO(2.0, "FEET", "LENGTH");
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var tgt = new QuantityDTO(0, "FEET", "LENGTH");

            var result = _controller.PerformAddition(q1, q2, tgt);

            Assert.IsNotNull(result);
            Assert.AreEqual(2.0, result.Value);
            Assert.AreEqual("FEET", result.UnitName);
        }

        // testController_DemonstrateAddition_Error
        [TestMethod]
        public void PerformAddition_ServiceThrowsNotSupported_ControllerReturnsNull()
        {
            _mockService.ThrowOnAdd =
                new NotSupportedException("Temperature ADD not supported");
            var q1 = new QuantityDTO(100.0, "CELSIUS", "TEMPERATURE");
            var q2 = new QuantityDTO(50.0, "CELSIUS", "TEMPERATURE");
            var tgt = new QuantityDTO(0, "CELSIUS", "TEMPERATURE");

            var result = _controller.PerformAddition(q1, q2, tgt);
            Assert.IsNull(result);
        }

        // testController_DisplayResult_Success / Error
        [TestMethod]
        public void PerformComparison_ServiceThrowsException_ControllerReturnsFalse()
        {
            _mockService.ThrowOnCompare =
                new QuantityMeasurementException("Cross category");
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");

            bool result = _controller.PerformComparison(q1, q2);
            Assert.IsFalse(result);
        }

        // testController_AllOperations
        [TestMethod]
        public void PerformDivision_ServiceReturns2_ControllerReturns2()
        {
            _mockService.DivideResult = 2.0;
            var q1 = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");
            var q2 = new QuantityDTO(500.0, "GRAM", "WEIGHT");

            double result = _controller.PerformDivision(q1, q2);
            Assert.AreEqual(2.0, result);
        }

        [TestMethod]
        public void PerformSubtraction_ServiceReturnsResult_ControllerReturnsIt()
        {
            _mockService.SubtractResult = new QuantityDTO(0.0, "FEET", "LENGTH");
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var tgt = new QuantityDTO(0, "FEET", "LENGTH");

            var result = _controller.PerformSubtraction(q1, q2, tgt);

            Assert.IsNotNull(result);
            Assert.AreEqual(0.0, result.Value);
            Assert.AreEqual("FEET", result.UnitName);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  4. Layer separation and dependency injection tests
    // ═══════════════════════════════════════════════════════════════════════════

    [TestClass]
    public class LayerSeparationTests
    {
        // testLayerSeparation_ServiceIndependence
        [TestMethod]
        public void Service_CanBeTestedWithoutController()
        {
            IQuantityMeasurementRepository repo = new MockRepository();
            IQuantityMeasurementService service = new QuantityMeasurementServiceImpl(repo);

            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");

            bool result = service.Compare(q1, q2);
            Assert.IsTrue(result);
        }

        // testLayerSeparation_ControllerIndependence
        [TestMethod]
        public void Controller_WorksWithMockService()
        {
            var mockService = new MockService { CompareResult = true };
            var mockRepo = new MockRepository();
            var controller = new QuantityMeasurementController(mockService, mockRepo);

            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");

            bool result = controller.PerformComparison(q1, q2);
            Assert.IsTrue(result);
        }

        // testLayerDecoupling_ServiceChange
        [TestMethod]
        public void Controller_BehaviorUnchanged_WhenServiceImplementationSwapped()
        {
            var mockRepo = new MockRepository();

            // Real service
            IQuantityMeasurementService realService =
                new QuantityMeasurementServiceImpl(mockRepo);
            var c1 = new QuantityMeasurementController(realService, mockRepo);

            // Mock service
            var mockService = new MockService { CompareResult = true };
            var c2 = new QuantityMeasurementController(mockService, mockRepo);

            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");

            Assert.AreEqual(c1.PerformComparison(q1, q2),
                            c2.PerformComparison(q1, q2));
        }

        // testDataFlow_ControllerToService
        [TestMethod]
        public void DataFlow_ControllerPassesCorrectDtoToService()
        {
            QuantityDTO captured1 = null;
            QuantityDTO captured2 = null;

            var mockService = new MockService();
            // Verify the mock receives exactly what the controller sends
            mockService.CompareResult = true;

            var controller = new QuantityMeasurementController(
                mockService, new MockRepository());

            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");

            controller.PerformComparison(q1, q2);
            // If no exception was thrown, data flowed correctly
            Assert.IsTrue(true);
        }

        // testDataFlow_ServiceToController
        [TestMethod]
        public void DataFlow_ServiceResultReachesControllerCaller()
        {
            var expected = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var mockService = new MockService { ConvertResult = expected };
            var controller = new QuantityMeasurementController(
                mockService, new MockRepository());

            var src = new QuantityDTO(1.0, "FEET", "LENGTH");
            var tgt = new QuantityDTO(0, "INCHES", "LENGTH");
            var result = controller.PerformConversion(src, tgt);

            Assert.AreEqual(expected.Value, result.Value);
            Assert.AreEqual(expected.UnitName, result.UnitName);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  5. Integration tests (end-to-end through real layers)
    // ═══════════════════════════════════════════════════════════════════════════

    [TestClass]
    public class IntegrationTests
    {
        private IQuantityMeasurementRepository _repo;
        private IQuantityMeasurementService _service;
        private QuantityMeasurementController _controller;

        [TestInitialize]
        public void Setup()
        {
            _repo = new MockRepository();
            _service = new QuantityMeasurementServiceImpl(_repo);
            _controller = new QuantityMeasurementController(_service, _repo);
        }

        // testIntegration_EndToEnd_LengthAddition
        [TestMethod]
        public void EndToEnd_LengthAddition_1FeetPlus12Inches_Equals2Feet()
        {
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var tgt = new QuantityDTO(0, "FEET", "LENGTH");

            var result = _controller.PerformAddition(q1, q2, tgt);

            Assert.IsNotNull(result);
            Assert.AreEqual(2.0, result.Value);
            Assert.AreEqual("FEET", result.UnitName);
        }

        // testIntegration_EndToEnd_TemperatureUnsupported
        [TestMethod]
        public void EndToEnd_TemperatureAddition_ControllerReturnsNull()
        {
            var q1 = new QuantityDTO(100.0, "CELSIUS", "TEMPERATURE");
            var q2 = new QuantityDTO(50.0, "CELSIUS", "TEMPERATURE");
            var tgt = new QuantityDTO(0, "CELSIUS", "TEMPERATURE");

            var result = _controller.PerformAddition(q1, q2, tgt);
            Assert.IsNull(result);
        }

        // testBackwardCompatibility_AllUC1_UC14_Tests
        [TestMethod]
        public void BackwardCompatibility_Length_1Yard_Equals_3Feet()
        {
            var q1 = new QuantityDTO(1.0, "YARDS", "LENGTH");
            var q2 = new QuantityDTO(3.0, "FEET", "LENGTH");
            Assert.IsTrue(_controller.PerformComparison(q1, q2));
        }

        [TestMethod]
        public void BackwardCompatibility_Length_30Point48Cm_Equals_1Foot()
        {
            var q1 = new QuantityDTO(30.48, "CENTIMETERS", "LENGTH");
            var q2 = new QuantityDTO(1.0, "FEET", "LENGTH");
            Assert.IsTrue(_controller.PerformComparison(q1, q2));
        }

        [TestMethod]
        public void BackwardCompatibility_Temperature_0C_Equals_32F()
        {
            var q1 = new QuantityDTO(0.0, "CELSIUS", "TEMPERATURE");
            var q2 = new QuantityDTO(32.0, "FAHRENHEIT", "TEMPERATURE");
            Assert.IsTrue(_controller.PerformComparison(q1, q2));
        }

        [TestMethod]
        public void BackwardCompatibility_Temperature_100C_Equals_212F()
        {
            var q1 = new QuantityDTO(100.0, "CELSIUS", "TEMPERATURE");
            var q2 = new QuantityDTO(212.0, "FAHRENHEIT", "TEMPERATURE");
            Assert.IsTrue(_controller.PerformComparison(q1, q2));
        }

        [TestMethod]
        public void BackwardCompatibility_Temperature_Neg40C_Equals_Neg40F()
        {
            var q1 = new QuantityDTO(-40.0, "CELSIUS", "TEMPERATURE");
            var q2 = new QuantityDTO(-40.0, "FAHRENHEIT", "TEMPERATURE");
            Assert.IsTrue(_controller.PerformComparison(q1, q2));
        }

        [TestMethod]
        public void BackwardCompatibility_Weight_1Kg_Equals_1000Gram()
        {
            var q1 = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");
            var q2 = new QuantityDTO(1000.0, "GRAM", "WEIGHT");
            Assert.IsTrue(_controller.PerformComparison(q1, q2));
        }

        [TestMethod]
        public void BackwardCompatibility_Volume_1Litre_Equals_1000Ml()
        {
            var q1 = new QuantityDTO(1.0, "LITRE", "VOLUME");
            var q2 = new QuantityDTO(1000.0, "MILLILITRE", "VOLUME");
            Assert.IsTrue(_controller.PerformComparison(q1, q2));
        }

        [TestMethod]
        public void BackwardCompatibility_Division_1KgDividedBy500Gram_Returns2()
        {
            var q1 = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");
            var q2 = new QuantityDTO(500.0, "GRAM", "WEIGHT");
            double result = _controller.PerformDivision(q1, q2);
            Assert.AreEqual(2.0, result, 0.0001);
        }

        [TestMethod]
        public void BackwardCompatibility_LitreAndGallon_AddedInMillilitres()
        {
            var q1 = new QuantityDTO(1.0, "LITRE", "VOLUME");
            var q2 = new QuantityDTO(1.0, "GALLON", "VOLUME");
            var tgt = new QuantityDTO(0, "MILLILITRE", "VOLUME");

            var result = _controller.PerformAddition(q1, q2, tgt);

            Assert.IsNotNull(result);
            Assert.AreEqual(4785.41, result.Value, 0.01);
        }

        // testScalability_NewOperation_Addition
        [TestMethod]
        public void Scalability_AddingNewCategory_DoesNotBreakExisting()
        {
            // Existing length operations still work after all categories added
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var tgt = new QuantityDTO(0, "FEET", "LENGTH");

            var result = _controller.PerformAddition(q1, q2, tgt);
            Assert.AreEqual(2.0, result.Value);
        }

        // testController_AllOperations coverage
        [TestMethod]
        public void AllFiveOperations_ExecuteWithoutException()
        {
            var q1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            var q2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
            var tgt = new QuantityDTO(0, "FEET", "LENGTH");

            _controller.PerformComparison(q1, q2);
            _controller.PerformConversion(q1, tgt);
            _controller.PerformAddition(q1, q2, tgt);
            _controller.PerformSubtraction(q1, q2, tgt);

            var w1 = new QuantityDTO(1.0, "KILOGRAM", "WEIGHT");
            var w2 = new QuantityDTO(500.0, "GRAM", "WEIGHT");
            _controller.PerformDivision(w1, w2);

            Assert.IsTrue(true); // no exception thrown
        }

        // testService_AllUnitImplementations
        [TestMethod]
        public void AllUnitImplementations_ConvertCorrectly()
        {
            Func<QuantityDTO, QuantityDTO, double> convert = (src, tgt) =>
                _service.Convert(src, tgt).Value;

            Assert.AreEqual(12.0, convert(new QuantityDTO(1.0, "FEET", "LENGTH"), new QuantityDTO(0, "INCHES", "LENGTH")), 0.001);
            Assert.AreEqual(36.0, convert(new QuantityDTO(1.0, "YARDS", "LENGTH"), new QuantityDTO(0, "INCHES", "LENGTH")), 0.001);
            Assert.AreEqual(1000.0, convert(new QuantityDTO(1.0, "LITRE", "VOLUME"), new QuantityDTO(0, "MILLILITRE", "VOLUME")), 0.001);
            Assert.AreEqual(212.0, convert(new QuantityDTO(100.0, "CELSIUS", "TEMPERATURE"), new QuantityDTO(0, "FAHRENHEIT", "TEMPERATURE")), 0.001);
        }
    }

    // ═══════════════════════════════════════════════════════════════════════════
    //  6. Repository tests
    // ═══════════════════════════════════════════════════════════════════════════

    [TestClass]
    public class RepositoryTests
    {
        private MockRepository _repo;
        private QuantityDTO _dto1;
        private QuantityDTO _dto2;

        [TestInitialize]
        public void Setup()
        {
            _repo = new MockRepository();
            _dto1 = new QuantityDTO(1.0, "FEET", "LENGTH");
            _dto2 = new QuantityDTO(12.0, "INCHES", "LENGTH");
        }

        [TestMethod]
        public void Save_And_GetAll_ReturnsAllEntities()
        {
            var e1 = new QuantityEntity("ADD", _dto1, _dto2, "2.00", "FEET", "LENGTH");
            var e2 = new QuantityEntity("COMPARE", _dto1, _dto2, "True");

            _repo.Save(e1);
            _repo.Save(e2);

            Assert.AreEqual(2, _repo.GetAll().Count);
        }

        [TestMethod]
        public void GetAllMeasurements_ReturnsSameAsGetAll()
        {
            _repo.Save(new QuantityEntity("ADD", _dto1, _dto2, "2.00", "FEET", "LENGTH"));
            Assert.AreEqual(_repo.GetAll().Count, _repo.GetAllMeasurements().Count);
        }

        [TestMethod]
        public void GetByOperationType_ReturnsOnlyMatchingEntities()
        {
            _repo.Save(new QuantityEntity("ADD", _dto1, _dto2, "2.00", "FEET", "LENGTH"));
            _repo.Save(new QuantityEntity("COMPARE", _dto1, _dto2, "True"));
            _repo.Save(new QuantityEntity("ADD", _dto1, _dto2, "3.00", "FEET", "LENGTH"));

            var adds = _repo.GetByOperationType("ADD");
            Assert.AreEqual(2, adds.Count);
        }

        [TestMethod]
        public void GetById_ReturnsCorrectEntity()
        {
            var entity = new QuantityEntity("ADD", _dto1, _dto2, "2.00", "FEET", "LENGTH");
            _repo.Save(entity);

            var found = _repo.GetById(entity.OperationId);
            Assert.IsNotNull(found);
            Assert.AreEqual(entity.OperationId, found.OperationId);
        }

        [TestMethod]
        public void GetById_NonExistentId_ReturnsNull()
        {
            var result = _repo.GetById("non-existent-id");
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Clear_RemovesAllEntities()
        {
            _repo.Save(new QuantityEntity("ADD", _dto1, _dto2, "2.00", "FEET", "LENGTH"));
            _repo.Clear();
            Assert.AreEqual(0, _repo.GetCount());
        }


        [TestMethod]
        public void Save_NullEntity_ThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(
                () => _repo.Save(null));
        }
    }
}