namespace ImReady4.Model
{
    public class Reading(DateTime _dateOfReading, decimal _restingHR, decimal _rmssd, decimal _sdnn)
    {
        public DateTime DateOfReading { get; set; } = _dateOfReading;
        public decimal RestingHR { get; set; } = _restingHR;
        public decimal RMSSD { get; set; } = _rmssd;
        public decimal SDNN { get; set; } = _sdnn;
    }
}