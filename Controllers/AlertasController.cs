using APICalidadAire.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APICalidadAire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlertasController : ControllerBase
    {
        private readonly IAlertaService _alertaService;

        public AlertasController(IAlertaService alertaService)
        {_alertaService = alertaService;}

        // GET: api/alertas
        [HttpGet]
        public async Task<IActionResult> ObtenerTodas(
            [FromQuery] string? nivel,
            [FromQuery] int? sensorId,
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta)
        {
            if (desde.HasValue && hasta.HasValue && desde > hasta)
            {
                return BadRequest(new
                {mensaje = "La fecha inicial no puede ser posterior a la fecha final."});
            }

            var nivelesPermitidos = new[]
            {"leve", "moderada", "crítica", "critica", "extrema"};

            if (!string.IsNullOrWhiteSpace(nivel) && !nivelesPermitidos.Contains(nivel.Trim().ToLowerInvariant()))
            {
                return BadRequest(new
                {mensaje = "Nivel inválido. Utiliza Leve, Moderada, Crítica o Extrema."});
            }

            var alertas = await _alertaService.ObtenerTodasAsync(
                nivel,sensorId, desde, hasta);

            return Ok(alertas);
        }

        // GET: api/alertas/#
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var alerta = await _alertaService.ObtenerPorIdAsync(id);

            if (alerta == null)
                return NotFound(new { mensaje = "Alerta no encontrada." });

            return Ok(alerta);
        }
    }
}
