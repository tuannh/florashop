using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Domain
{
    public class Order
    {
        public Order()
        {
            ProductOrders = new List<ProductOrder>();
        }

        public int Id { get; set; }

        public int? UserId { get; set; }

        [StringLength(100, ErrorMessage = "Họ tên không quá 100 kí tự")]
        [Required(ErrorMessage = "Họ tên không thể rỗng")]
        [Display(Name = "Họ tên")]
        public string CustomerName { get; set; }

        [StringLength(50, ErrorMessage = "Điện thoại không thể quá 50 kí tự")]
        [Required(ErrorMessage = "Điện thoại không thể rỗng")]
        [RegularExpression(@"[0-9 \-]+", ErrorMessage = "Số điện thoại chỉ chứa ký số")]
        [Display(Name = "Điện thoại")]
        public string Phone { get; set; }

        [StringLength(100, ErrorMessage = "Email không thể quá 100 kí tự")]
        [Required(ErrorMessage = "Email không thể rỗng")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Địa chỉ email chưa đúng")]
        public string Email { get; set; }

        [Display(Name = "Địa chỉ")]
        public string Address { get; set; }

        [Display(Name = "Quận/Huyện")]
        public int? DistrictId { get; set; }

        [StringLength(250, ErrorMessage = "Ghi chú không được quá 250 kí tự")]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        public DateTime? OrderDate { get; set; }

        [Display(Name = "Tỉnh/Thành phố")]
        public int? ProvinceId { get; set; }

         [Display(Name = "Trang thái")]
        public int Status { get; set; }

        public virtual ICollection<ProductOrder> ProductOrders { get; set; }

        public virtual User User { get; set; }

        public virtual District District { get; set; }

        public virtual Province Province { get; set; }
    }
}
