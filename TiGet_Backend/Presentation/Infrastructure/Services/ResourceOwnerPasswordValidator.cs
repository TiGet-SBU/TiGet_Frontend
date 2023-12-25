using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Rhazes.Services.Identity.Domain.AggregatesModel.UserAggregate;
using IdentityServer4.Events;
using static IdentityModel.OidcConstants;
using Rhazes.Services.Identity.API.Application.DTO.IhioViewModels;
using Rhazes.Services.Identity.API.Application.DTO.TenantViewModels;
using System.Threading;
using Microsoft.AspNetCore.Http;
using System;
using Rhazes.Services.PadidarServerIdentity.Models.HumanViewModels;
using System.Linq;
using System.Collections.Generic;
using Rhazes.Services.Identity.Domain.AggregatesModel.RoleAggregate;
using Rhazes.Services.Identity.API.Application.IntegrationEvents.Events;
using Rhazes.Services.Identity.API.Application.IntegrationEvents;
using Rhazes.BuildingBlocks.Common;
using Rhazes.Services.Identity.API.Application.DTO.IrimcViewModels;
using Rhazes.BuildingBlocks.Common.Extensions;
using Rhazes.BuildingBlocks.Common.Utility;
using Microsoft.Extensions.Configuration;

namespace Rhazes.Services.Identity.API.Infrastructure.Services
{
    public class ResourceOwnerPasswordValidator<TUser> : IResourceOwnerPasswordValidator
            where TUser : class
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUserManager _applicationUserManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IIhioService _ihioService;
        private readonly ITenantService _tenantService;
        private readonly IHumanService _humanService;
        private readonly IIrimcService _irimcService;
        private readonly ILogger<ResourceOwnerPasswordValidator<ApplicationUser>> _logger;
        private IEventService _events;
        private readonly IIdentityIntegrationEventService _identityIntegrationEventService;
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceOwnerPasswordValidator{ApplicationUser}"/> class.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="signInManager">The sign in manager.</param>
        /// <param name="events">The events.</param>
        /// <param name="logger">The logger.</param>
        public ResourceOwnerPasswordValidator(
            RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IApplicationUserManager applicationUserManager,
            IIdentityIntegrationEventService identityIntegrationEventService,
            IHttpContextAccessor httpContextAccessor,
            IIhioService ihioService,
            ITenantService tenantService,
            IHumanService humanService,
            IIrimcService irimcService,
            IEventService events,
            ILogger<ResourceOwnerPasswordValidator<ApplicationUser>> logger,
            IConfiguration configuration)
        {
            Configuration = MethodParameterChecker.CheckUp(configuration);
            _roleManager = MethodParameterChecker.CheckUp(roleManager);
            _userManager = MethodParameterChecker.CheckUp(userManager);
            _signInManager = MethodParameterChecker.CheckUp(signInManager);
            _applicationUserManager = MethodParameterChecker.CheckUp(applicationUserManager);
            _identityIntegrationEventService = MethodParameterChecker.CheckUp(identityIntegrationEventService);
            _httpContextAccessor = MethodParameterChecker.CheckUp(httpContextAccessor);
            _ihioService = MethodParameterChecker.CheckUp(ihioService);
            _tenantService = MethodParameterChecker.CheckUp(tenantService);
            _humanService = MethodParameterChecker.CheckUp(humanService);
            _irimcService = MethodParameterChecker.CheckUp(irimcService);
            _events = MethodParameterChecker.CheckUp(events);
            _logger = MethodParameterChecker.CheckUp(logger);
        }

        /// <summary>
        /// Validates the resource owner password credential
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            context.UserName = context.UserName.ConvertDigitsToLatin();
            context.Password = context.Password.ConvertDigitsToLatin();

            ApplicationUser user = null;
            if (_httpContextAccessor.HttpContext.Request.Headers["login-type"] == "IHIO")
            {
                await LoginWithIhio(context, user);
            }
            else if (_httpContextAccessor.HttpContext.Request.Headers["login-type"] == "OTP")
            {
                await LoginWithNationalCode(context, user);
            }
            else if (_httpContextAccessor.HttpContext.Request.Headers["login-type"] == "CITIZEN")
            {
                user = await _userManager.FindByNameAsync(context.UserName);

                if (user != null)
                {
                    await SetCurrentTenant(context, user);

                    var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);

                    if (result.Succeeded)
                    {

                        var sub = await _userManager.GetUserIdAsync(user);

                        _logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                        await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, interactive: false));

