using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.MinimalApi.Domain;
using WebApi.MinimalApi.Models;

namespace WebApi.MinimalApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : Controller
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;
    
    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }
    
    [HttpGet("{userId}", Name = nameof(GetUserById))]
    [Produces("application/json", "application/xml")]
    public ActionResult<UserDto> GetUserById([FromRoute] Guid userId)
    {
        var user = userRepository.FindById(userId);
        if (user == null)
            return NotFound();

        return Ok(mapper.Map<UserDto>(user));
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] CreateUserModel? user)
    {
        if (user == null)
        {
            ModelState.AddModelError("EmptyRequest", "Был получен пустой запрос.");
            return BadRequest();
        }

        if (user.Login.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            ModelState.AddModelError("Login", "Логин должен состоять из цифр и букв.");
            return UnprocessableEntity(ModelState);
        }
        var res = userRepository.Insert(mapper.Map<UserEntity>(user));
        
        return CreatedAtRoute(
            nameof(GetUserById),
            new { userId = res.Id },
            res);
    }
}