using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FloraShop.Web.Areas.Admin.Models
{
    public class ChangePassModel
    {
        public int Id { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu cũ")]
        [Required(ErrorMessage = "Bạn chưa nhâp mật khẩu cũ")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu mới")]
        [Required(ErrorMessage = "Bạn chưa nhâp mật khẩu mới")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Chưa nhập xác nhận mật khẩu")]
        [Compare("NewPassword", ErrorMessage = "Mật khẩu không trùng khớp")]
        public string ConfirmPassword { get; set; }
    }
}