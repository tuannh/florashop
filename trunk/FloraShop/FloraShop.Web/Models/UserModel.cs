using FloraShop.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;
using FloraShop.Core.Extensions;

namespace FloraShop.Web.Models
{
    public class UserModel
    {
        public UserModel()
        {

        }

        public UserModel(User user)
        {
            Username = user.Username;
            Email = user.Email;
            FullName = user.FullName;
            Phone = user.Phone;
            Address = user.Address;

            if (user.Birthday.HasValue)
                Birthday = user.Birthday.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

            Gender = user.Gender;
            DistrictId = user.DistrictId;
            ProvinceId = user.ProvinceId;
        }

        public void UpdateDomain(ref User user)
        {
            user.Username = this.Username;
            user.Email = this.Email;
            user.FullName = this.FullName;
            user.Phone = this.Phone;
            user.Address = this.Address;
            user.Birthday = DateTime.Now.GetDate(this.Birthday, "dd/MM/yyyy");

            if (this.Gender.HasValue)
                user.Gender = this.Gender.Value;

            user.DistrictId = this.DistrictId;
            user.ProvinceId = this.ProvinceId;
        }

        [Required(ErrorMessage = "Tên đăng nhập không thể rỗng")]
        [Display(Name = "Tên đăng nhập")]
        [RegularExpression("^[a-z0-9_-]{3,50}$", ErrorMessage = "Tên đăng nhập tối đa tối thiểu 3 kí tự, tối đa 50 kí tự")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được quá 50 kí tự")]
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

        [Display(Name = "Điện thoại")]
        [RegularExpression(@"[0-9 \-]+", ErrorMessage = "Số điện thoại chỉ chứa ký số")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Ngày sinh")]
        [Required(ErrorMessage = "Bạn chưa chọn ngày sinh")]
        public string Birthday { get; set; }

        [Display(Name = "Giới tính")]
        [Required(ErrorMessage = "Chưa chọn giới tính")]
        [RegularExpression(@"\d", ErrorMessage = "Chưa chọn giới tính")]
        public int? Gender { get; set; }

        [Display(Name = "Quận/Huyện")]
        public int? DistrictId { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public int? ProvinceId { get; set; }
    }
}