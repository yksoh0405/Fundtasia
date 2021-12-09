using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Fundtasia.Models
{
    public partial class Report
    {

    }

    public class ReportInsertVM
    {
        public string Title { get; set; }

        [Display(Name = "Created Date")]
        public System.DateTime CreatedDate { get; set; }
        
        [Display(Name = "Created By")]
        public System.Guid CreatedBy { get; set; }
    }
}