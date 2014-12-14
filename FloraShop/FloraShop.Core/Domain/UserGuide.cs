using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Domain
{
    public class UserGuide
    {
        public UserGuide()
        {
            Active = true;
            DisplayOrder = 1000;
        }

        public int Id { get; set; }

        [Display(Name = "Alias")]
        [Required(ErrorMessage = "Alias không thể rỗng")]
        [StringLength(100, ErrorMessage = "Alias không được quá 100 kí tự")]
        public string Alias { get; set; }

        [StringLength(1000, ErrorMessage = "Tên userguide không được quá 1000 kí tự")]
        [Required(ErrorMessage = "Tên userguide không thể rỗng")]
        [Display(Name = "Tên userguide")]
        public string Name { get; set; }

        [MaxLength, Column(TypeName = "ntext")]
        [Display(Name = "Tóm tắt")]
        public string Summary { get; set; }

        [Display(Name = "Nội dung")]
        [MaxLength, Column(TypeName = "ntext")]
        public string Content { get; set; }

        [Display(Name = "Hiển thị")]
        public bool Active { get; set; }

        [Range(0, 9999, ErrorMessage = "Thứ tự hiển thị là 1 số trong khoảng [0, 9999]")]
        [RegularExpression(@"^[0-9]{0,4}$", ErrorMessage = "Thứ tự hiển thị là 1 số trong khoảng [0, 9999]")]
        [Display(Name = "Thứ tự hiển thị")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Photo")]
        public string Photo { get; set; }
    }
}
