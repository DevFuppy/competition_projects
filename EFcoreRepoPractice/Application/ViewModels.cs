using System.ComponentModel.DataAnnotations;

namespace EFcoreRepoPractice.Application
{

    public class RegisterViewModel
    {
        //[Required]
        //[MaxLength(100)]
        //public string Account { get; set; }

        [Display(Name ="電子郵件")]
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }


        [Display(Name = "密碼")]
        [Required]
        //[DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        [Display(Name = "確認密碼")]
        [Required]
        [Compare("Password")]
        //[DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }


    }


}
