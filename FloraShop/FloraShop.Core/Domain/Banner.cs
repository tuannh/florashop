using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Domain
{
    public class Banner
    {
        public Banner()
        {
            Active = true;
            DisplayOrder = 1000;
        }

        public int Id { get; set; }

        [Required(ErrorMessage = "Tiêu đề không thể rỗng")]
        [Display(Name = "Tiêu đề")]
        public string Name { get; set; }

        [MaxLength, Column(TypeName = "ntext")]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [Display(Name = "Photo")]
        public string FileName { get; set; }

        [Display(Name = "Hiển thị")]
        public bool Active { get; set; }

        [Display(Name = "Target")]
        public string Target { get; set; }

        [Display(Name = "Url")]
        public string Url { get; set; }

        [Required(ErrorMessage = "Số thứ tự phải là một số dương")]
        [Range(1, 9999, ErrorMessage = "Số thứ tự nằm trong khoảng [1, 9999]")]
        [RegularExpression(@"^[0-9]{0,9}$", ErrorMessage = "Số thứ tự phải một số dương")]
        public int DisplayOrder { get; set; }

        [Display(Name = "Loại banner")]
        public int Category { get; set; }

    }
}
