using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ExpenseTrackerProject.DTOs;
using ExpenseTrackerProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ExpenseTrackerProject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<User> UserManager,
                          SignInManager<User> SignInManager,
                          IConfiguration Configuration)
        {
            userManager = UserManager;
            signInManager = SignInManager;
            configuration = Configuration;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new User
            {
                Name = registerDTO.Name,
                Email = registerDTO.Email, 
                UserName = registerDTO.Username,
                PhoneNumber = "02020"
                
            };

            var result = await userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                var token = GenerateJwtToken(user);
                return Ok(new { token });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return BadRequest(ModelState);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDTO loginDTO)
        {
            if (!ModelState.IsValid)
                return View(loginDTO);

            var user = await userManager.FindByNameAsync(loginDTO.Email);
            if (user == null)
                return Unauthorized(new { message = "There is no email adreesss" });

            
            var result = await signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded)
                return Unauthorized(new { message = "Invalid credentials" });

            
            var token = GenerateJwtToken(user);

            Response.Cookies.Append("X-Access-Token", token, new CookieOptions
            {
                HttpOnly = true,  
                Secure = false,
                Expires = DateTime.UtcNow.AddDays(1),  
                SameSite = SameSiteMode.Strict 
            });


            return RedirectToAction("Privacy","Home");
        }

        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim("nameidentifier", user.Id.ToString()),
                new Claim("name", user.UserName),
                new Claim("email", user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["Jwt:Issuer"],
                audience: configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
