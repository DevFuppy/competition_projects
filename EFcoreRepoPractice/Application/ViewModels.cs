using System.ComponentModel.DataAnnotations;

namespace EFcoreRepoPractice.Application
{

    public class RegisterViewModel
    {
        //[Required]
        //[MaxLength(100)]
        //public string Account { get; set; }

        [Display(Name ="電子郵件")]
        [Required(ErrorMessage = "Email為必填欄位")]
        [EmailAddress(ErrorMessage = "必須為電子郵件格式")]
        [MaxLength(100)]
        public string Email { get; set; }


        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼為必填欄位")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密碼長度須為8~20字")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]+$",
        ErrorMessage = "密碼需包含大小寫英文字母與數字")]
        public string Password { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "確認密碼為必填欄位")]
        [Compare("Password",ErrorMessage = "密碼與確認密碼不一致")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

    }

    public class LoginViewModel
    {

        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "Email為必填欄位")]
        [EmailAddress(ErrorMessage = "必須為電子郵件格式")]
        [MaxLength(100)]
        public string Email { get; set; }


        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼為必填欄位")]
        [DataType(DataType.Password)]
        //[StringLength(20, MinimumLength = 8)]
        public string Password { get; set; }


    }

    public class ForgetPasswordViewModel
    {

        [Display(Name = "電子郵件")]
        [Required(ErrorMessage = "必填")]
        [EmailAddress(ErrorMessage = "必須為電子郵件格式")]
        [MaxLength(100, ErrorMessage = "長度過長")]
        public string Email { get; set; } 


    }


    public class UpdatePasswordViewModel
    {

        [Display(Name = "密碼")]
        [Required(ErrorMessage = "密碼為必填欄位")]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "密碼長度須為8~20字")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[A-Za-z\d]+$",
        ErrorMessage = "密碼需包含大小寫英文字母與數字")]
        public string Password { get; set; }

        [Display(Name = "確認密碼")]
        [Required(ErrorMessage = "確認密碼為必填欄位")]
        [Compare("Password", ErrorMessage = "密碼與確認密碼不一致")]
        [DataType(DataType.Password)]
        //[DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage ="請至登入頁點選忘記密碼")]
        public string Token { get; set; }

    }


}
