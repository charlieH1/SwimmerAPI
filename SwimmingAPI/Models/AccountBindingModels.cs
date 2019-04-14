using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SwimmingAPI.Models
{
    // Models used as parameters to AccountController actions.

    /// <summary>
    /// The binding model for adding a external login
    /// </summary>
    public class AddExternalLoginBindingModel
    {
        /// <summary>
        /// The external access token
        /// </summary>
        [Required]
        [Display(Name = "External access token")]
        public string ExternalAccessToken { get; set; }
    }

    /// <summary>
    /// The model for changing the password
    /// </summary>
    public class ChangePasswordBindingModel
    {
        /// <summary>
        /// The current password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        /// <summary>
        /// The new password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Confirm the new password
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// The model for registering
    /// </summary>
    public class RegisterBindingModel
    {
        /// <summary>
        /// The Email address
        /// </summary>
        [Required] [Display(Name = "Email")] public string Email { get; set; }

        /// <summary>
        /// The password to be used
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        /// <summary>
        /// Confirm the password to be used
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// The phone number
        /// </summary>
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The family name
        /// </summary>
        [Required]
        [Display(Name ="Family Name")]
        public string FamilyName { get; set; }

        /// <summary>
        /// The given name
        /// </summary>
        [Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }

        /// <summary>
        /// The swimming club
        /// </summary>
        [Required]
        [Display(Name ="Club")]
        public string Club { get; set; }
       
        /// <summary>
        /// The gender must be either M or F
        /// </summary>
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// The date of birth
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// The postal address
        /// </summary>
        [Required]
        [Display(Name = "Postal Address")]
        public string Address { get; set; }

        /// <summary>
        /// The account type must be either Official or Swimmer
        /// </summary>
        [Required]
        [Display (Name="Type Of User")]
        public string AccountType { get; set; }

    }

    /// <summary>
    /// The model for updating a swimmer
    /// </summary>
    public class UpdateSwimmerInfoModel
    {
        /// <summary>
        /// The user id of the swimmer
        /// </summary>
        [Required]
        [Display(Name = "UserId")]
        public string userId { get; set; }

        /// <summary>
        /// The email of the swimmer
        /// </summary>
        [Required] [Display(Name = "Email")] public string Email { get; set; }


        /// <summary>
        /// The Phone number
        /// </summary>
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// The family name
        /// </summary>
        [Required]
        [Display(Name = "Family Name")]
        public string FamilyName { get; set; }

        /// <summary>
        /// The given name
        /// </summary>
        [Required]
        [Display(Name = "Given Name")]
        public string GivenName { get; set; }

        /// <summary>
        /// The club they are at
        /// </summary>
        [Required]
        [Display(Name = "Club")]
        public string Club { get; set; }

        /// <summary>
        /// Their gender must be either M or F
        /// </summary>
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        /// <summary>
        /// The date of birth
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Date Of Birth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// The address
        /// </summary>
        [Required]
        [Display(Name = "Postal Address")]
        public string Address { get; set; }

        
    }



    /// <summary>
    /// The biding for registering externally
    /// </summary>
    public class RegisterExternalBindingModel
    {
        /// <summary>
        /// The email address
        /// </summary>
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    /// <summary>
    /// The remove login binding model
    /// </summary>
    public class RemoveLoginBindingModel
    {
        /// <summary>
        /// The login provider
        /// </summary>
        [Required]
        [Display(Name = "Login provider")]
        public string LoginProvider { get; set; }

        /// <summary>
        /// The provider key
        /// </summary>
        [Required]
        [Display(Name = "Provider key")]
        public string ProviderKey { get; set; }
    }

    /// <summary>
    /// Model for setting passwords
    /// </summary>
    public class SetPasswordBindingModel
    {
        /// <summary>
        /// The new password
        /// </summary>
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        /// <summary>
        /// Confirm the new password
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}

