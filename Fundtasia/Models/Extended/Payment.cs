using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Fundtasia.Models
{
    [MetadataType(typeof(PaymentMetadata))]
    public partial class Payment
    {

    }

    public class PaymentMetadata
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

    }
}