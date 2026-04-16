namespace QuantityMeasurementAppEntity.DTOs
{
    public class QuantityDTO
    {
        public double Value           { get; set; }
        public string UnitName        { get; set; }  // e.g. "FEET", "CELSIUS"
        public string MeasurementType { get; set; }  // e.g. "LENGTH", "TEMPERATURE"

        public QuantityDTO() { }

        public QuantityDTO(double value, string unitName, string measurementType)
        {
            Value           = value;
            UnitName        = unitName;
            MeasurementType = measurementType;
        }

        public override string ToString() => $"{Value} {UnitName}";
    }
}