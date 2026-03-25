using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using QuantityMeasurementAppRepository.Exceptions;
using QuantityMeasurementAppEntity.Entity;
using QuantityMeasurementAppRepository.Config;
using QuantityMeasurementAppRepository.Interfaces;

namespace QuantityMeasurementAppRepository.Repository
{
    public class QuantityMeasurementDatabaseRepository
        : IQuantityMeasurementRepository
    {
        private readonly ConnectionPool _pool;

        public QuantityMeasurementDatabaseRepository()
            : this(ConnectionPool.Instance) { }

        public QuantityMeasurementDatabaseRepository(ConnectionPool pool)
        {
            _pool = pool ?? throw new ArgumentNullException(nameof(pool));
        }

        // ── Save ──────────────────────────────────────────────────────────

        public void Save(QuantityEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            SqlConnection conn = _pool.Acquire();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = @"
                    IF EXISTS (SELECT 1 FROM quantity_measurements
                               WHERE operation_id = @id)
                        UPDATE quantity_measurements SET
                            operation_type       = @opType,
                            operand1_value       = @op1Val,
                            operand1_unit        = @op1Unit,
                            operand1_measurement = @op1Meas,
                            operand2_value       = @op2Val,
                            operand2_unit        = @op2Unit,
                            operand2_measurement = @op2Meas,
                            result_value         = @resVal,
                            result_unit          = @resUnit,
                            result_measurement   = @resMeas,
                            is_error             = @isErr,
                            error_message        = @errMsg,
                            timestamp            = @ts
                        WHERE operation_id = @id
                    ELSE
                        INSERT INTO quantity_measurements (
                            operation_id,  operation_type,
                            operand1_value, operand1_unit, operand1_measurement,
                            operand2_value, operand2_unit, operand2_measurement,
                            result_value,  result_unit,   result_measurement,
                            is_error,      error_message, timestamp)
                        VALUES (
                            @id,    @opType,
                            @op1Val, @op1Unit, @op1Meas,
                            @op2Val, @op2Unit, @op2Meas,
                            @resVal, @resUnit, @resMeas,
                            @isErr,  @errMsg,  @ts)";

                cmd.Parameters.AddWithValue("@id",      entity.OperationId);
                cmd.Parameters.AddWithValue("@opType",  entity.OperationType);
                cmd.Parameters.AddWithValue("@op1Val",  entity.Operand1Value);
                cmd.Parameters.AddWithValue("@op1Unit", entity.Operand1Unit ?? "");
                cmd.Parameters.AddWithValue("@op1Meas", entity.Operand1Measurement ?? "");
                cmd.Parameters.AddWithValue("@op2Val",  (object)entity.Operand2Value       ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@op2Unit", (object)entity.Operand2Unit        ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@op2Meas", (object)entity.Operand2Measurement ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@resVal",  (object)entity.ResultValue         ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@resUnit", (object)entity.ResultUnit          ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@resMeas", (object)entity.ResultMeasurement   ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@isErr",   entity.IsError);
                cmd.Parameters.AddWithValue("@errMsg",  (object)entity.ErrorMessage        ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ts",      entity.Timestamp);

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) when (ex is not ArgumentNullException)
            {
                throw new DatabaseException("Save failed: " + ex.Message, ex);
            }
            finally { _pool.Release(conn); }
        }

        // ── Retrieve ──────────────────────────────────────────────────────

        public List<QuantityEntity> GetAll()
            => Query("SELECT * FROM quantity_measurements ORDER BY timestamp");

        public List<QuantityEntity> GetAllMeasurements() => GetAll();

        public QuantityEntity GetById(string operationId)
        {
            SqlConnection conn = _pool.Acquire();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "SELECT * FROM quantity_measurements " +
                    "WHERE operation_id = @id";
                cmd.Parameters.AddWithValue("@id", operationId);
                using var r = cmd.ExecuteReader();
                var list = ReadEntities(r);
                return list.Count > 0 ? list[0] : null;
            }
            catch (Exception ex)
            {
                throw new DatabaseException("GetById failed: " + ex.Message, ex);
            }
            finally { _pool.Release(conn); }
        }

