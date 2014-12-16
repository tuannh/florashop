using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FloraShop.Web.Models
{
    public class RegisterModel
    {
        public int Id { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Bạn chưa nhập tên đăng nhập")]
        [RegularExpression("^[a-z0-9_-]{5,50}$", ErrorMessage = "Tên đăng nhập tối thiểu 5 kí tự, chỉ chứa các kí tự [a-z0-9_-]")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Chưa nhập xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không trùng khớp")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email không thể rỗng")]
        [StringLength(150, ErrorMessage = "Email không được quá 150 kí tự")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Họ tên không thể rỗng")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Chưa chọn giới tính")]
        public int Gender { get; set; }
    }
}