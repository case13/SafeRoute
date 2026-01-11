namespace SafeRoute.Revit.NetFramework.Models
{
    public class RampData
    {
        /// <summary>
        /// Revit ElementId (string para desacoplar)
        /// </summary>
        public string ElementId { get; set; } = default!;

        /// <summary>
        /// Ramp width in meters
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Ramp length in meters
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// Ramp slope as decimal (e.g. 0.0833 = 8.33%)
        /// </summary>
        public double Slope { get; set; }
    }
}
