using Application.Modules.StudentsModule.Commands.StudentLoginCommand;
using Application.Modules.StudentsModule.Commands.StudentRegisterCommand;
using Infrastructure.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ApiAuthController : ControllerBase
    {
        private readonly IMediator mediator;

        public ApiAuthController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] StudentRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var result = await mediator.Send(request);

                return Created(string.Empty, new
                {
                    message = "Student registered successfully.",
                    studentNumber = result.StudentNumber,
                    defaultPassword = result.DefaultPassword
                });
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (ConflictException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] StudentLoginRequest request)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);

            try
            {
                var result = await mediator.Send(request);
                return Ok(new
                {
                    message = "Login successful",
                    fullName = result.FullName,
                    studentNumber = result.StudentNumber
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }
    }
}
