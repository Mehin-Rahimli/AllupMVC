using System.ComponentModel.DataAnnotations;

namespace AllupMVC.ViewModels.Account
{
    public class RegisterVM
    {
        [DataType(DataType.EmailAddress)]
        [MinLength(4)]
        [MaxLength(256)]
        public String EmailAddress { get; set; }

        [DataType(DataType.Password)]
        [MinLength(8)]
        public String Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public String ConfirmPassword { get; set; }

        [MinLength(3)]
        [MaxLength(25)]
        public string FirstName { get; set; }

        [MinLength(3)]
        [MaxLength(25)]
        public string LastName { get; set; }

        [MinLength(4)]
        [MaxLength(256)]
        public string UserName { get; set; }
    }
}
