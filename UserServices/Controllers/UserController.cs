using Microsoft.AspNetCore.Mvc;
using User.Application.Users;
using User.Domain.Users;
using UserServices.Users;

namespace UserServices.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> logger;
        private readonly IUserService userService;

        public UserController(ILogger<UserController> logger,
            IUserService userService)
        {
            this.logger = logger;
            this.userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserRequest request)
        {
            Guid id;
            try
            {
                id = await userService.Create(
                    request.Email == null ? null : new Email(request.Email),
                    request.PhoneNumber == null ? null : new PhoneNumber(request.PhoneNumber),
                    request.Password
                );
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(ex.Message);
            }

            return Ok(id);
        }

        [HttpGet("emailorphone")]
        public async Task<IActionResult> GetUserByEmailOrPhoneNumber(string emailOrPhoneNumber)
        {
            var user = await userService.GetUserByEmailOrPhoneNumber(emailOrPhoneNumber);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                Id = user.Id,
                Email = user.Email.Address,
                PhoneNumber = user.PhoneNumber.ToString()
            });
        }
    }
}