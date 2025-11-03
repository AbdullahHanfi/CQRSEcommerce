using Application.DTOs.Auth;
using Application.Features.Auth.Command.Login;
using Application.Features.Auth.Command.RefreshToken;
using Application.Features.Auth.Command.Register;
using Application.Request.Auth;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<ValidationErrorResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var result = await Sender.Send(new LoginCommand(request.Email, request.Password));
                if (result.IsSuccess)
                    return Ok(result.Value);
                else
                {
                    var tuple = new ValidationErrorResponse("Exception", [result.Error.Message]);
                    return BadRequest(tuple);
                }
            }
            catch (ValidationException ex)
            {

                var tuples = ex.Errors
                    .Select(err => new ValidationErrorResponse(err.Key, err.Value))
                    .ToArray();

                return BadRequest(tuples);
            }
            catch (Exception ex)
            {
                var tuple = new ValidationErrorResponse("Exception", [ex.Message]);
                return BadRequest(tuple);
            }
        }
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<ValidationErrorResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                var result = await Sender.Send(new RegisterCommand(request.Email, request.Password, request.ConfirmPassword, request.UserName));
                if (result.IsSuccess)
                    return Ok(result.Value);
                else
                {
                    var tuple = new ValidationErrorResponse("Exception", [result.Error.Message]);

                    return BadRequest(tuple);
                }
            }
            catch (ValidationException ex)
            {

                var tuples = ex.Errors
                    .Select(err => new ValidationErrorResponse(err.Key, err.Value))
                    .ToArray();

                return BadRequest(tuples);
            }
            catch (Exception ex)
            {
                var tuple = new ValidationErrorResponse("Exception", [ex.Message]);
                return BadRequest(tuple);
            }
        }

        [HttpPost("refresh")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AuthDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<ValidationErrorResponse>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] string token)
        {
            try
            {
                var result = await Sender.Send(new RefreshTokenCommand(token));
                if (result.IsSuccess)
                    return Ok(result.Value);
                else
                {
                    var tuple = new ValidationErrorResponse("Exception", [result.Error.Message]);

                    return BadRequest(tuple);
                }
            }
            catch (ValidationException ex)
            {

                var tuples = ex.Errors
                    .Select(err => new ValidationErrorResponse(err.Key, err.Value))
                    .ToArray();

                return BadRequest(tuples);
            }
            catch (Exception ex)
            {
                var tuple = new ValidationErrorResponse("Exception", [ex.Message]);
                return BadRequest(tuple);
            }
        }

    }
}
