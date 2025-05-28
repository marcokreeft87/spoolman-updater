using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController(IInputHandler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateSpoolInput input) =>
        Ok(await handler.HandleAsync(input));

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetSettingsInput input) =>
        Ok(await handler.HandleAsync(input));
}
