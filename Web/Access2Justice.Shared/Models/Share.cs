using Access2Justice.Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Access2Justice.Shared
{
    public class ShareInput : UnShareInput
    {
        [Required]
        public Uri Url { get; set; }
    }

    public class UnShareInput
    {
        [Required]
        public Guid ResourceId { get; set; }
        [Required]
        public string UserId { get; set; }
    }
}
