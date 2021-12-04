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
        [Required(AllowEmptyStrings = false, ErrorMessage = "Merchandise Name is required")]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Merchandise Image is required")]
        public string Image { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Merchandise Price is required")]
        public decimal Price { get; set; }
    }

    public class MerchandiseInsertVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Merchandise Name is required")]
        [StringLength(150)]
        public string Name { get; set; }

        [Required]
        public HttpPostedFileBase Image { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Merchandise Price is required")]
        public decimal Price { get; set; }
    }

    public class MerchandiseEditVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Merchandise Name is required")]
        public string Name { get; set; }

        [Required]
        public HttpPostedFileBase Image { get; set; }

        public string ImageURL { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Merchandise Price is required")]
        public decimal Price { get; set; }
    }
}