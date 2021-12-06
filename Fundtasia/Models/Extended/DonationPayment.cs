using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fundtasia.Models.Extended
{
    [MetadataType(typeof(DonationPaymentMetadata))]
    public partial class DonationPayment
    {
    }

    public class DonationPaymentMetadata
    {

        public System.Guid Id { get; set; }
        public System.Guid UserId { get; set; }
        public System.DateTime TimeDonated { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Donation amount is required")]
        public decimal Amount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Event is required")]
        public string EventId { get; set; }

        [Display(Name = "Name on card")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name on card is required")]
        public string Name { get; set; }

        [Display(Name = "Card Number")]
        [DisplayFormat(DataFormatString = "{0:MM/yyyy}")]
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