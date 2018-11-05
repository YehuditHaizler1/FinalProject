using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    public class ProjectWorker
    {
        //primary key
        [Key]
        public int Id { get; set; }

        //required
        //Foreign key from 'Projects' table
        [Required]
        [ForeignKey("Projects")]
        public int ProjectId { get; set; }

        //required
        //Foreign key from 'workers' table
        [Required]
        [ForeignKey("Workers")]
        public int WorkerId { get; set; }

        //required
        //positive numbers
        [Required]
        [Range(0, int.MaxValue)]
        public int TotalHours { get; set; }

        //required
        //positive numbers
        [Required]
        [Range(0, int.MaxValue)]
        public int SumHours { get; set; }
    }
}
