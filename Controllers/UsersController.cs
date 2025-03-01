using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Models;
using UserApi.Services;

namespace UserApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {
        private readonly UserService _userService;

        public UsersController(UserService userService) {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> Get() {
            try {
                var users = await _userService.GetAsync();
                return Ok(new { message = "Usuarios traídos correctamente", data = users });
            } catch (Exception ex) {
                return StatusCode(500, new { message = "Se produjo un error al traer usuarios.", details = ex.Message });
            }
        }

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public async Task<IActionResult> Get(string id) {
            try {
                var user = await _userService.GetAsync(id);
                if (user == null) {
                    return NotFound(new { message = "Usuario no encontrado." });
                }
                return Ok(new { message = "Usuario traído con éxito", data = user });
            } catch (Exception ex) {
                return StatusCode(500, new { message = "Se produjo un error al traer el usuario.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<User>> Create([FromBody] User user) {
            if (!ModelState.IsValid) {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Se encontró un error en el cuerpo", errors });
            }

            try {
                await _userService.CreateAsync(user);
                return CreatedAtRoute("GetUser", new { id = user.Id?.ToString() }, new { message = "Usuario creado correctamente", data = user });
            } catch (Exception ex) {
                return StatusCode(500, new { message = "Se produjo un error al crear el usuario.", details = ex.Message });
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] User userIn) {
            if (!ModelState.IsValid) {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { message = "Se encontró un error en el cuerpo", errors });
            }
            try {
                var user = await _userService.GetAsync(id);

                if (user == null) {
                    return NotFound(new { message = "Usuario no encontrado." });
                }
                await _userService.UpdateAsync(id, userIn);
                return Ok(new { message = "Usuario actualizado correctamente" });
            } catch (Exception ex) {
                return StatusCode(500, new { message = "Se produjo un error al actualizar el usuario.", details = ex.Message });
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id) {
            try {
                var user = await _userService.GetAsync(id);

                if (user == null) {
                    return NotFound(new { message = "Usuario no encontrado." });
                }
                await _userService.RemoveAsync(id);
                return Ok(new { message = "Usuario eliminado correctamente" });
            } catch (Exception ex) {
                return StatusCode(500, new { message = "Se produjo un error al eliminar el usuario.", details = ex.Message });
            }
        }
    }
}
