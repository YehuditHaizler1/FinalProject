using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BOL.Validations;

namespace BOL
{
    public class Worker
    {
        //primary key
        //[Key]
        public int Id { get; set; }

        //required
        //2 - 10 chars
        //[Required]
        //[MinLength(2), MaxLength(10)]
        public string UserName { get; set; }

        //required
        //DataType is password
        //5 - 8 chars
        //[Required]
        //[DataType(DataType.Password)]
        //[MinLength(5), MaxLength(8)]
        public string Password { get; set; }

        //2 - 10 chars
        //[MinLength(2), MaxLength(10)]
        public string FirstName { get; set; }

        //required
        //2 - 10 chars
        //[Required]
        //[MinLength(2), MaxLength(10)]
        public string LastName { get; set; }

        //required
        //DataType is emailAddress
        //pattern of emailAddress
        //5 - 100 chars
        //unique
        //[Required]
        //[DataType(DataType.EmailAddress)]
        //[EmailAddress]
        //[MinLength(5), MaxLength(100)]
        //[UniqueEmail]
        public string EMail { get; set; }

        //DataType is phoneNumber
        //pattern of phoneNumber
        //9 - 10 chars
        //[DataType(DataType.PhoneNumber)]
        //[Phone]
        //[MinLength(9), MaxLength(10)]
        public string Phone { get; set; }

        //required
        //Foreign key from 'Status' table
        //[Required]
        //[ForeignKey("Status")]
        public int StatusId { get; set; }

        //Foreign key from 'Worker' table
        //[ForeignKey("Worker")]
        public int? ManagerId { get; set; }

        //positive numbers
        //[Range(0, int.MaxValue)]
        public int? TotalHours { get; set; }
    }
}
