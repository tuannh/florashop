using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloraShop.Core.Domain
{
    public class Content
    {
        public int Id { get; set; }

        [StringLength(100, ErrorMessage = "Tên tối đa 100 kí tự")]
        [Required(ErrorMessage = "Tên không thể rỗng")]
        [Display(Name = "Tên")]
        public string Name { get; set; }

        [StringLength(100, ErrorMessage = "Alias tối đa 100 kí tự")]
        [Required(ErrorMessage = "Alias không thể rỗng")]
        [Display(Name = "Alias")]
        public string Alias { get; set; }

        [Display(Name = "Nội dung")]
        [MaxLength, Column(TypeName = "ntext")]
        public string Value { get; set; }

        [Display(Name = "Trang chứa nội dung")]
        [StringLength(100, ErrorMessage = "Page alias tối đa 100 kí tự")]
        public string PageAlias { get; set; }
    }
}