                        context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                        return;
                    }
                    else if (result.IsLockedOut)
                    {
                        _logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "locked out", interactive: false));
                    }
                    else if (result.IsNotAllowed)
                    {
                        _logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", interactive: false));
                    }
                    else
                    {
                        _logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", interactive: false));
                    }
                }
                else
                {
                    _logger.LogInformation("No user found matching username: {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
                }

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            }
            else if (_httpContextAccessor.HttpContext.Request.Headers["login-type"] == "PHARMACY")
            {
                await LoginPharmacy(context, user);
            }
            else
            {
                user = await _userManager.FindByNameAsync(context.UserName);

                if (user != null)
                {
                    await SetCurrentTenant(context, user);

                    var result = await _signInManager.CheckPasswordSignInAsync(user, context.Password, true);

                    if (result.Succeeded)
                    {
                        if (user.RegisterCompleted == true && user.PhoneNumberConfirmed == false)
                        {
                            context.Result = new GrantValidationResult(
                               TokenRequestErrors.InvalidGrant,
                               "PHONE_NUMBER_NOT_CONFIRMED");
                            return;
                        }

                        if (user.RegisterCompleted == true && user.IrimcConfirmed == false && (user.UserType == UserType.Healer.Id || user.UserType == UserType.Healer_SystemManager.Id))
                        {
                            context.Result = new GrantValidationResult(
                               TokenRequestErrors.InvalidGrant,
                               "IRIMC_NOT_CONFIRMED");
                            return;
                        }

                        if (user.RegisterCompleted == true && user.UserType != UserType.Admin.Id && (user.CurrentTenantId == null || Guid.Empty.Equals(user.CurrentTenantId)))
                        {
                            context.Result = new GrantValidationResult(
                               TokenRequestErrors.InvalidGrant,
                               "CURRENT_TENANT_IS_EMPTY");
                            return;
                        }


                        var sub = await _userManager.GetUserIdAsync(user);


                        _logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                        await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, sub, context.UserName, interactive: false));

                        context.Result = new GrantValidationResult(sub, AuthenticationMethods.Password);
                        return;
                    }
                    else if (result.IsLockedOut)
                    {
                        _logger.LogInformation("Authentication failed for username: {username}, reason: locked out", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "locked out", interactive: false));
                    }
                    else if (result.IsNotAllowed)
                    {
                        _logger.LogInformation("Authentication failed for username: {username}, reason: not allowed", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "not allowed", interactive: false));
                    }
                    else
                    {
                        _logger.LogInformation("Authentication failed for username: {username}, reason: invalid credentials", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid credentials", interactive: false));
                    }
                }
                else
                {
                    _logger.LogInformation("No user found matching username: {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
                }

                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
            }
        }





        private async Task LoginWithIhio(ResourceOwnerPasswordValidationContext context, ApplicationUser user)
        {
            _logger.LogInformation("Authentication LoginWithIhio for username: {username}  password:{password}", context.UserName, context.Password);
            Guid invitionId = string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["invitation"].ToString()) ? Guid.Empty : Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers["invitation"].ToString());
            UserSessionDTO ihioResult = new UserSessionDTO();
            if (_httpContextAccessor.HttpContext.Request.Headers["is-verified"].ToString() != "true")
                ihioResult = await _ihioService.GetUserSessionAsync(new UserSessionRequestDTO() { Username = context.UserName, Password = context.Password, HandleException = true, ReadTheCache = false }, new CancellationToken());
            else
                ihioResult = await _ihioService.GetUserSessionAsync(new UserSessionRequestDTO() { Username = context.UserName, Password = context.Password, HandleException = true, ReadTheCache = true }, new CancellationToken());

            if ((ihioResult.IsTwoStep == true || ihioResult.Message == "OTP_REQUIRED") && _httpContextAccessor.HttpContext.Request.Headers["is-verified"].ToString() != "true")
            {
                _logger.LogInformation("don't create user username: {username} message => IHIO_IS_TWO_STEP", context.UserName);
                context.Result = new GrantValidationResult(
                   TokenRequestErrors.InvalidGrant,
                   "IHIO_IS_TWO_STEP");
                return;
            }

            if (!string.IsNullOrEmpty(ihioResult.Message) && ihioResult.Message != "OTP_REQUIRED")
            {
                _logger.LogError("don't create user username: {username} message => {message}", context.UserName, ihioResult.Message);
                context.Result = new GrantValidationResult(
                   TokenRequestErrors.InvalidGrant,
                   ihioResult.Message);
                return;
            }


            user = await _userManager.FindByNameAsync(ihioResult.NationalCode);

            // کد ملی اکانت کاربری تست بیمه سلامت
            if (Configuration.GetValue<string>("AccountIhioTest").Split(" ").Contains(context.UserName))
            {
                if (context.UserName.Equals("006151553159936"))
                {
                    ihioResult.NationalCode = "2280327082";
                    ihioResult.PartnerName = "2280327082";
                    ihioResult.MedicalLicenseNumber = "181603";
                    ihioResult.PhoneNumber = "09031229250";
                    ihioResult.FullName = "سيدمهدي طباطبائي";
                    ihioResult.FirstName = "سيدمهدي";
                    ihioResult.LastName = "طباطبائي";
                    user = await _userManager.FindByNameAsync(ihioResult.NationalCode);
                }
                else
                {
                    _logger.LogInformation("user username: {username} message => user not found in db", context.UserName);

                    ihioResult.MedicalLicenseNumber = string.IsNullOrEmpty(ihioResult.MedicalLicenseNumber) ? "1" : ihioResult.MedicalLicenseNumber;
                    //var memberResult = await _irimcService.GetMemberByMcCodeAsync(new GetMemberByMcCodeDTO() 
                    //{ McCode = ihioResult.MedicalLicenseNumber }, new CancellationToken());
                    ihioResult.FullName = ihioResult.FullName;
                    ihioResult.FirstName = ihioResult.FullName;
                    ihioResult.LastName = ihioResult.FullName;
                }

            }
            // اصلاح کد نظام پزشکی اتباع
            else if (ihioResult.NationalCode != null && ihioResult.NationalCode.Length > 10 && user == null)
            {
                _logger.LogInformation("user username: {username} message => GetMemberByMcCodeAsync for doctor atbaa ", context.UserName);

                var memberResult = await _irimcService.GetMemberByMcCodeAsync(new GetMemberByMcCodeDTO() { McCode = string.Concat("ات-", ihioResult.MedicalLicenseNumber) }, new CancellationToken());
                if (memberResult != null)
                {
                    ihioResult.FullName = memberResult.FirstName + " " + memberResult.LastName;
                    ihioResult.FirstName = memberResult.FirstName;
                    ihioResult.LastName = memberResult.LastName;
                    ihioResult.NationalCode = memberResult.NationalCode;
                    ihioResult.MedicalLicenseNumber = memberResult.McCode;
                }
                if (ihioResult.NationalCode != null)
                    user = await _userManager.FindByNameAsync(ihioResult.NationalCode);
            }
            else if (ihioResult.NationalCode != null && user == null || user?.UserType == UserType.Patient.Id)
            {
                _logger.LogInformation("user username: {username} message => user not found in db", context.UserName);
                var memberResult = await _irimcService.GetMemberInfoByNationalCodeAsync(new GetMemberByNationalCodeDTO() { NationalCode = ihioResult.NationalCode }, new CancellationToken());
                if (memberResult != null)
                {
                    ihioResult.FullName = memberResult.FirstName + " " + memberResult.LastName;
                    ihioResult.FirstName = memberResult.FirstName;
                    ihioResult.LastName = memberResult.LastName;
                    ihioResult.MedicalLicenseNumber = memberResult.McCode;
                }
                if (ihioResult.NationalCode != null)
                    user = await _userManager.FindByNameAsync(ihioResult.NationalCode);
            }

            if (user == null && ihioResult.MedicalLicenseNumber != null && ihioResult.MedicalLicenseNumber != "0")
            {

                _logger.LogInformation("start create user username: {username}", context.UserName);
                var id = Guid.NewGuid();
                var date = DateTime.Now;

                user = new ApplicationUser()
                {
                    Id = id,
                    Name = ihioResult.FirstName,
                    LastName = ihioResult.LastName,
                    UserName = ihioResult.NationalCode,
                    PhoneNumber = !string.IsNullOrEmpty(ihioResult.PhoneNumber) ? ihioResult.PhoneNumber : ihioResult.PhoneNumber,
                    Email = $"default@gmail.com",
                    NormalizedEmail = $"default@gmail.com".ToUpper(),
                    UserType = UserType.Healer.Id,
                    IrimcConfirmed = true,
                    RegisterCompleted = true,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    Gender = ihioResult.Gender,
                    MedicalLicenseNumber = ihioResult.MedicalLicenseNumber,
                    CreateById = id,
                    ModifiedById = id,
                    CreateDate = date,
                    ModifiedDate = date
                };
                user.UserRoles = new List<ApplicationUserRole>();

                var result = await _userManager.CreateAsync(user, context.Password);



                if (result.Succeeded)
                {
                    _logger.LogInformation("finish create user username: {username} userId:{id}", context.UserName, user.Id);
                    try
                    {
                        _logger.LogInformation("start create tenant username: {username} userId:{id}", context.UserName, user.Id);
                        user.CurrentTenantId = await _tenantService.CreateTenantByIhioAsync(new CreateTenantByIhioDTO()
                        {
                            CurrentTenantId = Guid.Empty,
                            FullName = ihioResult.FullName,
                            Password = context.Password,
                            Username = context.UserName,
                            PhoneNumber = !string.IsNullOrEmpty(ihioResult.PhoneNumber) ? ihioResult.PhoneNumber : ihioResult.PhoneNumber,
                            UserId = id,
                            Gender = ihioResult.Gender,
                        }, new CancellationToken());

                        _logger.LogInformation("finish create tenant username: {username} CurrentTenantId:{CurrentTenantId}", context.UserName, user.CurrentTenantId);
                        _logger.LogInformation("start create role username: {username} UserType:{name}", context.UserName, UserType.Healer.Name);

                        if (!Guid.Empty.Equals(invitionId))
                            await CreateUserTenantWithInvitation(invitionId, user);

                        var role = await _roleManager.FindByNameAsync(UserType.Healer.Name);
                        var roleBasic = await _roleManager.FindByNameAsync("Basic");
                        user.UserRoles.Add(new ApplicationUserRole() { UserId = id, TenantId = user.CurrentTenantId, RoleId = role.Id });
                        user.UserRoles.Add(new ApplicationUserRole() { UserId = id, TenantId = user.CurrentTenantId, RoleId = roleBasic.Id });
                        await _userManager.UpdateAsync(user);
                        _logger.LogInformation("finish create role username: {username} UserType:{name}", context.UserName, UserType.Healer.Name);
                        _logger.LogInformation("start create staff username: {username}", context.UserName);
                        var staffdate = await _humanService.AddStaffAsync(new StaffRequestDTO()
                        {
                            Name = ihioResult.FirstName,
                            LastName = ihioResult.LastName,
                            NationalCode = ihioResult.NationalCode,
                            PhoneNumber = !string.IsNullOrEmpty(ihioResult.PhoneNumber) ? ihioResult.PhoneNumber : ihioResult.PhoneNumber,
                            FullName = ihioResult.FullName,
                            Gender = ihioResult.Gender,
                            MedicalLicenseNumber = ihioResult.MedicalLicenseNumber,
                            NationalityId = Guid.Parse("5BF70ADB-A059-4BBC-96D3-3BF54F19DB7D"),
                            UserType = UserType.Healer.Id,
                            UserId = id
                        }, new CancellationToken());

                        if (!string.IsNullOrEmpty(staffdate.Message))
                        {
                            _logger.LogInformation("dont create staff username: {username} message:{message}", context.UserName, staffdate.Message);
                            context.Result = new GrantValidationResult(
                               TokenRequestErrors.InvalidGrant,
                               staffdate.Message);

                            await _userManager.DeleteAsync(user);
                            if (!Guid.Empty.Equals(user.CurrentTenantId))
                                await _tenantService.DeleteTenantAsync(user.CurrentTenantId, user.Id, new CancellationToken());

                            return;
                        }


                        _logger.LogInformation("finish create staff username: {username} staffId:{staffId}", context.UserName, staffdate.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("don't create user username: {username} exception:{exception} message => {message}", context.UserName, ex, ex.Message);

                        await _userManager.DeleteAsync(user);
                        if (!Guid.Empty.Equals(user.CurrentTenantId))
                            await _tenantService.DeleteTenantAsync(user.CurrentTenantId, user.Id, new CancellationToken());
                    }
                }
            }
            else if (user != null && user.UserType == UserType.Patient.Id)
            {
                _logger.LogInformation("exist user username: {username} userId:{id}", context.UserName, user.Id);
                try
                {
                    _logger.LogInformation("start create tenant username: {username} userId:{id}", context.UserName, user.Id);
                    user.CurrentTenantId = await _tenantService.CreateTenantByIhioAsync(new CreateTenantByIhioDTO()
                    {
                        CurrentTenantId = user.CurrentTenantId,
                        FullName = ihioResult.FullName,
                        Password = context.Password,
                        Username = context.UserName,
                        PhoneNumber = !string.IsNullOrEmpty(ihioResult.PhoneNumber) ? ihioResult.PhoneNumber : ihioResult.PhoneNumber,
                        UserId = user.Id,
                        Gender = ihioResult.Gender,
                    }, new CancellationToken());
                    _logger.LogInformation("finish create tenant username: {username} CurrentTenantId:{CurrentTenantId}", context.UserName, user.CurrentTenantId);
                    _logger.LogInformation("start create role username: {username} UserType:{name}", context.UserName, UserType.Healer.Name);

                    if (!Guid.Empty.Equals(invitionId))
                        await CreateUserTenantWithInvitation(invitionId, user);


                    user.MedicalLicenseNumber = ihioResult.MedicalLicenseNumber;
                    user.IrimcConfirmed = true;
                    user.RegisterCompleted = true;
                    user.PhoneNumber = ihioResult.PhoneNumber; 
                    user.PhoneNumberConfirmed = true;
                    user.UserType = UserType.Healer.Id;
                    user.UserRoles = new List<ApplicationUserRole>();
                    var role = await _roleManager.FindByNameAsync(UserType.Healer.Name);
                    var roleBasic = await _roleManager.FindByNameAsync("Basic");
                    user.UserRoles.Add(new ApplicationUserRole() { UserId = user.Id, TenantId = user.CurrentTenantId, RoleId = role.Id });
                    user.UserRoles.Add(new ApplicationUserRole() { UserId = user.Id, TenantId = user.CurrentTenantId, RoleId = roleBasic.Id });
                    await _userManager.UpdateAsync(user);
                    _logger.LogInformation("finish create role username: {username} UserType:{name}", context.UserName, UserType.Healer.Name);
                    _logger.LogInformation("start create staff username: {username}", context.UserName);

                    var staffdate = await _humanService.AddStaffAsync(new StaffRequestDTO()
                    {
                        Name = ihioResult.FirstName,
                        LastName = ihioResult.LastName,
                        NationalCode = ihioResult.NationalCode,
                        PhoneNumber = !string.IsNullOrEmpty(ihioResult.PhoneNumber) ? ihioResult.PhoneNumber : ihioResult.PhoneNumber,
                        FullName = ihioResult.FullName,
                        Gender = ihioResult.Gender,
                        MedicalLicenseNumber = ihioResult.MedicalLicenseNumber,
                        NationalityId = Guid.Parse("5BF70ADB-A059-4BBC-96D3-3BF54F19DB7D"),
                        UserType = UserType.Healer.Id,
                        UserId = user.Id
                    }, new CancellationToken());

                    if (!string.IsNullOrEmpty(staffdate.Message))
                    {
                        _logger.LogInformation("dont create staff username: {username} message:{message}", context.UserName, staffdate.Message);
                        context.Result = new GrantValidationResult(
                           TokenRequestErrors.InvalidGrant,
                           staffdate.Message);

                        await _userManager.DeleteAsync(user);
                        if (!Guid.Empty.Equals(user.CurrentTenantId))
                            await _tenantService.DeleteTenantAsync(user.CurrentTenantId, user.Id, new CancellationToken());

                        return;
                    }

                    _logger.LogInformation("finish create staff username: {username} staffId:{staffId}", context.UserName, staffdate.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError("don't create user username: {username} exception:{exception} message => {message}", context.UserName, ex, ex.Message);

                    if (!Guid.Empty.Equals(user.CurrentTenantId))
                        await _tenantService.DeleteTenantAsync(user.CurrentTenantId, user.Id, new CancellationToken());
                }
            }
            else if (user != null && user.UserType == UserType.Healer.Id)
            {
                _logger.LogInformation("exist user username: {username} userId:{id} and RegisterCompleted", context.UserName, user.Id);
                _logger.LogInformation("start create tenant username: {username} userId:{id}", context.UserName, user.Id);
                user.CurrentTenantId = await _tenantService.CreateTenantByIhioAsync(new CreateTenantByIhioDTO()
                {
                    CurrentTenantId = user.CurrentTenantId,
                    FullName = ihioResult.FullName,
                    Password = context.Password,
                    Username = context.UserName,
                    PhoneNumber = !string.IsNullOrEmpty(ihioResult.PhoneNumber) ? ihioResult.PhoneNumber : user.PhoneNumber,
                    UserId = user.Id,
                    Gender = ihioResult.Gender,
                }, new CancellationToken());
                _logger.LogInformation("finish create tenant username: {username} CurrentTenantId:{CurrentTenantId}", context.UserName, user.CurrentTenantId);
                _logger.LogInformation("start create role username: {username} UserType:{name}", context.UserName, UserType.Healer.Name);

                if (!Guid.Empty.Equals(invitionId))
                    await CreateUserTenantWithInvitation(invitionId, user);

                user.RegisterCompleted = true;
                user.PhoneNumberConfirmed = true;
                user.UserType = UserType.Healer.Id;
                user.UserRoles = new List<ApplicationUserRole>();
                var userRoles = await _applicationUserManager.GetRolesAsync(user, new List<Guid>() { user.CurrentTenantId });
                var role = await _roleManager.FindByNameAsync(UserType.Healer.Name);
                var roleBasic = await _roleManager.FindByNameAsync("Basic");
                if (!userRoles.Select(x => x.Name).Contains(role.Name))
                {
                    user.UserRoles = new List<ApplicationUserRole>();
                    user.UserRoles.Add(new ApplicationUserRole() { UserId = user.Id, TenantId = user.CurrentTenantId, RoleId = role.Id });
                    user.UserRoles.Add(new ApplicationUserRole() { UserId = user.Id, TenantId = user.CurrentTenantId, RoleId = roleBasic.Id });
                }
                await _userManager.UpdateAsync(user);
                _logger.LogInformation("finish create role username: {username} UserType:{name}", context.UserName, UserType.Healer.Name);
            }
            else
            {
                _logger.LogError("Credentials validated for username: {username} message => invalid username", context.UserName);
                await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
                return;
            }

            await SetCurrentTenant(context, user);
            if (!string.IsNullOrEmpty(context.Result.ErrorDescription))
                return;

            _logger.LogError("Credentials validated for username: {username} message => invalid username", context.UserName);
            await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, user.Id.ToString(), context.UserName, interactive: false));
            context.Result = new GrantValidationResult(user.Id.ToString(), AuthenticationMethods.Password);
            return;
        }


        private async Task LoginWithNationalCode(ResourceOwnerPasswordValidationContext context, ApplicationUser user)
        {
            _logger.LogInformation("Authentication LoginWithNationalCode for username: {username}  password:{password}", context.UserName, context.Password);

            if (!ValidationHelper.IsValidNationalCode(context.UserName))
            {
                context.Result = new GrantValidationResult(
                              TokenRequestErrors.InvalidGrant,
                              "NATIONAL_CODE_IS_INVALID");
                _logger.LogError("don't create user username: {username} message => NATIONAL_CODE_IS_INVALID", context.UserName);
                return;
            }

            Guid invitionId = string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers["invitation"].ToString()) ? Guid.Empty : Guid.Parse(_httpContextAccessor.HttpContext.Request.Headers["invitation"].ToString());

            user = await _userManager.FindByNameAsync(context.UserName);
            bool phoneNumberIrimcConfirmed = true;
            if (user == null)
            {
                _logger.LogInformation("user username: {username} message => user not found in db", context.UserName);
                var memberResult = await _irimcService.GetMemberInfoByNationalCodeAsync(new GetMemberByNationalCodeDTO() { NationalCode = context.UserName }, new CancellationToken());
                if (memberResult != null)
                {
                    _logger.LogInformation("user username: {username} GetMemberByNationalCode => {message}", context.UserName, memberResult);
                    if (!await _irimcService.CheckValidMemberInMedicalCouncilAsync(new CheckMemberMobileNumberDTO() { NationalCode = context.UserName, MedicalLicenseNumber = memberResult.McCode, PhoneNumber = context.Password }, new CancellationToken()))
                    {
                        _logger.LogError("user username: {username} message => PHONE_NUMBER_IS_NOT_IRIMC", context.UserName);
                        //context.Result = new GrantValidationResult(
                        //  TokenRequestErrors.InvalidGrant,
                        //  "PHONE_NUMBER_IS_NOT_IRIMC");
                        //return;
                        phoneNumberIrimcConfirmed = false;
                    }

                    var id = Guid.NewGuid();
                    var date = DateTime.Now;
                    _logger.LogInformation("start create user username: {username}", context.UserName);
                    user = new ApplicationUser()
                    {
                        Id = id,
                        Name = memberResult.FirstName,
                        LastName = memberResult.LastName,
                        UserName = context.UserName,
                        PhoneNumber = context.Password,
                        Email = $"default@gmail.com",
                        NormalizedEmail = $"default@gmail.com".ToUpper(),
                        UserType = UserType.Healer.Id,
                        IrimcConfirmed = true,
                        PhoneNumberIrimcConfirmed = phoneNumberIrimcConfirmed,
                        EmailConfirmed = true,
                        Gender = GenderType.NotSpecified.Id,
                        MedicalLicenseNumber = memberResult.McCode,
                        Deleted = false,
                        CreateById = id,
                        ModifiedById = id,
                        CreateDate = date,
                        ModifiedDate = date
                    };
                    user.UserRoles = new List<ApplicationUserRole>();

                    var result = await _userManager.CreateAsync(user, context.Password);

                    if (result.Succeeded)
                    {
                        _logger.LogInformation("finish create user username: {username}", context.UserName);
                        try
                        {
                            _logger.LogInformation("start create tenant username: {username}", context.UserName);
                            user.CurrentTenantId = await _tenantService.CreateTenantForLoginOTPAsync(new CreateTenantForLoginOTPDTO()
                            {
                                CurrentTenantId = Guid.Empty,
                                MedicalLicenseNumber = user.MedicalLicenseNumber,
                                FullName = user.Name + " " + user.LastName,
                                PhoneNumber = user.PhoneNumber,
                                UserId = id,
                            }, new CancellationToken());

                            _logger.LogInformation("finish create tenant username: {username} CurrentTenantId:{CurrentTenantId}", context.UserName, user.CurrentTenantId);
                            _logger.LogInformation("start create role username: {username} UserType:{name}", context.UserName, UserType.Healer.Name);

                            if (!Guid.Empty.Equals(invitionId))
                                await CreateUserTenantWithInvitation(invitionId, user);

                            var role = await _roleManager.FindByNameAsync(UserType.Healer.Name);
                            var roleBasic = await _roleManager.FindByNameAsync("Basic");
                            user.UserRoles.Add(new ApplicationUserRole() { UserId = id, TenantId = user.CurrentTenantId, RoleId = role.Id });
                            user.UserRoles.Add(new ApplicationUserRole() { UserId = id, TenantId = user.CurrentTenantId, RoleId = roleBasic.Id });
                            await _userManager.UpdateAsync(user);

                            _logger.LogInformation("finish create role username: {username} CurrentTenantId:{CurrentTenantId}", context.UserName, UserType.Healer.Name);
                            _logger.LogInformation("start create staff username: {username}", context.UserName);

                            var staffdate = await _humanService.AddStaffAsync(new StaffRequestDTO()
                            {
                                Name = user.Name,
                                LastName = user.LastName,
                                NationalCode = user.UserName,
                                PhoneNumber = context.Password,
                                FullName = user.Name + " " + user.LastName,
                                Gender = GenderType.NotSpecified.Id,
                                MedicalLicenseNumber = user.MedicalLicenseNumber,
                                NationalityId = Guid.Parse("5BF70ADB-A059-4BBC-96D3-3BF54F19DB7D"),
                                UserType = UserType.Healer.Id,
                                UserId = id
                            }, new CancellationToken());

                            if (!string.IsNullOrEmpty(staffdate.Message))
                            {
                                _logger.LogInformation("dont create staff username: {username} message:{message}", context.UserName, staffdate.Message);

                                context.Result = new GrantValidationResult(
                                   TokenRequestErrors.InvalidGrant,
                                   staffdate.Message);

                                await _userManager.DeleteAsync(user);
                                if (!Guid.Empty.Equals(user.CurrentTenantId))
                                    await _tenantService.DeleteTenantAsync(user.CurrentTenantId, user.Id, new CancellationToken());

                                return;
                            }


                            _logger.LogInformation("finish create staff username: {username} staffId:{staffdate}", context.UserName, staffdate.Id);

                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("don't create user username: {username} exception:{exception} message => {message}", context.UserName, ex, ex.Message);

                            await _userManager.DeleteAsync(user);
                            if (!Guid.Empty.Equals(user.CurrentTenantId))
                                await _tenantService.DeleteTenantAsync(user.CurrentTenantId, user.Id, new CancellationToken());

                            await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
                            return;
                        }
                    }
                    else
                    {
                        _logger.LogError("don't create user username: {username} message => invalid username", context.UserName);
                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
                        return;
                    }

                }
                else
                {
                    _logger.LogError("don't create user username: {username} message => USER_NOT_FOUND", context.UserName);
                    context.Result = new GrantValidationResult(
                               TokenRequestErrors.InvalidGrant,
                               "USER_NOT_FOUND");
                    return;
                }
            }
            else if (user.UserType == UserType.Patient.Id && context.Password.Length == 11 || (context.Password.Length == 11 && user.PhoneNumber != context.Password && user.PhoneNumberConfirmed == false && user.IrimcConfirmed == true))
            {
                var memberResult = await _irimcService.GetMemberInfoByNationalCodeAsync(new GetMemberByNationalCodeDTO() { NationalCode = context.UserName }, new CancellationToken());
                if (memberResult != null)
                {
                    if (!await _irimcService.CheckValidMemberInMedicalCouncilAsync(new CheckMemberMobileNumberDTO() { NationalCode = context.UserName, MedicalLicenseNumber = memberResult.McCode, PhoneNumber = context.Password }, new CancellationToken()))
                    {
                        _logger.LogError("user username: {username} message => PHONE_NUMBER_IS_NOT_IRIMC", context.UserName);
                        //context.Result = new GrantValidationResult(
                        //  TokenRequestErrors.InvalidGrant,
                        //  "PHONE_NUMBER_IS_NOT_IRIMC");
                        //return;
                        phoneNumberIrimcConfirmed = false;
                    }



                    try
                    {
                        user.MedicalLicenseNumber = memberResult.McCode;
                        user.IrimcConfirmed = true;
                        user.PhoneNumber = context.Password;
                        user.UserType = UserType.Healer.Id;

                        _logger.LogInformation("start create tenant username: {username}", context.UserName);
                        user.CurrentTenantId = await _tenantService.CreateTenantForLoginOTPAsync(new CreateTenantForLoginOTPDTO()
                        {
                            CurrentTenantId = user.CurrentTenantId,
                            MedicalLicenseNumber = user.MedicalLicenseNumber,
                            FullName = user.Name + " " + user.LastName,
                            PhoneNumber = context.Password,
                            UserId = user.Id,
                        }, new CancellationToken());

                        if (!Guid.Empty.Equals(invitionId))
                            await CreateUserTenantWithInvitation(invitionId, user);

                        _logger.LogInformation("finish create role username: {username} CurrentTenantId:{CurrentTenantId}", context.UserName, UserType.Healer.Name);
                        _logger.LogInformation("start create staff username: {username}", context.UserName);


                        var userRoles = await _applicationUserManager.GetRolesAsync(user, new List<Guid>() { user.CurrentTenantId });
                        var role = await _roleManager.FindByNameAsync(UserType.Healer.Name);
                        var roleBasic = await _roleManager.FindByNameAsync("Basic");
                        if (!userRoles.Select(x => x.Name).Contains(role.Name))
                        {
                            user.UserRoles = new List<ApplicationUserRole>();
                            user.UserRoles.Add(new ApplicationUserRole() { UserId = user.Id, TenantId = user.CurrentTenantId, RoleId = role.Id });
                            user.UserRoles.Add(new ApplicationUserRole() { UserId = user.Id, TenantId = user.CurrentTenantId, RoleId = roleBasic.Id });
                        }
                        await _userManager.UpdateAsync(user);

                        _logger.LogInformation("finish create role username: {username} CurrentTenantId:{CurrentTenantId}", context.UserName, UserType.Healer.Name);
                        _logger.LogInformation("start create staff username: {username}", context.UserName);

                        var staffdate = await _humanService.AddStaffAsync(new StaffRequestDTO()
                        {
                            Name = user.Name,
                            LastName = user.LastName,
                            NationalCode = user.UserName,
                            PhoneNumber = context.Password,
                            FullName = user.Name + " " + user.LastName,
                            Gender = GenderType.NotSpecified.Id,
                            MedicalLicenseNumber = user.MedicalLicenseNumber,
                            NationalityId = Guid.Parse("5BF70ADB-A059-4BBC-96D3-3BF54F19DB7D"),
                            UserType = UserType.Healer.Id,
                            UserId = user.Id
                        }, new CancellationToken());

                        _logger.LogInformation("finish create staff username: {username} staffId:{staffdate}", context.UserName, staffdate.Id);

                        if (!string.IsNullOrEmpty(staffdate.Message))
                        {
                            _logger.LogError("don't create user username: {username} message => {message}", context.UserName, staffdate.Message);
                            context.Result = new GrantValidationResult(
                               TokenRequestErrors.InvalidGrant,
                               staffdate.Message);

                            if (!Guid.Empty.Equals(user.CurrentTenantId))
                                await _tenantService.DeleteTenantAsync(user.CurrentTenantId, user.Id, new CancellationToken());

                            return;
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("don't create user username: {username} exception:{exception} message => {message}", context.UserName, ex, ex.Message);

                        if (!Guid.Empty.Equals(user.CurrentTenantId))
                            await _tenantService.DeleteTenantAsync(user.CurrentTenantId, user.Id, new CancellationToken());

                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
                        return;
                    }
                }
                else
                {
                    _logger.LogError("user Exists dont response GetMemberByNational: {username}  message => USER_NOT_FOUND", context.UserName);
                    context.Result = new GrantValidationResult(
                              TokenRequestErrors.InvalidGrant,
                              "USER_NOT_FOUND");
                    return;
                }

            }
            else if ((user.UserType == UserType.Healer.Id || user.UserType == UserType.Healer_SystemManager.Id) && context.Password.Length == 11 && (user.IrimcConfirmed == false || user.RegisterCompleted == false))
            {
                var memberResult = await _irimcService.GetMemberInfoByNationalCodeAsync(new GetMemberByNationalCodeDTO() { NationalCode = context.UserName }, new CancellationToken());
                if (memberResult != null)
                {
                    if (!await _irimcService.CheckValidMemberInMedicalCouncilAsync(new CheckMemberMobileNumberDTO() { NationalCode = context.UserName, MedicalLicenseNumber = memberResult.McCode, PhoneNumber = context.Password }, new CancellationToken()))
                    {
                        _logger.LogError("user username: {username} message => PHONE_NUMBER_IS_NOT_IRIMC", context.UserName);
                        //context.Result = new GrantValidationResult(
                        //  TokenRequestErrors.InvalidGrant,
                        //  "PHONE_NUMBER_IS_NOT_IRIMC");
                        //return;
                        phoneNumberIrimcConfirmed = false;
                    }

                    try
                    {
                        user.MedicalLicenseNumber = memberResult.McCode;
                        user.IrimcConfirmed = true;
                        user.PhoneNumber = context.Password;
                        user.UserType = UserType.Healer.Id;

                        _logger.LogInformation("start create tenant username: {username}", context.UserName);
                        user.CurrentTenantId = await _tenantService.CreateTenantForLoginOTPAsync(new CreateTenantForLoginOTPDTO()
                        {
                            CurrentTenantId = user.CurrentTenantId,
                            MedicalLicenseNumber = user.MedicalLicenseNumber,
                            FullName = user.Name + " " + user.LastName,
                            PhoneNumber = context.Password,
                            UserId = user.Id,
                        }, new CancellationToken());

                        _logger.LogInformation("finish create role username: {username} CurrentTenantId:{CurrentTenantId}", context.UserName, UserType.Healer.Name);
                        _logger.LogInformation("start create staff username: {username}", context.UserName);

                        if (!Guid.Empty.Equals(invitionId))
                            await CreateUserTenantWithInvitation(invitionId, user);


                        var userRoles = await _applicationUserManager.GetRolesAsync(user, new List<Guid>() { user.CurrentTenantId });
                        var role = await _roleManager.FindByNameAsync(UserType.From(user.UserType).Name);
                        var roleBasic = await _roleManager.FindByNameAsync("Basic");
                        if (!userRoles.Select(x => x.Name).Contains(role.Name))
                        {
                            user.UserRoles = new List<ApplicationUserRole>();
                            user.UserRoles.Add(new ApplicationUserRole() { UserId = user.Id, TenantId = user.CurrentTenantId, RoleId = role.Id });
                            user.UserRoles.Add(new ApplicationUserRole() { UserId = user.Id, TenantId = user.CurrentTenantId, RoleId = roleBasic.Id });
                        }
                        await _userManager.UpdateAsync(user);

                        _logger.LogInformation("finish create role username: {username} CurrentTenantId:{CurrentTenantId}", context.UserName, UserType.Healer.Name);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("don't create user username: {username} exception:{exception} message => {message}", context.UserName, ex, ex.Message);

                        if (!Guid.Empty.Equals(user.CurrentTenantId))
                            await _tenantService.DeleteTenantAsync(user.CurrentTenantId, user.Id, new CancellationToken());

                        await _events.RaiseAsync(new UserLoginFailureEvent(context.UserName, "invalid username", interactive: false));
                        return;
                    }
                }
                else
                {
                    _logger.LogError("user Exists dont response GetMemberByNational: {username}  message => CURRENT_TENANT_IS_EMPTY", context.UserName);
                    context.Result = new GrantValidationResult(
                              TokenRequestErrors.InvalidGrant,
                              "CURRENT_TENANT_IS_EMPTY");
                    return;
                }
            }
            if (context.Password.Length == 11)
            {
                var sendTokenEvent = new SendChangePhoneNumberTokenIntegrationEvent(user.Id, context.Password);
                await _identityIntegrationEventService.AddAndSaveEventAsync(sendTokenEvent);
                await _identityIntegrationEventService.PublishThroughEventBusAsync(sendTokenEvent);
                _logger.LogInformation("send otp for username: {username}  message => TOKEN_SENDED", context.Password);
                context.Result = new GrantValidationResult(
                         TokenRequestErrors.InvalidGrant,
                         "TOKEN_SENDED");
                return;
            }
            else
            {
                if (await _userManager.VerifyChangePhoneNumberTokenAsync(user, context.Password, user.PhoneNumber))
                {
                    await SetCurrentTenant(context, user);

                    if (!string.IsNullOrEmpty(context.Result.ErrorDescription))
                        return;

                    if (!Guid.Empty.Equals(user.CurrentTenantId))
                    {
                        user.RegisterCompleted = true;
                        user.PhoneNumberConfirmed = true;
                        await _userManager.UpdateAsync(user);
                    }

                    if (user.PhoneNumberConfirmed == false)
                    {
                        _logger.LogError("user Exists : {username}  message => PHONE_NUMBER_NOT_CONFIRMED", context.UserName);
                        context.Result = new GrantValidationResult(
                           TokenRequestErrors.InvalidGrant,
                           "PHONE_NUMBER_NOT_CONFIRMED");
                        return;
                    }

                    if (user.RegisterCompleted == true && user.IrimcConfirmed == false && (user.UserType == UserType.Healer.Id || user.UserType == UserType.Healer_SystemManager.Id))
                    {
                        _logger.LogError("user Exists : {username}  message => IRIMC_NOT_CONFIRMED", context.UserName);
                        context.Result = new GrantValidationResult(
                           TokenRequestErrors.InvalidGrant,
                           "IRIMC_NOT_CONFIRMED");
                        return;
                    }

                    if (user.RegisterCompleted == true && user.UserType != UserType.Admin.Id && Guid.Empty.Equals(user.CurrentTenantId))
                    {
                        _logger.LogError("user Exists : {username}  message => CURRENT_TENANT_IS_EMPTY : {CurrentTenantId}", context.UserName, user.CurrentTenantId);
                        context.Result = new GrantValidationResult(
                           TokenRequestErrors.InvalidGrant,
                           "CURRENT_TENANT_IS_EMPTY");
                        return;
                    }



                    _logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, user.Id.ToString(), context.UserName, interactive: false));

                    context.Result = new GrantValidationResult(user.Id.ToString(), AuthenticationMethods.Password);
                    return;
                }
                else
                {
                    _logger.LogError("user Exists : {username}  message => TOKEN_IS_INVALID", context.UserName);
                    context.Result = new GrantValidationResult(
                              TokenRequestErrors.InvalidGrant,
                              "TOKEN_IS_INVALID");
                    return;
                }

            }

        }

        private async Task LoginPharmacy(ResourceOwnerPasswordValidationContext context, ApplicationUser user)
        {
            _logger.LogInformation("Authentication LoginWithNationalCode for username: {username}  password:{password}", context.UserName, context.Password);

            if (!ValidationHelper.IsValidNationalCode(context.UserName))
            {
                context.Result = new GrantValidationResult(
                              TokenRequestErrors.InvalidGrant,
                              "NATIONAL_CODE_IS_INVALID");
                _logger.LogError("don't create user username: {username} message => NATIONAL_CODE_IS_INVALID", context.UserName);
                return;
            }

            user = await _userManager.FindByNameAsync(context.UserName);
            var roles = await _applicationUserManager.GetRolesAsync(user, new List<Guid>() { user.CurrentTenantId });

            if (user == null)
            {

                _logger.LogError("don't create user username: {username} message => USER_NOT_FOUND", context.UserName);
                context.Result = new GrantValidationResult(
                           TokenRequestErrors.InvalidGrant,
                           "USER_NOT_FOUND");
                return;
            }
            if (!roles.Any(x => x.Name.Equals("Pharmacy")))
            {

                _logger.LogError("don't user in role username: {username} message => ROLE_NOT_FOUND", context.UserName);
                context.Result = new GrantValidationResult(
                           TokenRequestErrors.InvalidGrant,
                           "ROLE_NOT_FOUND");
                return;
            }
            if (context.Password.Length == 11)
            {
                var sendTokenEvent = new SendChangePhoneNumberTokenIntegrationEvent(user.Id, context.Password);
                await _identityIntegrationEventService.AddAndSaveEventAsync(sendTokenEvent);
                await _identityIntegrationEventService.PublishThroughEventBusAsync(sendTokenEvent);
                _logger.LogInformation("send otp for username: {username}  message => TOKEN_SENDED", context.Password);
                context.Result = new GrantValidationResult(
                         TokenRequestErrors.InvalidGrant,
                         "TOKEN_SENDED");
                return;
            }
            else
            {
                if (await _userManager.VerifyChangePhoneNumberTokenAsync(user, context.Password, user.PhoneNumber))
                {
                    if (!Guid.Empty.Equals(user.CurrentTenantId))
                    {
                        user.RegisterCompleted = true;
                        user.PhoneNumberConfirmed = true;
                        await _userManager.UpdateAsync(user);
                    }

                    if (user.PhoneNumberConfirmed == false)
                    {
                        _logger.LogError("user Exists : {username}  message => PHONE_NUMBER_NOT_CONFIRMED", context.UserName);
                        context.Result = new GrantValidationResult(
                           TokenRequestErrors.InvalidGrant,
                           "PHONE_NUMBER_NOT_CONFIRMED");
                        return;
                    }


                    if (user.RegisterCompleted == true && user.UserType != UserType.Admin.Id && Guid.Empty.Equals(user.CurrentTenantId))
                    {
                        _logger.LogError("user Exists : {username}  message => CURRENT_TENANT_IS_EMPTY : {CurrentTenantId}", context.UserName, user.CurrentTenantId);
                        context.Result = new GrantValidationResult(
                           TokenRequestErrors.InvalidGrant,
                           "CURRENT_TENANT_IS_EMPTY");
                        return;
                    }



                    _logger.LogInformation("Credentials validated for username: {username}", context.UserName);
                    await _events.RaiseAsync(new UserLoginSuccessEvent(context.UserName, user.Id.ToString(), context.UserName, interactive: false));

                    context.Result = new GrantValidationResult(user.Id.ToString(), AuthenticationMethods.Password);
                    return;
                }
                else
                {
                    _logger.LogError("user Exists : {username}  message => TOKEN_IS_INVALID", context.UserName);
                    context.Result = new GrantValidationResult(
                              TokenRequestErrors.InvalidGrant,
                              "TOKEN_IS_INVALID");
                    return;
                }

            }

        }


        private async Task SetCurrentTenant(ResourceOwnerPasswordValidationContext context, ApplicationUser user)
        {
            var tenants = await _tenantService.GetAllTenantIncludeDeletedByUserIdAsync(user.Id, new CancellationToken());
            TenantDTO activeTenant = null;
            if (user.CurrentTenantId != null || !Guid.Empty.Equals(user.CurrentTenantId))
                activeTenant = tenants.FirstOrDefault(x => x.UserTenant.Deleted != true && x.UserTenant.IsActive == true && x.UserTenant.IsConfirm == true && x.UserTenant.CooperationRequestTypeId == 2 && x.Id == user.CurrentTenantId);
            if (activeTenant == null)
                activeTenant = tenants.FirstOrDefault(x => x.UserTenant.Deleted != true && x.UserTenant.IsActive == true && x.UserTenant.IsConfirm == true && x.UserTenant.CooperationRequestTypeId == 2);
            if (activeTenant == null)
            {
                activeTenant = tenants.FirstOrDefault(x => x.UserTenant.Deleted != true && x.UserTenant.IsActive == true && x.UserTenant.IsConfirm == true && x.UserTenant.CooperationRequestTypeId == 1);
                if (activeTenant != null)
                    user.IsPendingCurrentTenant = true;
                else
                    user.IsPendingCurrentTenant = false;
            }
            if (activeTenant == null)
            {
                activeTenant = tenants.OrderByDescending(x => x.UserTenant.Deleted != true).FirstOrDefault();
                if (activeTenant == null)
                {
                    context.Result = new GrantValidationResult(
                       TokenRequestErrors.InvalidGrant,
                       "CURRENT_TENANT_IS_EMPTY");
                    return;
                }
                //مدیر درخواست را رد کرده است
                else if (activeTenant.UserTenant.IsActive == true && activeTenant.UserTenant.IsConfirm == false && activeTenant.UserTenant.CooperationRequestTypeId == 3)
                {
                    context.Result = new GrantValidationResult(
                       TokenRequestErrors.InvalidGrant,
                       "CURRENT_TENANT_IS_CONFIRM_FALSE");
                    return;
                }
                //مدیر کاربر را معلق کرده است
                else if (activeTenant.UserTenant.IsActive == false && activeTenant.UserTenant.IsConfirm == true && activeTenant.UserTenant.CooperationRequestTypeId == 2)
                {
                    context.Result = new GrantValidationResult(
                       TokenRequestErrors.InvalidGrant,
                       "CURRENT_TENANT_IS_ACTIVE_FALSE");
                    return;
                }
                else
                {
                    context.Result = new GrantValidationResult(
                       TokenRequestErrors.InvalidGrant,
                       "CURRENT_TENANT_IS_EMPTY");
                    return;
                }

            }
            user.CurrentTenantId = activeTenant.Id;
            user.UserType = activeTenant.UserTenant.UserTypeId;
            await _userManager.UpdateAsync(user);




        }


        private async Task CreateUserTenantWithInvitation(Guid invitationId, ApplicationUser user)
        {
            var invitation = await _tenantService.GetInvitationByIdAsync(invitationId, new CancellationToken());
            if (invitation != null)
            {
                UserTenantDTO userTenant = new UserTenantDTO()
                {
                    UserId = user.Id,
                    UserTypeId = invitation.UserTypeId,
                    CooperationRequestTypeId = 1,
                    IsActive = true,
                    IsConfirm = true,
                    TenantId = invitation.TenantId,
                };
                var result = await _tenantService.AddUserTenantAsync(userTenant, new CancellationToken());
            }
        }
    }


}

