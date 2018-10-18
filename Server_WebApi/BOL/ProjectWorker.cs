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
        //Foreign key from 'Project' table
        [Required]
        [ForeignKey("Project")]
        public string ProjectId { get; set; }

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
