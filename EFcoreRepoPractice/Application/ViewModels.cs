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

    public class LoginViewModel
    {

        [Display(Name = "電子郵件")]
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }


        [Display(Name = "密碼")]
        [Required] 
        public string Password { get; set; }


    }

    public class ForgetPasswordViewModel
    {

        [Display(Name = "電子郵件")]
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; } 


    }


    public class UpdatePasswordViewModel
    {

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

        [Required(ErrorMessage ="請至登入頁點選忘記密碼")]
        public string Token { get; set; }


    }


}
