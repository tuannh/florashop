
using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace FloraShop.Core.Domain
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập không thể rỗng")]
        [Display(Name = "Tên đăng nhập")]
        [RegularExpression("^[a-z0-9_-]{3,50}$", ErrorMessage = "Tên đăng nhập tối đa tối thiểu 3 kí tự, tối đa 50 kí tự")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được quá 50 kí tự")]
        public string Username { get; set; }

        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        public string PasswordSalt { get; set; }

        [Required(ErrorMessage = "Email không thể rỗng")]
        [StringLength(150, ErrorMessage = "Email không được quá 150 kí tự")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Họ tên không thể rỗng")]
        [Display(Name = "Họ tên")]
        public string FullName { get; set; }

        [Display(Name = "Trạng thái")]
        public bool Active { get; set; }

        public bool IsAdmin { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public DateTime? LastLogin { get; set; }

        public Guid? ResetCode { get; set; }

        public DateTime? ResetExpiredCode { get; set; }

        [Display(Name = "Điện thoại")]
        [RegularExpression(@"[0-9 \-]+", ErrorMessage = "Số điện thoại chỉ chứa ký số")]
        public string Phone { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Điểm thưởng")]
        public int TotalPoints { get; set; }

        [Display(Name = "Ngày sinh")]
        public DateTime? Birthday { get; set; }

        [Display(Name = "Giới tính")]
        public int Gender { get; set; }

        [Display(Name = "Quận/Huyện")]
        public int? DistrictId { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public int? ProvinceId { get; set; }

        public virtual District District { get; set; }

        public virtual Province Province { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public virtual ICollection<UserPoint> UserPoints { get; set; }
    }
}
