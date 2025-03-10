﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using TalentTrail.Enum;

namespace TalentTrail.Models
{
    public class JobApplication
    {
        [Key]
        public int ApplicationId { get; set; }

        [Required]
        public int SeekerId { get; set; }

        [Required]
        public int JobId { get; set; }

        [StringLength(1000)]
        public string? CoverLetter { get; set; }

        [Required]
        [StringLength(100)]
        public string ResumePath { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ApplicationDate { get; set; }

        [Required]
        public ApplicationStatus ApplicationStatus { get; set; }

        //Relationships
        public JobSeeker? jobSeeker { get; set; }
        public JobPost? jobPost { get; set; }

    }
}
