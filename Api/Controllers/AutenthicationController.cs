using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Bussines.Dto.AutenticationDto;
using Data;
using Data.Models;
using Data.Models.AuthenticationModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutenthicationController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly FacturesManagementContext _context;
    private readonly IConfiguration _configuration;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public AutenthicationController(
        UserManager<ApplicationUser> userManager,
        FacturesManagementContext context,
        IConfiguration configuration,
        TokenValidationParameters tokenValidationParameters)
    {
        ArgumentNullException.ThrowIfNull(userManager);
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(configuration);
        ArgumentNullException.ThrowIfNull(tokenValidationParameters);

        _userManager = userManager;
        _context = context;
        _configuration = configuration;
        _tokenValidationParameters = tokenValidationParameters;
    }

    [HttpPost("register-user")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterVM registerVM)
    {
        var userExist = await _userManager.FindByEmailAsync(registerVM.EmailAddress);

        if (userExist != null)
        {
            return BadRequest($"User with email = {userExist.Email} alredy exists");
        }

        var company = new Company()
        {
            Address = new CompanyAddress()
            {
                City = registerVM.Company.Address.City,
                Country = registerVM.Company.Address.Country,
                HouseNumber = registerVM.Company.Address.HouseNumber,
                LocalNumber = registerVM.Company.Address.LocalNumber,
                PosteCode = registerVM.Company.Address.PosteCode,
                Street = registerVM.Company.Address.Street,
            },
            Name = registerVM.Company.Name,
            NIP = registerVM.Company.NIP,
        };

        await _context.Companies.AddAsync(company);

        var newUser = new ApplicationUser()
        {
            UserName = registerVM.UserName,
            CompanyId = company.Id,
            Email = registerVM.EmailAddress,
        };

        var result = await _userManager.CreateAsync(newUser, registerVM.Password);

        if (result.Succeeded)
        {
            await _context.SaveChangesAsync();
            return Ok("User created!");
        }
        else
        {
            // Get detailed error messages
            var errors = result.Errors.Select(e => e.Description);
            var errorMessage = string.Join("; ", errors);

            return BadRequest($"User could not be created: {errorMessage}");
        }
    }

    [HttpPost("login-user")]
    public async Task<IActionResult> Login([FromBody] LoginVM loginVM)
    {
        var userExist = await _userManager.FindByEmailAsync(loginVM.EmailAddress);

        if (userExist != null && await _userManager.CheckPasswordAsync(userExist, loginVM.Password))
        {
            var token = await GenerateJWTTokenAsync(userExist, null);
            return Ok(token);
        }
        else
        {
            return Unauthorized();
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] TokenRequestVM tokenRequestVM)
    {
        var result = await VerifyAndGenerateTokenAsync(tokenRequestVM);

        return Ok(result);
    }

    private async Task<AuthResultVM> VerifyAndGenerateTokenAsync(TokenRequestVM tokenRequestVM)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();
        var storedToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == tokenRequestVM.RefreshToken);
        var dbUser = await _userManager.FindByIdAsync(storedToken.UserId) ?? throw new Exception("User not found");

        try
        {
            var tokenCheckResult = jwtTokenHandler.ValidateToken(
                tokenRequestVM.Token,
                _tokenValidationParameters,
                out var validatedToken);

            return await GenerateJWTTokenAsync(dbUser, storedToken);
        }
        catch (SecurityTokenExpiredException)
        {
            return storedToken.DateExpired >= DateTime.UtcNow
                ? await GenerateJWTTokenAsync(dbUser, storedToken)
                : await GenerateJWTTokenAsync(dbUser, null);
        }
    }

    private async Task<AuthResultVM> GenerateJWTTokenAsync(ApplicationUser applicationUser, RefreshToken rToken)
    {
        var authClaims = new List<Claim>()
        {
            new(ClaimTypes.Name, applicationUser.UserName ?? throw new Exception("GenerateJWTTokenAsync username error")),
            new(ClaimTypes.NameIdentifier, applicationUser.Id),
            new(JwtRegisteredClaimNames.Email, applicationUser.Email ?? throw new Exception("GenerateJWTTokenAsync email error")),
            new(JwtRegisteredClaimNames.Sub, applicationUser.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]
            ?? throw new Exception("Secret Key is Null")));

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:Issuer"] ?? throw new Exception("Issuer is Null"),
            audience: _configuration["JWT:Audience"] ?? throw new Exception("Audience is null"),
            expires: DateTime.UtcNow.AddMinutes(1),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));

        var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

        if (rToken != null)
        {
            var rTokenResponse = new AuthResultVM()
            {
                Token = jwtToken,
                RefreshToken = rToken.Token,
                CompanyId = applicationUser.CompanyId,
                ExpiresAt = token.ValidTo,
            };

            return rTokenResponse;
        }

        var refreshToken = new RefreshToken()
        {
            JwtId = token.Id,
            IsRevoked = false,
            UserId = applicationUser.Id,
            DateAdded = DateTime.UtcNow,
            DateExpired = DateTime.UtcNow.AddMonths(6),
            Token = Guid.NewGuid().ToString() + "_" + Guid.NewGuid().ToString(),
        };

        await _context.RefreshTokens.AddAsync(refreshToken);
        await _context.SaveChangesAsync();

        var response = new AuthResultVM()
        {
            Token = jwtToken,
            RefreshToken = refreshToken.Token,
            CompanyId = applicationUser.CompanyId,
            ExpiresAt = token.ValidTo,
        };

        return response;
    }
}
