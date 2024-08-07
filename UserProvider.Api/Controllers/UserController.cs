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
        bool result;
        try
        {
            if (model != null)
                result = await _userService.CreateProfileAsync(model);
            else
                return BadRequest();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return BadRequest();
        }

        return result ? Ok() : Conflict();
    }

    [HttpPost]
    [Route("/api/account/update-user")]
    public async Task<IActionResult> UpdateUser(UserAccountModel model)
    {
        bool result;
        try
        {
            if (model != null)
                result = await _userService.UpdateUserAsync(model);
            else
                return BadRequest();
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return BadRequest();
        }

        return result ? Ok() : Conflict();
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
