using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using SwimmingAPI.Models;
using SwimmingAPI.Providers;
using SwimmingAPI.Repo.Interfaces;
using SwimmingAPI.Results;

namespace SwimmingAPI.Controllers
{
    /// <summary>
    /// The controller for accounts
    /// </summary>
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private ApplicationUserManager _userManager;
        private readonly IUserRepo _userRepo;

        /// <inheritdoc />
        public AccountController()
        {
            
        }

        /// <inheritdoc />
        public AccountController(ApplicationUserManager userManager,
            ISecureDataFormat<AuthenticationTicket> accessTokenFormat, IUserRepo userRepo)
        {
            _userRepo =userRepo;
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
        }

        /// <summary>
        /// The user manager
        /// </summary>
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }


        
        // GET api/Account/UserInfo
        /// <summary>
        /// Gets the user info.
        /// </summary>
        /// <returns>The user info</returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            var user = _userRepo.GetUser(User.Identity.GetUserId());
            var userRole = User.IsInRole("Official") ? "Official" : "Swimmer";

            return new UserInfoViewModel
            {
                Email = User.Identity.GetUserName(),
                Address = user.Address,
                Club = user.Club,
                FamilyName = user.FamilyName,
                Gender = user.Gender,
                GivenName = user.GivenName,
                PhoneNumber = user.PhoneNumber,
                Role = userRole,
            };
        }


        /// <summary>
        /// Gets the profile of the swimmer if the account is a official
        /// </summary>
        /// <param name="userId">The user id of the swimmer</param>
        /// <returns>The profile of a swimmer</returns>
        [Authorize(Roles = "Official")]
        [Route("GetProfileOfSwimmer")]
        public IHttpActionResult GetProfileOfSwimmer(string userId)
        {
            var user = _userRepo.GetUser(userId);
            
            if (user == null)
            {

                return BadRequest("User Not found");
            }
            var roleId = user.Roles.First().RoleId;

            if (roleId == "Official")
            {
                return Content(HttpStatusCode.Forbidden,"You can not access another officials account");
            }

            var userInfoView = new UserInfoViewModel()
            {
                Address = user.Address,
                Club = user.Club,
                Email = user.Email,
                FamilyName = user.FamilyName,
                Gender = user.Gender,
                GivenName = user.GivenName,
                PhoneNumber = user.PhoneNumber,
                Role = "Swimmer"
            };
            return Ok(userInfoView);
        }


        
        /// <summary>
        /// Get the profiles of swimmers by that name
        /// </summary>
        /// <param name="GivenName">The given name of the swimmer</param>
        /// <param name="FamilyName">The family name of the swimmer</param>
        /// <returns>A list of swimmers that match that criteria</returns>
        [Authorize(Roles = "Official")]
        [Route("GetProfileOfSwimmer")]
        public IHttpActionResult GetProfilesOfSwimmerByName(string GivenName,string FamilyName)
        {
            var users = _userRepo.GetUserByName(GivenName,FamilyName);

            if (users.Count==0)
            {

                return BadRequest("User Not found by that name");
            }

            foreach (var user in users)
            {
                if (user.Roles.First().RoleId == "Official")
                {
                    users.Remove(user);
                }
            }

            if (users.Count == 0)
            {
                return BadRequest("No Swimmer found by that name");
            }

            var userInfoViewList = new List<UserInfoViewModel>();
            foreach (var user in users)
            {
                var userInfoView = new UserInfoViewModel()
                {
                    Address = user.Address,
                    Club = user.Club,
                    Email = user.Email,
                    FamilyName = user.FamilyName,
                    Gender = user.Gender,
                    GivenName = user.GivenName,
                    PhoneNumber = user.PhoneNumber,
                    Role = "Swimmer"
                };
                userInfoViewList.Add(userInfoView);

            }
            
            return Ok(userInfoViewList);
        }



        /// <summary>
        /// Get the profiles of swimmers by that name
        /// </summary>
        /// <param name="GivenName">The given name of the swimmer</param>
        /// <param name="FamilyName">The family name of the swimmer</param>
        /// <returns>A list of swimmers that match that criteria</returns>
        [Authorize(Roles = "Official")]
        [Route("GetProfileOfSwimmer")]
        public IHttpActionResult GetProfilesOfSwimmerByAge(int Age)
        {
            var users = _userRepo.GetUserByAge(Age);

            if (users.Count == 0)
            {

                return BadRequest("User Not found by that name");
            }

            foreach (var user in users)
            {
                if (user.Roles.First().RoleId == "Official")
                {
                    users.Remove(user);
                }
            }

            if (users.Count == 0)
            {
                return BadRequest("No Swimmer found by that name");
            }

            var userInfoViewList = new List<UserInfoViewModel>();
            foreach (var user in users)
            {
                var userInfoView = new UserInfoViewModel()
                {
                    Address = user.Address,
                    Club = user.Club,
                    Email = user.Email,
                    FamilyName = user.FamilyName,
                    Gender = user.Gender,
                    GivenName = user.GivenName,
                    PhoneNumber = user.PhoneNumber,
                    Role = "Swimmer"
                };
                userInfoViewList.Add(userInfoView);

            }

            return Ok(userInfoViewList);
        }


        /// <summary>
        /// Updates the info of a account
        /// </summary>
        /// <param name="model">The model of the update info</param>
        /// <returns>The action result</returns>
        //POST api/Account/UpdateInfo
        [Route("UpdateInfo")]
        public async Task<IHttpActionResult> UpdateInfo(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userRepo.GetUser(User.Identity.GetUserId());
            user.Gender = model.Gender;
            user.Address = model.Address;
            user.Club = model.Club;
            user.FamilyName = model.FamilyName;
            user.GivenName = model.GivenName;
            user.DateOfBirth = model.DateOfBirth;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;


            IdentityResult result = await _userManager.UpdateAsync(user);

            return !result.Succeeded ? GetErrorResult(result) : Ok();
        }

        //POST api/Account/UpdateInfoOfSwimmer
        /// <summary>
        /// Updates swimmer info
        /// </summary>
        /// <param name="model"> the model required to update the info of the swimmer</param>
        /// <returns>Whether successful or not</returns>
        [Authorize(Roles = "Official")]
        [Route("UpdateInfoOfSwimmer")]
        public async Task<IHttpActionResult> UpdateInfoOfSwimmer(UpdateSwimmerInfoModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userRepo.GetUser(model.userId);
            if (user == null)
            {
                return BadRequest("No User found with that Id");
            }
            var roleId = user.Roles.First().RoleId;
            if (roleId == "Official")
            {
                return BadRequest("Can not edit another officials account");
            }
            user.Gender = model.Gender;
            user.Address = model.Address;
            user.Club = model.Club;
            user.FamilyName = model.FamilyName;
            user.GivenName = model.GivenName;
            user.DateOfBirth = model.DateOfBirth;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;


            IdentityResult result = await _userManager.UpdateAsync(user);

            return !result.Succeeded ? GetErrorResult(result) : Ok();
        }

        // POST api/Account/Logout
        /// <summary>
        /// Logs out user
        /// </summary>
        /// <returns>Whether successful or not</returns>
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // GET api/Account/ManageInfo?returnUrl=%2F&generateState=true
        /// <summary>
        /// Gets the manage info view
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="generateState"></param>
        /// <returns></returns>
        [Route("ManageInfo")]
        public async Task<ManageInfoViewModel> GetManageInfo(string returnUrl, bool generateState = false)
        {
            IdentityUser user = await UserManager.FindByIdAsync(User.Identity.GetUserId());

            if (user == null)
            {
                return null;
            }

            List<UserLoginInfoViewModel> logins = new List<UserLoginInfoViewModel>();

            foreach (IdentityUserLogin linkedAccount in user.Logins)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = linkedAccount.LoginProvider,
                    ProviderKey = linkedAccount.ProviderKey
                });
            }

            if (user.PasswordHash != null)
            {
                logins.Add(new UserLoginInfoViewModel
                {
                    LoginProvider = LocalLoginProvider,
                    ProviderKey = user.UserName,
                });
            }

            return new ManageInfoViewModel
            {
                LocalLoginProvider = LocalLoginProvider,
                Email = user.UserName,
                Logins = logins,
                ExternalLoginProviders = GetExternalLogins(returnUrl, generateState)
            };
        }

        // POST api/Account/ChangePassword
        /// <summary>
        /// Changes password of the account logged in
        /// </summary>
        /// <param name="model">Change password model</param>
        /// <returns></returns>
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        /// <summary>
        /// Sets the password of a account
        /// </summary>
        /// <param name="model">Set password model</param>
        /// <returns></returns>
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/AddExternalLogin
        /// <summary>
        /// For adding a external login - currently not implemented 
        /// </summary>
        /// <param name="model">External login model</param>
        /// <returns></returns>
        [Route("AddExternalLogin")]
        public async Task<IHttpActionResult> AddExternalLogin(AddExternalLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);

            AuthenticationTicket ticket = AccessTokenFormat.Unprotect(model.ExternalAccessToken);

            if (ticket == null || ticket.Identity == null || (ticket.Properties != null
                && ticket.Properties.ExpiresUtc.HasValue
                && ticket.Properties.ExpiresUtc.Value < DateTimeOffset.UtcNow))
            {
                return BadRequest("External login failure.");
            }

            ExternalLoginData externalData = ExternalLoginData.FromIdentity(ticket.Identity);

            if (externalData == null)
            {
                return BadRequest("The external login is already associated with an account.");
            }

            IdentityResult result = await UserManager.AddLoginAsync(User.Identity.GetUserId(),
                new UserLoginInfo(externalData.LoginProvider, externalData.ProviderKey));

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // POST api/Account/RemoveLogin
        /// <summary>
        /// For removing a login
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("RemoveLogin")]
        public async Task<IHttpActionResult> RemoveLogin(RemoveLoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result;

            if (model.LoginProvider == LocalLoginProvider)
            {
                result = await UserManager.RemovePasswordAsync(User.Identity.GetUserId());
            }
            else
            {
                result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(),
                    new UserLoginInfo(model.LoginProvider, model.ProviderKey));
            }

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogin
        /// <summary>
        /// For getting a external login - currently not implemented
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalCookie)]
        [AllowAnonymous]
        [Route("ExternalLogin", Name = "ExternalLogin")]
        public async Task<IHttpActionResult> GetExternalLogin(string provider, string error = null)
        {
            if (error != null)
            {
                return Redirect(Url.Content("~/") + "#error=" + Uri.EscapeDataString(error));
            }

            if (!User.Identity.IsAuthenticated)
            {
                return new ChallengeResult(provider, this);
            }

            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            if (externalLogin == null)
            {
                return InternalServerError();
            }

            if (externalLogin.LoginProvider != provider)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                return new ChallengeResult(provider, this);
            }

            ApplicationUser user = await UserManager.FindAsync(new UserLoginInfo(externalLogin.LoginProvider,
                externalLogin.ProviderKey));

            bool hasRegistered = user != null;

            if (hasRegistered)
            {
                Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
                
                 ClaimsIdentity oAuthIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    OAuthDefaults.AuthenticationType);
                ClaimsIdentity cookieIdentity = await user.GenerateUserIdentityAsync(UserManager,
                    CookieAuthenticationDefaults.AuthenticationType);

                AuthenticationProperties properties = ApplicationOAuthProvider.CreateProperties(user.UserName);
                Authentication.SignIn(properties, oAuthIdentity, cookieIdentity);
            }
            else
            {
                IEnumerable<Claim> claims = externalLogin.GetClaims();
                ClaimsIdentity identity = new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);
                Authentication.SignIn(identity);
            }

            return Ok();
        }

        // GET api/Account/ExternalLogins?returnUrl=%2F&generateState=true
        /// <summary>
        /// Gets external logins available - currently not working as not implemented
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="generateState"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("ExternalLogins")]
        public IEnumerable<ExternalLoginViewModel> GetExternalLogins(string returnUrl, bool generateState = false)
        {
            IEnumerable<AuthenticationDescription> descriptions = Authentication.GetExternalAuthenticationTypes();
            List<ExternalLoginViewModel> logins = new List<ExternalLoginViewModel>();

            string state;

            if (generateState)
            {
                const int strengthInBits = 256;
                state = RandomOAuthStateGenerator.Generate(strengthInBits);
            }
            else
            {
                state = null;
            }

            foreach (AuthenticationDescription description in descriptions)
            {
                ExternalLoginViewModel login = new ExternalLoginViewModel
                {
                    Name = description.Caption,
                    Url = Url.Route("ExternalLogin", new
                    {
                        provider = description.AuthenticationType,
                        response_type = "token",
                        client_id = Startup.PublicClientId,
                        redirect_uri = new Uri(Request.RequestUri, returnUrl).AbsoluteUri,
                        state = state
                    }),
                    State = state
                };
                logins.Add(login);
            }

            return logins;
        }

        // POST api/Account/Register
        /// <summary>
        /// For registering a account
        /// </summary>
        /// <param name="model">The registration binding model</param>
        /// <returns></returns>
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (model.Gender != "M" && model.Gender != "F" )
            {
                return BadRequest("Gender Must either be M or F");
            }

            if (model.AccountType != "Swimmer" && model.AccountType != "Official")
            {
                return BadRequest("Roles available are Official or Swimmer");
            }

            var user = new ApplicationUser()
            {
                UserName = model.Email, Email = model.Email, Address = model.Address, Gender = model.Gender,
                PhoneNumber = model.PhoneNumber, DateOfBirth = model.DateOfBirth, FamilyName = model.FamilyName,
                GivenName = model.GivenName,Club = model.Club
            };


            IdentityResult accountResult = await UserManager.CreateAsync(user, model.Password);
            

            if (!accountResult.Succeeded)
            {
                return GetErrorResult(accountResult);
            }

            IdentityResult roleResult = await UserManager.AddToRoleAsync(user.Id, model.AccountType);

            if (!roleResult.Succeeded)
            {
                return GetErrorResult(roleResult);
            }


            return Ok();
        }

        // POST api/Account/RegisterExternal
        /// <summary>
        /// For registering a externla login- currently not working / implemented
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [OverrideAuthentication]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("RegisterExternal")]
        public async Task<IHttpActionResult> RegisterExternal(RegisterExternalBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var info = await Authentication.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return InternalServerError();
            }

            var user = new ApplicationUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            result = await UserManager.AddLoginAsync(user.Id, info.Login);
            if (!result.Succeeded)
            {
                return GetErrorResult(result); 
            }
            return Ok();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }
}
