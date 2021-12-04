using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Fundtasia.Models
{
    public class MerchandiseCreate
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Title required")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public HttpPostedFileBase ImagePic { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Price required")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$", ErrorMessage = "Invalid Price format")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Status required")]
        public string Status { get; set; }
    }
}