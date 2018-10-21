using BOL.Validations;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    public class Project
    {

        //primary key
        [Key]
        public int Id { get; set; }

        //required
        //3 - 20 chars
        [Required]
        [MinLength(3), MaxLength(20)]
        public string Name { get; set; }

        //required
        //3 - 20 chars
        [Required]
        [MinLength(3), MaxLength(20)]
        public string CustomerName { get; set; }

        //required
        //Foreign key from 'User' table
        [Required]
        [ForeignKey("User")]
        public string ProjectManagerId { get; set; }

        //positive numbers
        [Range(0, int.MaxValue)]
        public string ProgramHours { get; set; }

        //positive numbers
        [Range(0, int.MaxValue)]
        public string QAHours { get; set; }

        //positive numbers
        [Range(0, int.MaxValue)]
        public string UIUXHours { get; set; }

        //required
        //DataType is date 
        [Required]
        [DataType(DataType.Date)]
        //[DefaultValue(DateTime,DateTime.Now.Date.ToString())]
        public DateTime StartDate { get; set; } = DateTime.Now.Date;

        //required
        //DataType is date
        //end date must be after start date
        [Required]
        [DataType(DataType.Date)]
        [MinEndDate]
        public DateTime EndDate { get; set; }

    }
}
