namespace SafeRoute.Revit.NetFramework.Utils
{
    public static class UnitConverter
    {
        private const double FeetToMeterFactor = 0.3048;

        public static double FeetToMeters(double feet)
        {
            return feet * FeetToMeterFactor;
        }
    }
}
