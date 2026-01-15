using Domain;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers;

[ApiController]
[Route("[controller]")]
public class SpoolsController(IInputHandler handler) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Update([FromBody] UpdateSpoolInput input) =>
        Ok(await handler.HandleAsync(input));

    [HttpPost("tray")]
    public async Task<IActionResult> UpdateTray([FromBody] UpdateTrayInput input) =>
        Ok(await handler.HandleAsync(input));

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetAllSpoolsInput input) =>
        Ok(await handler.HandleAsync(input));
}
