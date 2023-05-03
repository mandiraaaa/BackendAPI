using BackendAPI.Data;
using BackendAPI.Helpers;
using BackendAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace BackendAPI.Controllers
{
    public class UsersController : Controller
    {
        private readonly MainDbContext _mainDbContext;

        public UsersController(MainDbContext mainDbContext)
        {
            _mainDbContext = mainDbContext;
        }

        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            var userobj = await _mainDbContext.Users.DefaultIfEmpty()
                .FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (userobj == null)
                return NotFound(new { Message = "User Not Found!" });

            if (!PasswordHasher.VerifyPassword(user.Password, userobj.Password))
            {
                return BadRequest(new { Message = "Incorrect Password" });
            }

            userobj.Token = CreateJwt(userobj);

            return Ok(new
            {
                Token = userobj.Token,
                Message = "Login Success!"
            });
        }

        //[HttpPost("authenticate")]
        //public async Task<IActionResult> Authenticate([FromBody] User user)
        //{
        //    if (user == null)
        //        return BadRequest();

        //    var userobj = await _mainDbContext.Users
        //        .FirstOrDefaultAsync(x => x.UserName == user.UserName);
        //    if (userobj == null)
        //        return NotFound(new { Message = "User Not Found!" });

        //    if (userobj.Password == null)
        //    {
        //        return BadRequest(new { Message = "Password is null!" });
        //    }

        //    if (!PasswordHasher.VerifyPassword(user.Password, userobj.Password))
        //    {
        //        return BadRequest(new { Message = "Incorrect Password" });
        //    }
        //    return Ok(new
        //    {
        //        Message = "Login Success!"
        //    });
        //}

        //[HttpPost("authenticate")]
        //public async Task<IActionResult> Authenticate([FromBody] User user)
        //{
        //    if (user == null || string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
        //        return BadRequest();

        //    var userobj = await _mainDbContext.Users
        //        .FirstOrDefaultAsync(x => x.UserName == user.UserName);
        //    if (userobj == null)
        //        return NotFound(new { Message = "User Not Found!" });

        //    if (!PasswordHasher.VerifyPassword(user.Password, userobj.Password))
        //    {
        //        return BadRequest(new { Message = "Incorrect Password" });
        //    }
        //    return Ok(new
        //    {
        //        Message = "Login Success!"
        //    });
        //}




        [HttpPost("register")]

        public async Task<IActionResult> RegisterUser([FromBody] User user)
        {
            if(user == null)    
                return BadRequest();
            //check username
            if(await CheckUsernameExistingAsync (user.UserName))
                return BadRequest(new {Message = "Username Already Exist"});

            //check password
            var pass = CheckPasswordStrength(user.Password);
            if (!string.IsNullOrEmpty(pass))
                return BadRequest(new { Message = pass.ToString() });

            user.Password = PasswordHasher.HashPassword(user.Password);
            user.JobeRole = "User";
            user.Token = "";
            await _mainDbContext.Users.AddAsync(user);  
            await _mainDbContext.SaveChangesAsync();    
            return Ok(new
            {
                Message = "User Registered!"
            });
        }

        private Task<bool> CheckUsernameExistingAsync(string username)
            => _mainDbContext.Users.AnyAsync(x => x.UserName == username);


        private string CheckPasswordStrength(string password)
        {
            StringBuilder sb = new StringBuilder(); 
            if(password.Length < 5)
                sb.Append("Minimum password length should be 5"+Environment.NewLine);
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") 
                && Regex.IsMatch(password, "[0-9]"))) 
                sb.Append("Password Should be Alphanumeric" + Environment.NewLine);

            return sb.ToString();

        }

        private string CreateJwt(User user) 
        { 
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecret....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Role, user.JobeRole),
                new Claim(ClaimTypes.Name,$"{user.Name}")
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        [Authorize]
        [HttpGet("authusers")]
        public async Task<ActionResult<User>> GetAllUsers()
        {
            return Ok(await _mainDbContext.Users.DefaultIfEmpty()
                .ToListAsync());
        }



        //private Task<bool> CheckPhoneNumberExistingAsync(string phonenumber)
        //    => _mainDbContext.Users.AnyAsync(x => x.UserName == phonenumber);


        //[HttpPost("RegisterClients")]

        //public async Task<IActionResult> RegisterClients([FromBody] User user)
        //{
        //    if (user == null)
        //        return BadRequest();
        //    //check username
        //    if (await CheckPhoneNumberExistingAsync(user.UserName))
        //        return BadRequest(new { Message = "Username Already Exist" });

        //    //check password
        //    var pass = CheckPasswordStrength(user.Password);
        //    if (!string.IsNullOrEmpty(pass))
        //        return BadRequest(new { Message = pass.ToString() });

        //    //user.Password = PasswordHasher.HashPassword(user.Password);
        //    //user.JobeRole = "User";
        //    //user.Token = "";
        //    await _mainDbContext.Users.AddAsync(user);
        //    await _mainDbContext.SaveChangesAsync();
        //    return Ok(new
        //    {
        //        Message = "User Registered!"
        //    });
        //}

        [HttpPost("authenticateclient")]
        public async Task<IActionResult> AuthenticateClient([FromBody] User user)
        {
            if (user == null)
                return BadRequest();

            var userobj = await _mainDbContext.Users.DefaultIfEmpty()
                .FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (userobj == null)
                return NotFound(new { Message = "User Not Found!" });

            if (userobj.Password != user.Password)
            {
                return BadRequest(new { Message = "Incorrect Password" });
            }

            userobj.Token = CreateJwtForApp(userobj);

            return Ok(new
            {
                Token = userobj.Token,
                Message = "Login Success!"
            });
        }

        private string CreateJwtForApp(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("veryverysecret....");
            var identity = new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.Name, user.Name)
            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials,
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }











    }
}
