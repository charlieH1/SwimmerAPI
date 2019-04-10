using System;
using System.Collections.Generic;

namespace SwimmingAPI.Models
{
    // Models returned by AccountController actions.

    public class ExternalLoginViewModel
    {
        public string Name { get; set; }

        public string Url { get; set; }

        public string State { get; set; }
    }

    public class ManageInfoViewModel
    {
        public string LocalLoginProvider { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }
        public string Role { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Club { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
    
        public IEnumerable<UserLoginInfoViewModel> Logins { get; set; }

        public IEnumerable<ExternalLoginViewModel> ExternalLoginProviders { get; set; }
    }

    public class UserInfoViewModel
    {
        public string Email { get; set; }
        public string Address { get; set; }
        public string Role { get; set; }
        public string FamilyName { get; set; }
        public string GivenName { get; set; }
        public string Club { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }

    }

    public class UserLoginInfoViewModel
    {
        public string LoginProvider { get; set; }

        public string ProviderKey { get; set; }
    }
}
