using System;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Identity.Domain.AggregatesModel.UserAggregate;
using Identity.Infrastructure.Repositories;
using MediatR;
using System.Net;
using Identity.API.Application.DTO;
using Identity.API.Application.Commands;
using Identity.API.Application.IntegrationEvents;
using Identity.API.Application.IntegrationEvents.Events;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Identity.API.Controllers
{

#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    public class AccountController : Controller
    {
        //private readonly InMemoryUserLoginService _loginService;
        private readonly ILoginService<ApplicationUser> _loginService;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly ILogger<AccountController> _logger;
        private readonly ApplicationUserManager _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly ITokenService _tokenService;
        private readonly IIdentityIntegrationEventService _identityIntegrationEventService;

        public AccountController(

            //InMemoryUserLoginService loginService,
            ILoginService<ApplicationUser> loginService,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            ILogger<AccountController> logger,
            ApplicationUserManager userManager,
            IConfiguration configuration,
            IMediator mediator,
            ITokenService tokenService,
            IIdentityIntegrationEventService identityIntegrationEventService
            )
        {
            _loginService = loginService;
            _interaction = interaction;
            _clientStore = clientStore;
            _logger = logger;
            _userManager = userManager;
            _configuration = configuration;
            _mediator = MethodParameterChecker.CheckUp(mediator);
            _identityIntegrationEventService = MethodParameterChecker.CheckUp(identityIntegrationEventService);
            tokenService = _tokenService;
        }


        ///// <summary>
        ///// Show login page
        ///// </summary>
        [Route("[controller]/Login")]
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                throw new NotImplementedException("External login is not implemented!");
            }

            var vm = await BuildLoginViewModelAsync(returnUrl, context);

            ViewData["ReturnUrl"] = returnUrl;

            return View(vm);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [Route("[controller]/Login")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _loginService.FindByUsername(model.Username);
                _logger.LogInformation("ValidateCredentials: ");
                if (await _loginService.ValidateCredentials(user, model.Password))
                {
                    _logger.LogInformation("ValidateCredentials: true");
                    var tokenLifetime = _configuration.GetValue("TokenLifetimeMinutes", 120);
                    _logger.LogInformation("tokenLifetime:" + tokenLifetime);
                    var props = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime),
                        AllowRefresh = true,
                        RedirectUri = model.ReturnUrl
                    };
                    _logger.LogInformation("RedirectUri:" + model.ReturnUrl);
                    if (model.RememberMe)
                    {
                        var permanentTokenLifetime = _configuration.GetValue("PermanentTokenLifetimeDays", 365);

                        props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(permanentTokenLifetime);
                        props.IsPersistent = true;
                    };
                    _logger.LogInformation("SignInAsync:");
                    await _loginService.SignInAsync(user, props);
                    _logger.LogInformation("RedirectUri:" + true);
                    // make sure the returnUrl is still valid, and if yes - redirect back to authorize endpoint
                    if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                    {
                        _logger.LogInformation("Redirect:" + true);
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("~/");
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }

            // something went wrong, show form with error
            var vm = await BuildLoginViewModelAsync(model);

            ViewData["ReturnUrl"] = model.ReturnUrl;

            return View(vm);
        }


        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl, AuthorizationRequest context)
        {
            var allowLocal = true;
            if (context?.Client?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.Client.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                }
            }

            return new LoginViewModel
            {
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
            };
        }
        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginViewModel model)
        {
            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl, context);
            vm.Username = model.Username;
            vm.RememberMe = model.RememberMe;
            return vm;
        }

        [HttpGet("[controller]/Logout")]
        public async Task<ActionResult> Logout(string logoutId)
        {
            _logger.LogInformation("Logout:");
            if (User.Identity.IsAuthenticated == false)
            {
                _logger.LogInformation("Logout:" + User.Identity.IsAuthenticated);
                // if the user is not authenticated, then just show logged out page
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            //Test for Xamarin. 
            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                //it's safe to automatically sign-out
                return await Logout(new LogoutViewModel { LogoutId = logoutId });
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            var vm = new LogoutViewModel
            {
                LogoutId = logoutId
            };
            return View(vm);
        }

        [HttpPost("[controller]/Logout")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Logout(LogoutViewModel model)
        {
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if (idp != null && idp != IdentityServerConstants.LocalIdentityProvider)
            {
                if (model.LogoutId == null)
                {
                    // if there's no current logout context, we need to create one
                    // this captures necessary info from the current logged in user
                    // before we signout and redirect away to the external IdP for signout
                    model.LogoutId = await _interaction.CreateLogoutContextAsync();
                }

                string url = "/Account/Logout?logoutId=" + model.LogoutId;

                try
                {

                    // hack: try/catch to handle social providers that throw
                    await HttpContext.SignOutAsync(idp, new AuthenticationProperties
                    {
                        RedirectUri = url
                    });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "LOGOUT ERROR: {ExceptionMessage}", ex.Message);
                }
            }

            // delete authentication cookie
            await HttpContext.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            // get context information (client name, post logout redirect URI and iframe for federated signout)
            var logout = await _interaction.GetLogoutContextAsync(model.LogoutId);

            return Redirect(_configuration[logout.ClientName]);
        }

        [HttpPost("[controller]/DeviceLogOut")]
        public async Task<ActionResult> DeviceLogOut(string redirectUrl)
        {
            // delete authentication cookie
            await HttpContext.SignOutAsync();

            // set this so UI rendering sees an anonymous user
            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            return Redirect(redirectUrl);
        }

        [HttpGet("[controller]/Redirecting")]
        public ActionResult Redirecting()
        {
            return View();
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        [HttpGet("[controller]/Ping")]
        public ActionResult Ping()
        {
            try
            {
                return Content("Reply is Ok");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [Route("api/v1/[controller]/Register")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ActionResult<UserPatientDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserPatientDTO>> RegisterAsync([FromBody] RegisterCommand registerUserCommand)
        {
            var result = await _mediator.Send(registerUserCommand);
            if (registerUserCommand.ObjectDTO.Id != null)
            {
                var sendTokenEvent = new SendChangePhoneNumberTokenIntegrationEvent(registerUserCommand.ObjectDTO.Id.Value, registerUserCommand.ObjectDTO.PhoneNumber);
                await _identityIntegrationEventService.AddAndSaveEventAsync(sendTokenEvent);
                await _identityIntegrationEventService.PublishThroughEventBusAsync(sendTokenEvent);
            }

            return result;
        }

        [Route("api/v1/[controller]/VerifyRegister")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public virtual async Task<ActionResult<bool>> VerifyRegister([FromBody] VerifyChangePhoneNumberTokenCommand verifyChangePhoneNumberTokenCommand)
        {
            var result = await _mediator.Send(verifyChangePhoneNumberTokenCommand);

            ChangeStaffPhoneNumberIntegrationEvent changeStaffPhoneNumberIntegrationEvent = new ChangeStaffPhoneNumberIntegrationEvent(verifyChangePhoneNumberTokenCommand.UserId, verifyChangePhoneNumberTokenCommand.PhoneNumber);
            await _identityIntegrationEventService.AddAndSaveEventAsync(changeStaffPhoneNumberIntegrationEvent);
            await _identityIntegrationEventService.PublishThroughEventBusAsync(changeStaffPhoneNumberIntegrationEvent);

            return result;
        }

        [Route("api/v1/[controller]/ResendTwoFactorToken")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public virtual async Task<ActionResult<bool>> ResendTwoFactorToken([FromBody] GenerateChangePhoneNumberTokenCommand generateChangePhoneNumberTokenCommand)
        {
            var token = await _mediator.Send(generateChangePhoneNumberTokenCommand);

            var verifyLookupIntegrationEvent = new VerifyLookupIntegrationEvent(generateChangePhoneNumberTokenCommand.PhoneNumber, token);
            await _identityIntegrationEventService.AddAndSaveEventAsync(verifyLookupIntegrationEvent);
            await _identityIntegrationEventService.PublishThroughEventBusAsync(verifyLookupIntegrationEvent);

            return true;
        }

        [Route("api/v1/[controller]/ForgotPassword")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public virtual async Task<ActionResult<bool>> ForgotPasswordAsync([FromBody] ForgotPasswordDTO request)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            var token = await _mediator.Send(new GenerateResetPasswordTokenCommand(user));

            var forgetPasswordIntegrationEvent = new ForgetPasswordIntegrationEvent(user.PhoneNumber, token);
            await _identityIntegrationEventService.AddAndSaveEventAsync(forgetPasswordIntegrationEvent);
            await _identityIntegrationEventService.PublishThroughEventBusAsync(forgetPasswordIntegrationEvent);

            return true;
        }

        [Route("api/v1/[controller]/ResetPassword")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public virtual async Task<ActionResult<bool>> ResetPasswordAsync([FromBody] ResetPasswordCommand resetPasswordCommand)
        {
            var result = await _mediator.Send(resetPasswordCommand);
            return result;
        }

        [Route("api/v1/[controller]/VerifyForgotPasswordToken")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public virtual async Task<ActionResult<bool>> VerifyForgotPasswordTokenAsync([FromBody] VerifyTwoFactorTokenCommand verifyTwoFactorToken)
        {
            var result = await _mediator.Send(verifyTwoFactorToken);
            return result;
        }

        [Route("api/v1/[controller]/SetIrimcConfirmed")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public virtual async Task SetIrimcConfirmedAsync([FromBody] CheckIrimcMemberInfoCommand CheckIrimcConfirmedCommand)
        {
            await _mediator.Send(CheckIrimcConfirmedCommand);
        }

    }
}