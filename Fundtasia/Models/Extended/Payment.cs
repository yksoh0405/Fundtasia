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

    public class DonationPaymentVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Donation Amount is required")]
        public decimal Amount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Event Selection is required")]
        public string EventId { get; set; }
    }

    public class MerchandisePaymentVM
    {
        public System.Guid UserId { get; set; }

        public string MerchandiseId { get; set; }

        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Size is required")]
        public string Size { get; set; }

        [Display(Name = "Full Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Full name is required")]
        public string FullName { get; set; }

        [Display(Name = "Street Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Street Address is required")]
        public string StreetAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "State is required")]
        public string State { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "City is required")]
        public string City { get; set; }

        [Display(Name = "Postal Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Postal Code is required")]
        [RegularExpression(@"\d{5}", ErrorMessage = "Invalid Postal Code format.")]
        public string PostalCode { get; set; }
    }
}