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
    }

    public class DonationPaymentVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Name is required")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Invalid Name format.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Card Number is required")]
        [RegularExpression(@"^[\d]{16,16}$", ErrorMessage = "Invalid Card Number format.")]
        public string CardNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Expiry Date is required")]
        [RegularExpression(@"^((0[1-9])|(1[0-2]))\/((21)|([2][1-9]))$", ErrorMessage = "*Invalid Expiry Date format.")]
        public string ExpiryDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*CVV is required")]
        [RegularExpression(@"^[\d]{3,3}$", ErrorMessage = "Invalid CVV format.")]
        public string CVV { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Donation Amount is required")]
        public decimal Amount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Event Selection is required")]
        public string EventId { get; set; }
    }

    public class MerchPaymentVM
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Name is required")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Invalid Name format.")]
        [StringLength(100)]
        public string Name { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Card Number is required")]
        [RegularExpression(@"^[\d]{16,16}$", ErrorMessage = "Invalid Card Number format.")]
        public string CardNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Expiry Date is required")]
        [RegularExpression(@"^((0[1-9])|(1[0-2]))\/((2021)|(20[1-2][0-9]))$", ErrorMessage = "Invalid Expiry Date format.")]
        public string ExpiryDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*CVV is required")]
        [RegularExpression(@"^[\d]{3,3}$", ErrorMessage = "Invalid CVV format.")]
        public string CVV { get; set; }

        public System.Guid UserId { get; set; }

        public string MerchandiseId { get; set; }

        public decimal Price { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*Size is required")]
        public string Size { get; set; }

        [Display(Name = "Full Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Full Name is required")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "*Invalid Full Name format.")]
        public string FullName { get; set; }

        [Display(Name = "Street Address")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Street Address is required")]
        public string StreetAddress { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*State is required")]
        public string State { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "*City is required")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "*Invalid City format.")]
        public string City { get; set; }

        [Display(Name = "Postal Code")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "*Postal Code is required")]
        [RegularExpression(@"\d{5}", ErrorMessage = "*Invalid Postal Code format.")]
        public string PostalCode { get; set; }
    }
}