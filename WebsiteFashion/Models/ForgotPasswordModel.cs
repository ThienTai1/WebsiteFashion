﻿using System.ComponentModel.DataAnnotations;

namespace WebsiteFashion.Models
{
    public class ForgotPasswordModel
    {
        [Required, EmailAddress, Display(Name="Registered email address")]
        public string Email { get; set; }
        public bool EmailSent {  get; set; }
    }
}
