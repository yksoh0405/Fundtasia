using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Fundtasia.Models
{
    [MetadataType(typeof(UserMerchandiseMetadata))]
    public partial class UserMerchandise
    {
    }

    public class UserMerchandiseMetadata
    {
        public string MerchandiseId { get; set; }
        public System.DateTime PurchaseTime { get; set; }
        public string Size { get; set; }

        [Display(Name = "Full Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Full Name is required")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Invalid Full Name format.")]
        public string FullName { get; set; }

        [Display(Name = "Street Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Street Address is required")]
        public string StreetAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Invalid City format.")]
        public string City { get; set; }

        [Display(Name = "Postal Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Postal Code is required")]
        [RegularExpression(@"\d{5}", ErrorMessage = "Invalid Postal Code format.")]
        public string PostalCode { get; set; }
    }
}