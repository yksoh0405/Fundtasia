﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

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

        [Required(AllowEmptyStrings = false, ErrorMessage = "Title required")]
        [StringLength(150)]
        public string Title { get; set; }

        [Display(Name = "Cover Image")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Cover Image required")]
        public string CoverImage { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Target required")]
        public decimal Target { get; set; }

        [Display(Name = "YouTube Link")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "YouTube link required")]
        public string YouTubeLink { get; set; }

        [Display(Name = "Display images")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Image required")]
        public string ImageArray { get; set; }

        [AllowHtml]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Article required")]
        public string Article { get; set; }
    }
}