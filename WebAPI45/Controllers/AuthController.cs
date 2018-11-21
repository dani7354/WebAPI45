using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPI45.Model;



namespace WebAPI45.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signinManager;
        private readonly UserManager<IdentityUser> _userManager;

        public IPasswordHasher<IdentityUser> PasswordHasher { get; }

        public AuthController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IPasswordHasher<IdentityUser> passwordHasher)
        {
            _signinManager = signInManager;
            _userManager = userManager;
            PasswordHasher = passwordHasher;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginModel login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _signinManager.PasswordSignInAsync(
                login.Email, login.Password, false, false

            );

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Login error!");
                return BadRequest();
            }

            return Ok();
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registration)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var newUser = new IdentityUser
            {
                Email = registration.Email,
                UserName = registration.Email,
            };
            var result = await _userManager.CreateAsync(newUser, registration.Password);

            return !result.Succeeded ? BadRequest() : (IActionResult)Ok();
        }
        [HttpPost, Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signinManager.SignOutAsync();

            return Ok();
        }

        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if(user != null)
                {
                    if(PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                    {

                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                        };
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ALONGKEY"));
                        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            issuer: "http://localhost/", audience: "http://localhost/", 
                            claims: claims, 
                            expires: DateTime.UtcNow.AddMinutes(15), 
                            signingCredentials: credentials
                        );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        });


                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
    }
}