using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Identity.API.Application.Commands;
using Identity.Domain.AggregatesModel.UserAggregate;
using Identity.API.Application.Queries;
using System.Collections.Generic;
using Identity.API.Application.DTO;
using static IdentityServer4.IdentityServerConstants;
using Identity.API.Application.IntegrationEvents;
using Identity.API.Application.IntegrationEvents.Events;

namespace Identity.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [Authorize(LocalApi.PolicyName)]
    [ApiController]
#if DEBUG
    [ApiExplorerSettings(IgnoreApi = false)]
#else
    [ApiExplorerSettings(IgnoreApi = true)]
#endif
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IIdentityService _identityService;
        private readonly IIdentityIntegrationEventService _identityIntegrationEventService;
        private readonly IApplicationUserManager _userManager;

        public UserController(
            IMediator mediator,
            IIdentityService identityService,
            IIdentityIntegrationEventService identityIntegrationEventService,
            IApplicationUserManager userManager
            )
        {
            _mediator = MethodParameterChecker.CheckUp(mediator);
            _identityService = MethodParameterChecker.CheckUp(identityService);
            _identityIntegrationEventService = MethodParameterChecker.CheckUp(identityIntegrationEventService);
            _userManager = MethodParameterChecker.CheckUp(userManager);
        }

        [Route("Add")]
        [HttpPost]
        public async Task<ActionResult<UserPatientDTO>> CreateAsync([FromBody] CreateUserCommand createUserCommand)
        {
            var result = await _mediator.Send(createUserCommand);
            return result;
        }

        [Route("Update")]
        [HttpPost]
        public async Task<ActionResult<UserPatientDTO>> UpdateAsync([FromBody] UpdateUserCommand updateUserCommand)
        {
            var reslut = await _mediator.Send(updateUserCommand);

            var sendTokenEvent = new SendChangePhoneNumberTokenIntegrationEvent(updateUserCommand.ObjectDTO.Id.Value, updateUserCommand.ObjectDTO.PhoneNumber);
            await _identityIntegrationEventService.AddAndSaveEventAsync(sendTokenEvent);
            await _identityIntegrationEventService.PublishThroughEventBusAsync(sendTokenEvent);

            return reslut;
        }



        [Route("AddOrUpdate")]
        [HttpPost]
        //[AllowAnonymous]
        public async Task<ActionResult<UserPatientDTO>> AddOrUpdateAsync([FromBody] AddOrUpdateUserCommand addOrUpdateUserCommand)
        {
            var result = await _mediator.Send(addOrUpdateUserCommand);
            return result;
        }


        [Route("UpdateProfile")]
        [HttpPost]
        public async Task<ActionResult<UserPatientDTO>> UpdateProfileAsync([FromBody] UpdateProfileCommand updateProfileCommand)
        {
            return await _mediator.Send(updateProfileCommand);
        }


        [Route("Delete")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<UserDeleteDTO>> DeleteAsync([FromBody] DeleteUserCommand deleteUserCommand)
        {
            return await _mediator.Send(deleteUserCommand);
        }

        [Route("DeleteLogical")]
        [HttpPost]
        public async Task<ActionResult<UserDeleteLogicalDTO>> DeleteLogicalAsync([FromBody] DeleteLogicalUserCommand deleteLogicalUserCommand)
        {
            return await _mediator.Send(deleteLogicalUserCommand);
        }

        [Route("SetCurrentTenant")]
        [HttpPost]
        public async Task<ActionResult<bool>> SetCurrentTenantAsync([FromBody] SetCurrentTenantUserCommand setCurrentTenantUserCommand)
        {
            return await _mediator.Send(setCurrentTenantUserCommand);
        }

        [Route("GetCurrentUser")]
        [HttpGet]
        public async Task<ActionResult> GetCurrentUser()
        {
            var result = await _mediator.Send(new GetUserByIdQuery { userId = _identityService.GetUserId().Value });
            return new JsonResult(result);
        }

        [Route("GetById/{userId}")]
        [HttpGet]
        public async Task<ActionResult> GetByIdAsync(string userId)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { userId = Guid.Parse(userId) });
            return new JsonResult(result);
        }

        [Route("GetByPhoneNumbers")]
        [HttpPost]
        public async Task<ActionResult<List<UserPatientDTO>>> GetByPhoneNumbersAsync([FromBody] List<string> phoneNumbers)
        {
            var result = await _mediator.Send(new GetUserByPhoneNumbersQuery { PhoneNumbers = phoneNumbers });
            return new JsonResult(result);
        }

        [Route("GetByListId")]
        [HttpPost]
        public async Task<ActionResult<List<UserPatientDTO>>> GetByListIdAsync([FromBody] List<string> listUserId)
        {
            var result = await _mediator.Send(new GetUserByListIdQuery { listUserId = listUserId });
            return new JsonResult(result);
        }


        [Route("GetAll")]
        [HttpGet]
        public async Task<ActionResult> GetAllAsync()
        {
            var result = await _mediator.Send(new GetAllUserQuery());
            return new JsonResult(result);
        }

        [AllowAnonymous]
        [Route("GetAllPaging")]
        [HttpGet]
        public async Task<ActionResult> GetAllPagingAsync([DataSourceRequest] DataSourceRequest request)
        {
            var result = await _mediator.Send(new GetAllUserPagingQuery()
            {
                Aggregates = request.Aggregates,
                Filters = request.Filters,
                Groups = request.Groups,
                Skip = request.Skip,
                Take = request.Take,
                Sorts = request.Sorts
            });
            return new JsonResult(result);
        }


        [Route("AddUserRoles")]
        [HttpPost]
        public async Task<ActionResult<bool>> CreateUserRolesAsync([FromBody] CreateUserRoleCommand createUserRoleCommand)
        {

            var result = await _mediator.Send(createUserRoleCommand);

            return result;
        }


        [Route("AddUserRolesSystemic")]
        [HttpPost]
        public async Task<ActionResult<bool>> CreateUserRolesSystemicAsync([FromBody] CreateUserRoleSystemicCommand createUserRoleSystemicCommand)
        {

            var result = await _mediator.Send(createUserRoleSystemicCommand);

            return result;
        }



        [Route("GetUserRoles")]
        [HttpGet]
        public async Task<ActionResult<List<UserRoleDTO>>> GetUserRolesAsync([FromQuery] GetUserRoleQuery getUserRoleQuery)
        {
            var result = await _mediator.Send(getUserRoleQuery);
            return new JsonResult(result);
        }


        [Route("GetGenderType")]
        [HttpGet]
        public async Task<ActionResult> GetGenderTypeAsync()
        {
            var result = GenderType.List();
            return new JsonResult(result);
        }

        [Route("GetUserType")]
        [HttpGet]
        public async Task<ActionResult> GetUserTypeAsync()
        {
            var result = UserType.List();
            return new JsonResult(result);
        }

        [Route("GetRegisterUserTypes")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> GetRegisterUserTypesAsync()
        {
            var result = UserType.RegisterList();
            return new JsonResult(result);
        }

        //[Route("GetGenderType")]
        //[HttpGet]
        //public async Task<ActionResult> GetGenderTypeAsync()
        //{
        //    var result = GenderType.List();
        //    return new JsonResult(result);
        //}

        [Route("GetExistByNationalCode/{nationalCode}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> GetExistByNationalCode(string nationalCode)
        {
            var result = await _mediator.Send(new GetUserExistByNationalCodeQuery(nationalCode));
            return new JsonResult(result);
        }


        [Route("GetCurrentUserTypeByUserId/{id}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<int>> GetCurrentUserTypeByUserId(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery { userId = id });
            return new JsonResult(result.UserType);
        }

        [Route("GetByNationalCode/{nationalCode}")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<UserPatientDTO>> GetByNationalCode(string nationalCode)
        {
            var result = await _mediator.Send(new GetByNationalCodeQuery(nationalCode));
            return new JsonResult(result);
        }

        [Route("GetByNationalCodes")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<List<UserPatientDTO>>> GetByNationalCode([FromBody] List<string> nationalCodes)
        {
            var result = await _mediator.Send(new GetByNationalCodesQuery(nationalCodes));
            return new JsonResult(result);
        }


        [HttpGet("GetAllRegisteries")]
        public virtual async Task<ActionResult<List<RegistryDTO>>> GetAllRegisteries()
        {
            var result = await _mediator.Send(new GetAllRegisteriesQuery());
            return result;
        }

        [HttpPost("CompleteRegister")]
        public virtual async Task<ActionResult<bool>> CompleteRegister()
        {
            var result = await _mediator.Send(new CompleteRegisterCommand());
            return result;
        }

        [HttpPost("SetPassword")]
        public virtual async Task<ActionResult<bool>> SetPasswordAsync([FromBody] SetPasswordCommand setPasswordCommand)
        {
            var result = await _mediator.Send(setPasswordCommand);
            return result;
        }

        [HttpPost("VerifyPhoneNumber2Token")]
        public virtual async Task<ActionResult<bool>> VerifyPhoneNumber2Token([FromBody] VerifyChangePhoneNumber2TokenCommand verifyChangePhoneNumber2TokenCommand)
        {
            var result = await _mediator.Send(verifyChangePhoneNumber2TokenCommand);
            return result;
        }


        [Route("AddOrEditUserPatient")]
        [HttpPost]
        public async Task<ActionResult<UserPatientDTO>> CreateOrUpdateUserPatientAsync([FromBody] CreateOrUpdateUserPatientCommand createUserPatientCommand)
        {
            var result = await _mediator.Send(createUserPatientCommand);

            CreateOrUpdatePatientIntegrationEvent createOrUpdatePatientIntegrationEvent = new CreateOrUpdatePatientIntegrationEvent(
                      createUserPatientCommand.ObjectDTO.PhoneNumber,
                      createUserPatientCommand.ObjectDTO.Name,
                      createUserPatientCommand.ObjectDTO.LastName,
                      createUserPatientCommand.ObjectDTO.NationalCode,
                      createUserPatientCommand.ObjectDTO.Gender,
                      createUserPatientCommand.ObjectDTO.Birthdate,
                      result.Id.Value,
                      _identityService.GetUserId().Value,
                      createUserPatientCommand.ObjectDTO.NationalityId,
                      createUserPatientCommand.ObjectDTO.FatherName,
                      createUserPatientCommand.ObjectDTO.Email,
                      createUserPatientCommand.ObjectDTO.MaritalType,
                      createUserPatientCommand.ObjectDTO.BirthStateId,
                      createUserPatientCommand.ObjectDTO.BirthCityId,
                      createUserPatientCommand.ObjectDTO.Tel,
                      createUserPatientCommand.ObjectDTO.StateId,
                      createUserPatientCommand.ObjectDTO.CityId,
                      createUserPatientCommand.ObjectDTO.Address,
                      createUserPatientCommand.ObjectDTO.ZipCode,
                      createUserPatientCommand.ObjectDTO.BloodTypeId,
                      createUserPatientCommand.ObjectDTO.SpouseBloodTypeId,
                      createUserPatientCommand.ObjectDTO.EducationId,
                      createUserPatientCommand.ObjectDTO.JobId
                      );

            await _identityIntegrationEventService.AddAndSaveEventAsync(createOrUpdatePatientIntegrationEvent);
            await _identityIntegrationEventService.PublishThroughEventBusAsync(createOrUpdatePatientIntegrationEvent);

            return result;
        }

        [Route("AddOrEditDeviceInfo")]
        [HttpPost]
        public async Task<ActionResult<bool>> CreateOrUpdateDeviceInfoAsync([FromBody] CreateOrUpdateDeviceInfoCommand createOrUpdateDeviceInfoCommand)
        {
            return await _mediator.Send(createOrUpdateDeviceInfoCommand);
        }


    }
}
