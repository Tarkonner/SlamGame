using Microsoft.AspNetCore.Mvc;
using SlamGame;
using System.Numerics;

namespace SlamGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        [HttpPost("create")]
        public IActionResult CreatePlayer([FromQuery] string id)
        {
            GameManager.instance.AddPlayer(id);
            return Ok(new { message = $"Player {id} created." });
        }

        [HttpPost("move")]
        public IActionResult MovePlayer([FromQuery] string id, [FromQuery] string direction)
        {
            GameManager.instance.MovePlayer(id, direction);
            var pos = GameManager.instance.GetPlayerInfo(id);
            return Ok(new { id, position = pos });
        }

        [HttpGet("info/{id}")]
        public IActionResult GetPlayerInfo(string id)
        {
            var pos = GameManager.instance.GetPlayerInfo(id);
            return Ok(new { id, position = pos });
        }

        [HttpGet("all")]
        public IActionResult GetAllPlayerInfo()
        {
            var info = GameManager.instance.GetAllPlayerInfo();
            return Ok(info);
        }

        [HttpDelete("remove/{id}")]
        public IActionResult RemovePlayer(string id)
        {
            GameManager.instance.RemovePlayer(id);
            return Ok(new { message = $"Player {id} removed." });
        }
    }
}
