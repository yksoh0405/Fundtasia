using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace Fundtasia.Models
{
    [MetadataType(typeof(MerchandiseMetadata))]
    public partial class Merchandise
    {
    }

    public class MerchandiseMetadata
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Merchandise Name is required")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Merchandise Price is required")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Merchandise Status is required")]
        public string Status { get; set; }
    }

    public class MerchandiseInsertVM
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Merchandise Name is required")]
        [StringLength(150)]
        public string Name { get; set; }

        [Required(ErrorMessage = "*Merchandise Image is required")]
        public HttpPostedFileBase ImageURL { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Merchandise Price is required")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Invalid Price format.")]
        [Range(1, 1000, ErrorMessage = "Price must be between 1 to 1,000")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Merchandise Status is required")]
        public string Status { get; set; }
    }

    public class MerchandiseEditVM
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Merchandise Name is required")]
        public string Name { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public string ImageURL { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Merchandise Price is required")]
        [RegularExpression(@"^\d+.\d{0,2}$", ErrorMessage = "Invalid Price format.")]
        [Range(1, 1000, ErrorMessage = "Price must be between 1 to 1,000")]
        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Merchandise Status is required")]
        public string Status { get; set; }
    }
}