using AutoMapper;
using JabilScreeningTest.Entities;
using JabilScreeningTest.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JabilScreeningTest.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;

        public AccountsController(UserManager<AppUser> userManager, ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            //_mapper = mapper;
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("cretaeNewUser")]
        public async Task<IActionResult> Post([FromBody] RegistrationModel model)
        {
            try
            {
                var userIdentity = new AppUser() { UserName = Guid.NewGuid().ToString(), Email = model.Email };
                var result = await _userManager.CreateAsync(userIdentity, model.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                };
                return Ok("User created");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}