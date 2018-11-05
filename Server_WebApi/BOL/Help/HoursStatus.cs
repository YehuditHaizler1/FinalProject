using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BOL.Help
{
    public class HoursStatus
    {
        [Required]
        public int WorkerId { get; set; }

        [Required]
        public int ProjectId { get; set; }

        [Required]
        public long? RequiredHours { get; set; }

        [DefaultValue(0)]
        [Required]
        public long? ActualHours { get; set; }

    }
}
