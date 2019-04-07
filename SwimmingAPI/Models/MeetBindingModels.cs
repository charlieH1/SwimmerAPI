using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SwimmingAPI.Models
{
    public class AddMeetModel
    {
        [Required]
        [StringLength(48, ErrorMessage = "The {0} must be at least {2} characters long and at most {1} characters long", MinimumLength = 4)]
        [Display(Name = "Meet Name")]
        public string MeetName { get; set; }

        [Required]
        [StringLength(20,ErrorMessage = "The {0} must be at least {2} characters long and at most {1} characters long",MinimumLength = 4)]
        [Display(Name = "Meet Venue")]
        public string MeetVenue { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Meet Date")]
        public DateTime MeetDate { get; set; }

        [Required]
        [Display(Name = "Pool Length")]
        public int PoolLength { get; set; }

    }

    public class Meet
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int MeetId { get; set; }

        [Required]
        [StringLength(48, ErrorMessage = "The {0} must be at least {2} characters long and at most {1} characters long", MinimumLength = 4)]
        [Display(Name = "Meet Name")]
        public string MeetName { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "The {0} must be at least {2} characters long and at most {1} characters long", MinimumLength = 4)]
        [Display(Name = "Meet Venue")]
        public string MeetVenue { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Meet Date")]
        public DateTime MeetDate { get; set; }

        [Required]
        [Display(Name = "Pool Length")]
        public int PoolLength { get; set; }
    }

    
}