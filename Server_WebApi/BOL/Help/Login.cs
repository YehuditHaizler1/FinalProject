using System.ComponentModel.DataAnnotations;

namespace BOL.Help
{
    public class Login
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [MinLength(5), MaxLength(100)]
        public string EMail { get; set; }



    }
}
