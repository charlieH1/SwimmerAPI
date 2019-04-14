using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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


        /// <summary>
        /// A two character code derived from:
        ///     a.Number of turns per 100 (metres or yards)
        ///     b.Metric or imperial pool code where
        ///         1 -Metres pool
        ///         2 -Yards metric equivalent e.g. 36 2/3 yards
        ///         3 -Yards pool  e.g. 33 1/3 yards
        /// </summary>
        [Required]
        [Display(Name = "Pool Length")]
        [StringLength(2,ErrorMessage = "Pool length must match neutral file format ",MinimumLength = 2)]
        public string PoolLength { get; set; }

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

        /// <summary>
        /// A two character code derived from:
        ///     a.Number of turns per 100 (metres or yards)
        ///     b.Metric or imperial pool code where
        ///         1 -Metres pool
        ///         2 -Yards metric equivalent e.g. 36 2/3 yards
        ///         3 -Yards pool  e.g. 33 1/3 yards
        /// </summary>
        [Required]
        [Display(Name = "Pool Length")]
        [StringLength(2, ErrorMessage = "Pool length must match neutral file format ", MinimumLength = 2)]
        public string PoolLength { get; set; }
    }

    
}