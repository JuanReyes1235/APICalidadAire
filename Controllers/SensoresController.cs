using APICalidadAire.DTOs;
using APICalidadAire.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APICalidadAire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SensoresController : ControllerBase
    {
        private readonly ISensorService _sensorService;

        public SensoresController(ISensorService sensorService)
        {_sensorService = sensorService;}

        // GET: api/sensores
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var sensores = await _sensorService.ObtenerTodosAsync();
            return Ok(sensores);
        }

        // GET: api/sensores/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var sensor = await _sensorService.ObtenerPorIdAsync(id);

            if (sensor == null)
                return NotFound(new { mensaje = "Sensor no encontrado." });

            return Ok(sensor);
        }

        // POST: api/sensores
        [HttpPost]
        public async Task<IActionResult> Crear(CrearSensorDTO dto)
        {
            var sensor = await _sensorService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = sensor.Id },
                sensor);
        }

        // PUT: api/sensores/#
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Actualizar(
            int id,
            ActualizarSensorDTO dto)
        {
            var actualizado = await _sensorService.ActualizarAsync(id, dto);

            if (!actualizado)
                return NotFound(new { mensaje = "Sensor no encontrado." });

            return Ok(new { mensaje = "Sensor actualizado correctamente." });
        }

        // DELETE: api/sensores/#
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var eliminado = await _sensorService.EliminarAsync(id);

            if (!eliminado)
                return Conflict(new
                {mensaje = "No se pudo eliminar el sensor. Puede que no exista o tenga lecturas o alertas asociadas."
                });

            return Ok(new { mensaje = "Sensor eliminado correctamente." });
        }
    }
}
