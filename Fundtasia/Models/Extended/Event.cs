using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Fundtasia.Models
{
    [MetadataType(typeof(EventMetadata))]
    public partial class Event
    {

    }

    public class EventMetadata
    {
        public string Id { get; set; }
        public int View { get; set; }
        public int Likes { get; set; }
        public System.DateTime CreatedDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Title required <span class='text-danger'>*</span>")]
        public string Title { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Cover Image required")]
        public string CoverImage { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Target required")]
        public decimal Target { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "YouTube link required")]
        public string YouTubeLink { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Image required")]
        public string ImageArray { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Summary required")]
        public string Summary { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Challenge required")]
        public string Challenge { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Solution required")]
        public string Solution { get; set; }
    }
}