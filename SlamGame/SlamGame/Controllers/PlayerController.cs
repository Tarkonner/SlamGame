using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SlamGame;
using System.Numerics;
using System.Security.Claims;

namespace SlamGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlayerController : ControllerBase
    {
        [HttpPost("create")]
        [Authorize]
        public IActionResult CreatePlayer()
        {
            // Get the user name from the token
            var userName = User?.Identity?.Name; // This is ClaimTypes.Name by default

            if (string.IsNullOrEmpty(userName))
                return Unauthorized("No name claim in token");

            GameManager.instance.AddPlayer(userName);
            return Ok($"Player {userName} made");
        }

        [HttpPost("move")]
        [Authorize]
        public IActionResult MovePlayer([FromQuery] string direction)
        {
            var userName = User?.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
                return Unauthorized("No name claim in token");

            if (string.IsNullOrWhiteSpace(direction))
                return BadRequest("Direction cannot be empty");

            if (!GameManager.instance.playerList.ContainsKey(userName))
                return NotFound($"Player {userName} not found");

            GameManager.instance.MovePlayer(userName, direction);
            var pos = GameManager.instance.GetPlayerInfo(userName);

            return Ok(new { userName, position = new { X = pos.X, Y = pos.Y } });
        }

        [HttpGet("allPosition")]
        public IActionResult GetAllPlayerInfo()
        {
            var info = GameManager.instance.playerList
                .ToDictionary(
                    p => p.Key,
                    p => new { X = p.Value.coordinats.X, Y = p.Value.coordinats.Y } 
                );
            return Ok(info);
        }



        [HttpDelete("remove/{id}")]
        public IActionResult RemovePlayer(string id)
        {
            GameManager.instance.RemovePlayer(id);
            return Ok(new { message = $"Player {id} removed." });
        }

        [HttpPost("validata")]
        [Authorize]
        public IActionResult ValidateToken()
        {
            return Ok();
        }
    }
}
