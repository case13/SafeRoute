using System.Collections.Generic;

namespace SafeRoute.Shared.Dtos.Common
{
    public class ReplaceResultDto
    {
        public int ProjectId { get; set; }
        public int Deleted { get; set; }
        public int Inserted { get; set; }

        public Dictionary<int, int> InsertedBySeverity { get; set; }
            = new Dictionary<int, int>();
    }
}
