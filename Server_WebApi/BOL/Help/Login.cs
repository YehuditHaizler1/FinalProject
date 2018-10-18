using BOL.Validations;
using System.ComponentModel.DataAnnotations;

namespace BOL.Help
{
    public class Login
    {
        //required
        //DataType is emailAddress
        //pattern of emailAddress
        //5 - 100 chars
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        [MinLength(5), MaxLength(100)]
        public string EMail { get; set; }

        //required
        //64 chars
        //valid password
        [Required]
        [Range(64, 64)]
        [ValidPassword]
        public string Password { get; set; }

    }
}
