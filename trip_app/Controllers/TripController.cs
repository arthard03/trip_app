using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using trip_app.DTO;
using trip_app.Exceptions;
using trip_app.OutputFolder;
using trip_app.OutputFolder.Helper;
using trip_app.Service;

namespace trip_app.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly ITripService _tripRepository;
        private readonly ApbdContext _context;
        private readonly IConfiguration _configuration;

        public TripController(ITripService tripRepository,IConfiguration configuration, ApbdContext context)
        {
            _tripRepository = tripRepository;
            _context = context;
            _configuration = configuration;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        // {
        //     var result = await _tripRepository.GetPaginatedTripsAsync(page, pageSize);
        //     return Ok(result);
        // }
        //
        // [HttpGet("clients")] 
        // public async Task<IActionResult> GetTripsClients()
        // {
        //     var result = await _tripRepository.GetAllTripsAsync();
        //     return Ok(result);
        // }
        // [HttpPost("{idTrip}/clients")]
        // public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] ClientPostDTO clientDto)
        // {
        //     try
        //     {
        //         var result = await _tripRepository.AssignClientToTripAsync(clientDto, idTrip);
        //         if (!result)
        //         {
        //             return BadRequest("Unable to assign client to trip.");
        //         }
        //
        //         return Ok();
        //     }
        //     catch (NotFoundException ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }
        //
        // [HttpDelete("{idClient}")]
        // public async Task<IActionResult> DeleteClient(int idClient)
        // {
        //     try
        //     {
        //         var result = await _tripRepository.DeleteClientAsync(idClient);
        //
        //         if (!result)
        //         {
        //             return NotFound("Client not found.");
        //         }
        //
        //         return NoContent();
        //     }
        //     catch (NotFoundException ex)
        //     {
        //         return BadRequest(ex.Message);
        //     }
        // }

         [AllowAnonymous]
    [HttpPost("register")]
    public IActionResult RegisterStudent(RegisterRequest model)
    {
        var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(model.Password);

        //alaMaKota
        //hash(alaMaKota+salt1+pepper)=>sdsd3dfgsd3fdfdfdfdsfsfsdfsdfsdf
        //hash(alaMaKota+salt2+pepper)=>df4htghdfgdfg32fedfdfsfq23fedfdd


        var user = new AppUser()
        {
            Email = model.Email,
            Login = model.Login,
            Password = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = SecurityHelpers.GenerateRefreshToken(),
            RefreshTokenExp = DateTime.Now.AddDays(1)
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok();
    }

    [Authorize]
    [HttpGet]
    public IActionResult GetStudents()
    {
        var claimsFromAccessToken = User.Claims;
        return Ok("Secret data");
    }

    [AllowAnonymous]
    [HttpGet("anon")]
    public IActionResult GetAnonData()
    {
        return Ok("Public data");
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        AppUser user = _context.Users.Where(u => u.Login == loginRequest.Login).FirstOrDefault();

        string passwordHashFromDb = user.Password;
        string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

        if (passwordHashFromDb != curHashedPassword)
        {
            return Unauthorized();
        }


   

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(token),
            refreshToken = user.RefreshToken
        });
    }

    [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
    [HttpPost("refresh")]
    public IActionResult Refresh(RefreshTokenRequest refreshToken)
    {
        AppUser user = _context.Users.Where(u => u.RefreshToken == refreshToken.RefreshToken).FirstOrDefault();
        if (user == null)
        {
            throw new SecurityTokenException("Invalid refresh token");
        }

        if (user.RefreshTokenExp < DateTime.Now)
        {
            throw new SecurityTokenException("Refresh token expired");
        }

        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken jwtToken = new JwtSecurityToken(
            issuer: "https://localhost:5001",
            audience: "https://localhost:5001",
            expires: DateTime.Now.AddMinutes(10),
            signingCredentials: creds
        );

        user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
        user.RefreshTokenExp = DateTime.Now.AddDays(1);
        _context.SaveChanges();

        return Ok(new
        {
            accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
            refreshToken = user.RefreshToken
        });
    }
        
    }
}