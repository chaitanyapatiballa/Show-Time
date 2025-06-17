using AuthService.Logic;
using DBModels.Dto;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthLogic _logic;

    public AuthController(AuthLogic logic)
    {
        _logic = logic;
    }

}
