using BE.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace BE.Controllers
{
    /* 
        Test Controller
     */
    [ApiController]
    [Route("api/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtHelper jwtHelper;
        private readonly EncryptionHelper encryptionHelper;

        public AuthController(JwtHelper jwtHelper, EncryptionHelper encryptionHelper)
        {
            this.jwtHelper = jwtHelper;
            this.encryptionHelper = encryptionHelper;
        }

        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var hashString = await encryptionHelper.Encrypt("abc");
            // var token = await jwtHelper.GenerateToken();
            return Ok(hashString);
        }
    }
}