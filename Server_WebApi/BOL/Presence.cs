using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BOL
{
    public class Presence
    {
        
        //primary key
        [Key]
        public int Id { get; set; }

        //required
        //Foreign key from 'Workers' table
        [Required]
        [ForeignKey("Workers")]
        public int WorkerId { get; set; }

        //required
        //Foreign key from 'Projects' table
        [Required]
        [ForeignKey("Projects")]
        public int ProjectId { get; set; }

        //required
        //DataType is dateTime 
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BeginningTime { get; set; }

        //required
        //DataType is dateTime 
        [Required]
        [DataType(DataType.DateTime)]
        public DateTime EndTime { get; set; }

    }
}
