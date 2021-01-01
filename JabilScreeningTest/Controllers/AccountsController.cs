using AutoMapper;
using JabilScreeningTest.Entities;
using JabilScreeningTest.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace JabilScreeningTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly JsonSerializerSettings _serializerSettings;
        public AccountsController(UserManager<AppUser> userManager, ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("registerUser")]
        public async Task<IActionResult> RegisterUser([FromBody] RegistrationModel model)
        {
            try
            {
                var userIdentity = new AppUser() { UserName = model.UserName, Email = model.Email };
                var result = await _userManager.CreateAsync(userIdentity, model.Password);
                if (!result.Succeeded)
                {
                    var description = "";
                    foreach (var item in result.Errors)
                    {
                        description += description == "" ? item.Description : item.Description + "\n";
                    }
                    var error = new
                    {
                        message = description
                    };
                    var errroJson = JsonConvert.SerializeObject(error, _serializerSettings);
                    return new BadRequestObjectResult(errroJson);
                };
                var json = JsonConvert.SerializeObject("User registered successfully.");
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}