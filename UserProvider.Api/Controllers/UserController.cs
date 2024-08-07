using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using UserProvider.Api.Services;
using UserProvider.Data.Models;

namespace UserProvider.Api.Controllers;

public class UserController(IUserAccountService userProfileService) : ControllerBase
{
    private readonly IUserAccountService _userService = userProfileService;

    [HttpPost]
    [Route("/api/account/create-profile")]
    public async Task<IActionResult> CreateProfile(UserAccountModel model)
    {
        try
        {
            if (model != null)
                return await _userService.CreateProfileAsync(model) ? Ok() : Conflict();
            else
                return NotFound();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return BadRequest();
        }
    }

    [HttpPost]
    [Route("/api/account/update-user")]
    public async Task<IActionResult> UpdateUser(UserAccountModel model)
    {
        try
        {
            if (model != null)
                return await _userService.UpdateUserAsync(model) ? Ok() : Conflict();
            else
                return NotFound();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return BadRequest();
        }
    }

    [HttpGet]
    [Route("/api/account/getuserbyid")]
    public async Task<ActionResult<UserAccountModel>> GetUserById(string userId)
    {
        UserAccountModel user;
        try
        {
            if (!string.IsNullOrWhiteSpace(userId))
                user = await _userService.GetUserByIdAsync(userId);
            else
                return BadRequest(null!);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return BadRequest();
        }

        return user != null ? Ok(user) : NotFound();
    }

    [HttpGet]
    [Route("/api/account/getuserbyemail")]
    public async Task<ActionResult<UserAccountModel>> GetUserByEmail(string email)
    {
        UserAccountModel user;
        try
        {
            if (!string.IsNullOrWhiteSpace(email))
                user = await _userService.GetUserByEmailAsync(email);
            else
                return BadRequest();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return BadRequest();
        }

        return user != null ? Ok(user) : NotFound();
    }
}
