using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Fundtasia.Models
{
    [MetadataType(typeof(MerchandisePaymentMetadata))]
    public partial class MerchandisePayment
    {

    }

    public class MerchandisePaymentMetadata
    {
        [Display(Name = "Name on card")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name on card is required")]
        public string Name { get; set; }

        [Display(Name = "Card Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Card Number is required")]
        public string CardNumber { get; set; }

        [Display(Name = "Expiry Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Expiry Date is required")]
        public DateTime ExpiryDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "CVV is required")]
        [RegularExpression(@"\d{3}", ErrorMessage = "Invalid CVV format.")]
        public string CVV { get; set; }

        [Display(Name = "Full Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Full Name is required")]
        public string FullName { get; set; }

        [Display(Name = "Street Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Street Address is required")]
        public string StreetAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required")]
        [RegularExpression(@"[a-z][A-Z]", ErrorMessage = "Invalid {0} format.")]
        public string City { get; set; }

        [Display(Name = "Postal Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Postal Code is required")]
        [RegularExpression(@"\d{5}", ErrorMessage = "Invalid Postal Code format.")]
        public string PostalCode { get; set; }

    }
}