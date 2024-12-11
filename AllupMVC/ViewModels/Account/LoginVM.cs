using System.ComponentModel.DataAnnotations;

namespace AllupMVC.ViewModels.Account
{
    public class LoginVM
    {
        [MinLength(4)]
        [MaxLength(256)]
        public string UserNameOrEmail { get; set; }

        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool IsPersistent { get; set; }
    }
}
