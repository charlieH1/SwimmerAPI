using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SwimmingAPI.Models
{
    public class Event
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EventId { get; set; }

        [Required]
        [Display(Name="Event Code")]
        public int EventCode { get; set; }

        [Required]
        [Display(Name = "Round")]
        public string Round { get; set; }

        [Required]
        [Display(Name ="Meet ID")]     
        public int MeetId { get; set; }

        [ForeignKey("MeetId")]
        public Meet Meet { get; set; }
    }

    public class EventViewAndAdd
    {
        [Required]
        [Display(Name = "Event Code")]
        public int EventCode { get; set; }

        [Required]
        [Display(Name = "Round")]
        public string Round { get; set; }

        [Required]
        [Display(Name ="Meet Id")]
        public int MeetId { get; set; }
    }

    public class EventResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int EventResultId { get; set; }

        [Required]
        public int EventId { get; set; }

        [ForeignKey("EventId")]
        public Event Event { get; set; }

        [Required]
        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public ApplicationUser Swimmer { get; set; }

        [Required]
        public int Hours { get; set; }

        [Required]
        public int Minutes { get; set; }

        [Required]
        public int Seconds { get; set; }

    }

    public class EventResultView
    {
        [Required] public string Gender { get; set; }

        [Required] public string FamilyName { get; set; }

        [Required] public string GivenName { get; set; }

        [Required] public string Club { get; set; }

        [Required] public DateTime DateOfBirth { get; set; }

        [Required] public string Time { get; set; }

        [Required] public int EventCode { get; set; }

        [Required] public string Round { get; set; }
    }
}