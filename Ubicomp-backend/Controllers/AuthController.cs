using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;


[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private IConfiguration _config;
    private readonly ApplicationDbContext _context;

    public AuthController(IConfiguration config, ApplicationDbContext context)
    {
        _config = config;
        _context = context;
    }


    [Route("Login")]
    public IActionResult Login(string username, string password)
    {
        UserModel login = new UserModel();
        login.UserName = username;
        login.Password = Base64Encode(password);

        IActionResult response = Unauthorized();
        var user = AuthenticatedUser(login);

        if (user != null)
        {
            var tokenStr = GenerateJSONWebToken(user);
            response = Ok(new { token = tokenStr });
        }

        return response;
    }

    private string GenerateJSONWebToken(UserModel userinfo)
    {

        var seckey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtToken:MaSecurtKey"]));

        var credentials = new SigningCredentials(seckey, SecurityAlgorithms.HmacSha256);

        var claims = new[] {

                new Claim(JwtRegisteredClaimNames.Sub, userinfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userinfo.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        var token = new JwtSecurityToken(
            issuer: _config["JwtToken:Issuer"],
            audience: _config["JwtToken:Issuer"],
            claims,
            expires: DateTime.Now.AddMinutes(120),
            signingCredentials: credentials);

        var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
        return encodetoken;

    }
    [Authorize]
    [HttpPost("Post")]
    public string Post()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        IList<Claim> claim = identity.Claims.ToList();
        var userName = claim[0].Value;
        return "Welcome" + userName;
    }


    [Authorize]
    [Route("Welcome")]
    [HttpGet]
    public string Welcome()
    {

        return "Welcome";
    }
    private UserModel AuthenticatedUser(UserModel login)
    {
        UserModel user = (from u in _context.Users where u.UserName == login.UserName select u).FirstOrDefault();
        if (user != null)
        {
            if (login.UserName == user.UserName && login.Password == user.Password)
            {
                return user;
            }
        }
        return null;
    }

    [HttpPost("Register")]
    public async Task<string> Register([FromBody] UserModel user)
    {

        if (user != null)
        {

            var iExist = (from ur in _context.Users where ur.EmailAddress == user.EmailAddress select ur).Any();
            if (!iExist)
            {
                user.Password = Base64Encode(user.Password);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return "user registered";
            }
        }
        return "not registered";

    }

    public static string Base64Encode(string plainText) =>
        Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));

}
