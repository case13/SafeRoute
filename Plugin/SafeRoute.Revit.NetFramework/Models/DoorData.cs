namespace SafeRoute.Revit.NetFramework.Models
{
    public class DoorData
    {
        /// <summary>
        /// Revit ElementId (string para desacoplar)
        /// </summary>
        public string ElementId { get; set; } = default!;

        /// <summary>
        /// Door width in meters
        /// </summary>
        public double Width { get; set; }

        /// <summary>
        /// Door height in meters
        /// </summary>
        public double Height { get; set; }

        /// <summary>
        /// Indicates whether the door is marked as accessible
        /// </summary>
        public bool IsAccessible { get; set; }
    }
}
