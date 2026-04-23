using Dalleni.API.Bases;
using Dalleni.Application.DTOs.Requests.Auth;
using Dalleni.Application.DTOs.Responses.Auth;
using Dalleni.Application.Features.Authantications.ChangePassword;
using Dalleni.Application.Features.Authantications.ConfirmEmail;
using Dalleni.Application.Features.Authantications.ConfirmOTPForResetPassword;
using Dalleni.Application.Features.Authantications.GooleSignin;
using Dalleni.Application.Features.Authantications.Login;
using Dalleni.Application.Features.Authantications.Logout;
using Dalleni.Application.Features.Authantications.RefreshToken;
using Dalleni.Application.Features.Authantications.ResendEmailConfirmationCode;
using Dalleni.Application.Features.Authantications.ResendOTPForResetPassword;
using Dalleni.Application.Features.Authantications.ResetPasssword;
using Dalleni.Application.Features.Authantications.SendResetPasswordCode;
using Dalleni.Application.Features.Authantications.SignUp;
using Dalleni.Domin.Helpers;
using Dalleni.Domin.ResponsePattern;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dalleni.API.Controllers
{
    [ApiVersion("1.0")]
    public class AuthenticationController : BaseController
    {

        public AuthenticationController(IMediator mediator) : base(mediator)
        {
        }

        #region Auth Endpoints

        /// <summary>
        /// Register a new client user.
        /// </summary>
        [HttpPost(APIROUTES.Authentication.SignUp)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SignUpAsync([FromForm] SignUpRequest dto)
        {
            var result = await _mediator.Send(new SignUpCommand(dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// Login and retrieve access + refresh tokens.
        /// </summary>
        [HttpPost(APIROUTES.Authentication.Login)]
        [ProducesResponseType(typeof(Response<TokenReponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto dto)
        {
            var result = await _mediator.Send(new LoginCommand(dto));
            return FinalResponse(result);
        }
        /// <summary>
        ///  LogOut User
        /// </summary>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(APIROUTES.Authentication.Logout)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> LogoutAsync()
        {
            // Get current user ID from JWT
            var userId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var result = await _mediator.Send(new LogoutCommand(userId));
            return FinalResponse(result);

        }
        /// <summary>
        /// Refresh access token using refresh token.
        /// </summary>
        [HttpPost(APIROUTES.Authentication.RefreshToken)]
        [ProducesResponseType(typeof(Response<TokenReponseDto>), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RefreshTokenAsync([FromBody] TokenRequestDto dto)
        {
            var result = await _mediator.Send(new RefreshTokenAsyncCommand(dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// Change password for logged-in user.
        /// </summary>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost(APIROUTES.Authentication.ChangePassword)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ChangePasswordAsync([FromBody] ChangePasswordRequestDto dto)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var result = await _mediator.Send(new ChangePasswordCommand(userId, dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// Send reset password code to user email.
        /// </summary>
        [HttpPost(APIROUTES.Authentication.SendResetCode)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendResetPasswordCodeAsync([FromBody] ForgetPasswordRequestDto dto)
        {
            var result = await _mediator.Send(new SendResetPasswordCodeCommand(dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// ReSend reset password code to user email.
        /// </summary>
        [HttpPost(APIROUTES.Authentication.ResendResetCode)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReSendResetPasswordCodeAsync([FromBody] ForgetPasswordRequestDto dto)
        {
            var result = await _mediator.Send(new ReSendOTPForResetPasswordCommand(dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// Confirm the reset password code before setting new password.
        /// </summary>
        [HttpPost(APIROUTES.Authentication.ConfirmResetPasswordCode)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmResetPasswordCodeAsync([FromBody] ConfirmResetPasswordCodeRequest dto)
        {
            var result = await _mediator.Send(new ConfirmResetPasswordOTPCommand(dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// Reset user password.
        /// </summary>
        [HttpPost(APIROUTES.Authentication.ResetPassword)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] SetNewPasswordRequestDto dto)
        {
            var result = await _mediator.Send(new ResetPasswordCommand(dto));
            return FinalResponse(result);
        }
        /// <summary>
        /// Confirm user email.
        /// </summary>
        [HttpGet(APIROUTES.Authentication.ConfirmEmail)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery] ConfirmEmailRequest dto)
        {
            var result = await _mediator.Send(new ConfirmEmailCommand(dto));
            return FinalResponse(result);
        }

        /// <summary>
        /// Re Send Confirmation user email.
        /// </summary>
        [HttpGet(APIROUTES.Authentication.ResendConfirmationEmail)]
        [ProducesResponseType(typeof(Response<bool>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReSendConfirmationEmailAsync([FromQuery] ReSendConfirmationEmailRequest dto)
        {
            var result = await _mediator.Send(new ResendConfirmaionEmailCommand(dto));
            return FinalResponse(result);
        }
        [HttpGet(APIROUTES.Authentication.googleSignup)]
        public async Task<IActionResult> GoogleSignin()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (!result.Succeeded)
                return BadRequest("Google authentication failed");
            var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
            var Surname = result.Principal.FindFirst(ClaimTypes.Surname)?.Value;
            var Username = result.Principal.FindFirst(ClaimTypes.Name)?.Value;
            var picture = result.Principal.FindFirst("picture")?.Value;
            var nameIdentifier = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _mediator.Send(new GooleSigninCommand(email, Surname, Username, picture, nameIdentifier));

            return FinalResponse(response);
        }
        [HttpGet(APIROUTES.Authentication.GoogleLogin)]
        public async Task<IActionResult> LoginWithGoogle()
        {
            var redirectUrl = Url.Action("GoogleSignin", "Authentication", null, Request.Scheme);
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };

            var result = Challenge(properties, GoogleDefaults.AuthenticationScheme);
            Console.WriteLine(result.ToString());
            foreach (var item in result.Properties.Items)
            {
                Console.WriteLine(item.ToString());
            }
            return result;
        }
        #endregion
    }
}
