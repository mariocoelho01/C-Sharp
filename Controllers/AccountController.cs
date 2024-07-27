using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.Services;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecureIdentity.Password;

namespace Blog.Controllers;

/*[Authorize]*/
[ApiController]
public class AccountController : ControllerBase
{
    /*private readonly TokenServices _tokenServices;
    public AccountController(TokenServices tokenServices)//depends on tokenservices
    {
        _tokenServices = tokenServices;
    }*/
    
    /*[AllowAnonymous]*/
    [HttpPost("v1/accounts")]
    public async Task<IActionResult> Post(
        [FromBody]RegisterViewModel model,
        [FromServices] EmailService emailService,
        [FromServices]BlogDataContext context)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = new User()
        {
            Name = model.Name,
            Email = model.Email,
            Slug = model.Email.Replace("@", "-").Replace(".", "-")
        };
        //Generator password
        var password = PasswordGenerator.Generate(25);
        user.PasswordHash = PasswordHasher.Hash(password);

        try
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            emailService.Send(
                user.Name,
                user.Email,
                "Welcome to the blog!",
                $"Your password is : </strong>{password}</strong>");
            
            return Ok(new ResultViewModel<dynamic>(new
            {
                user = user.Email,
                password
            }));
            
        }
        catch(DbUpdateException)
        {
            return StatusCode(400, new ResultViewModel<string>("05X99 - This E-mail is register"));
        }
        catch
        {
            return StatusCode(400, new ResultViewModel<string>("05X04 - Internal fault"));
        }
    }

    [HttpPost("v1/accounts/login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginViewModel model,
        [FromServices] BlogDataContext context,
        [FromServices] TokenServices tokenServices)
    {
        if (!ModelState.IsValid)
            return BadRequest(new ResultViewModel<string>(ModelState.GetErrors()));

        var user = await context
                .Users
                .AsNoTracking()
                .Include(x=> x.Roles)
                .FirstOrDefaultAsync(x => x.Email == model.Email);

        if (user == null)
            return StatusCode(401, new ResultViewModel<string>("User or password is invalid!"));

        if (!PasswordHasher.Verify(user.PasswordHash, model.Password))
            return StatusCode(401, new ResultViewModel<string>("User or password is invalid!"));

        try
        {
            var token = tokenServices.GenerateToken(user);
            return Ok(new ResultViewModel<string>(token, null));
        }
        catch
        {
            return StatusCode(500, new ResultViewModel<string>("05X04 - Internal fault "));
        }
    }
    
    
}