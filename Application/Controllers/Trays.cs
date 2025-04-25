using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("[controller]")]
public class TraysController(IInputHandler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllAMSInput input) =>    
        Ok(await handler.HandleAsync(input));
}
