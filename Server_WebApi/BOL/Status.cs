using System.ComponentModel.DataAnnotations;

namespace BOL
{
    public class Status
    {
        //primary key
        [Key]
        public int Id { get; set; }

        //required
        //3 - 10 chars
        [Required]
        [MinLength(3),MaxLength(20)]
        public string StatusName { get; set; }
    }
}