        public List<QuantityEntity> GetByOperationType(string operationType)
        {
            SqlConnection conn = _pool.Acquire();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "SELECT * FROM quantity_measurements " +
                    "WHERE operation_type = @t ORDER BY timestamp";
                cmd.Parameters.AddWithValue("@t", operationType);
                using var r = cmd.ExecuteReader();
                return ReadEntities(r);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                    "GetByOperationType failed: " + ex.Message, ex);
            }
            finally { _pool.Release(conn); }
        }

        public List<QuantityEntity> GetByMeasurementType(string measurementType)
        {
            SqlConnection conn = _pool.Acquire();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "SELECT * FROM quantity_measurements " +
                    "WHERE operand1_measurement = @m ORDER BY timestamp";
                cmd.Parameters.AddWithValue("@m", measurementType);
                using var r = cmd.ExecuteReader();
                return ReadEntities(r);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                    "GetByMeasurementType failed: " + ex.Message, ex);
            }
            finally { _pool.Release(conn); }
        }

        public int GetCount()
        {
            SqlConnection conn = _pool.Acquire();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "SELECT COUNT(*) FROM quantity_measurements";
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                    "GetCount failed: " + ex.Message, ex);
            }
            finally { _pool.Release(conn); }
        }

        public void Clear()
        {
            SqlConnection conn = _pool.Acquire();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "DELETE FROM quantity_measurements";
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new DatabaseException(
                    "Clear failed: " + ex.Message, ex);
            }
            finally { _pool.Release(conn); }
        }

        public string GetPoolStatistics() => _pool.GetStatistics();
        public void   ReleaseResources()   => _pool.Dispose();

        // ── Helpers ───────────────────────────────────────────────────────

        private List<QuantityEntity> Query(string sql)
        {
            SqlConnection conn = _pool.Acquire();
            try
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                using var r = cmd.ExecuteReader();
                return ReadEntities(r);
            }
            catch (Exception ex)
            {
                throw new DatabaseException("Query failed: " + ex.Message, ex);
            }
            finally { _pool.Release(conn); }
        }

        private static List<QuantityEntity> ReadEntities(SqlDataReader r)
        {
            var list = new List<QuantityEntity>();
            while (r.Read())
            {
                list.Add(new QuantityEntity
                {
                    OperationId         = r["operation_id"].ToString(),
                    OperationType       = r["operation_type"].ToString(),
                    Operand1Value       = Convert.ToDouble(r["operand1_value"]),
                    Operand1Unit        = r["operand1_unit"].ToString(),
                    Operand1Measurement = r["operand1_measurement"].ToString(),
                    Operand2Value       = r["operand2_value"] == DBNull.Value
                                          ? (double?)null
                                          : Convert.ToDouble(r["operand2_value"]),
                    Operand2Unit        = r["operand2_unit"] == DBNull.Value
                                          ? null : r["operand2_unit"].ToString(),
                    Operand2Measurement = r["operand2_measurement"] == DBNull.Value
                                          ? null : r["operand2_measurement"].ToString(),
                    ResultValue         = r["result_value"] == DBNull.Value
                                          ? null : r["result_value"].ToString(),
                    ResultUnit          = r["result_unit"] == DBNull.Value
                                          ? null : r["result_unit"].ToString(),
                    ResultMeasurement   = r["result_measurement"] == DBNull.Value
                                          ? null : r["result_measurement"].ToString(),
                    IsError             = Convert.ToBoolean(r["is_error"]),
                    ErrorMessage        = r["error_message"] == DBNull.Value
                                          ? null : r["error_message"].ToString(),
                    Timestamp           = Convert.ToDateTime(r["timestamp"])
                });
            }
            return list;
        }
    }
}