using JabilScreeningTest.Entities;
using JabilScreeningTest.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JabilScreeningTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JsonSerializerSettings _serializerSettings;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(UserManager<AppUser> userManager, IJwtFactory jwtFactory, IOptions<JwtIssuerOptions> jwtOptions)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;

            _serializerSettings = new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            };
        }

        /// <summary>
        /// POST api/auth/login
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Post([FromBody] CredentialModel credentials)
        {
            try
            {
                var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
                if (identity == null)
                {
                    return BadRequest("Invalid username or password.");
                }

                // Serialize and return the response
                var response = new
                {
                    id = identity.Claims.Single(c => c.Type == "id").Value,
                    auth_token = await _jwtFactory.GenerateEncodedToken(credentials.UserName, identity),
                    expires_in = (int)_jwtOptions.ValidFor.TotalSeconds
                };

                var json = JsonConvert.SerializeObject(response, _serializerSettings);
                return new OkObjectResult(json);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Claims Identity
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
                // get the user to verifty
                var userToVerify = await _userManager.FindByNameAsync(userName);

                if (userToVerify != null)
                {
                    // check the credentials  
                    if (await _userManager.CheckPasswordAsync(userToVerify, password))
                    {
                        return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
                    }
                }
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }
    }
}