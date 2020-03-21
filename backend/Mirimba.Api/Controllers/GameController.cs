using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mirimba.Api.Hubs;

namespace Mirimba.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : Controller
    {
        //[HttpGet("reset/{roomId}")]
        //public async Task<IActionResult> Reset(string roomId)
        //{
        //    await GameHub.ResetRoom(roomId);

        //    return Ok();
        //}
    }
}