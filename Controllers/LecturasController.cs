using APICalidadAire.DTOs;
using APICalidadAire.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APICalidadAire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class LecturasController : ControllerBase
    {
        private readonly ILecturaService _lecturaService;
        private readonly IClimaService _climaService;

        public LecturasController(
            ILecturaService lecturaService,
            IClimaService climaService)
        {
            _lecturaService = lecturaService;
            _climaService = climaService;
        }

        // GET: api/lecturas/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> ObtenerPorId(int id)
        {
            var lectura = await _lecturaService.ObtenerPorIdAsync(id);

            if (lectura == null)
                return NotFound(new { mensaje = "Lectura no encontrada." });

            return Ok(lectura);
        }

        // POST: api/lecturas
        [HttpPost]
        public async Task<IActionResult> Registrar(CrearLecturaDTO dto)
        {
            if (dto.PM2_5 < 0 || dto.PM10 < 0 || dto.CO2 < 0)
            {
                return BadRequest(new
                {mensaje = "Los valores PM2.5, PM10 y CO2 no pueden ser negativos."});
            }

            if (dto.FechaHora == default)
            {
                return BadRequest(new
                {mensaje = "La fecha y hora de la lectura son obligatorias."});
            }

            if (dto.FechaHora > DateTime.UtcNow.AddMinutes(5))
            {
                return BadRequest(new
                {mensaje = "La fecha de la lectura no puede estar en el futuro."});
            }

            var lectura = await _lecturaService.RegistrarAsync(dto);

            if (lectura == null)
            {
                return NotFound(new
                {mensaje = "El sensor indicado no existe o no está activo."});
            }

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = lectura.Id },
                lectura);
        }

        // GET: api/lecturas
        [HttpGet]
        public async Task<IActionResult> Consultar(
            [FromQuery] DateTime? desde,
            [FromQuery] DateTime? hasta,
            [FromQuery] string? contaminante,
            [FromQuery] int? sensorId)
        {
            if (desde.HasValue && hasta.HasValue && desde > hasta)
            {
                return BadRequest(new
                {mensaje = "La fecha inicial no puede ser posterior a la fecha final."});
            }

            if (sensorId.HasValue && sensorId.Value <= 0)
            {
                return BadRequest(new
                {mensaje = "El identificador del sensor debe ser positivo."});
            }

            var contaminantesPermitidos = new[]
            {"PM2.5", "PM10", "CO2"};

            if (!string.IsNullOrWhiteSpace(contaminante) &&
                !contaminantesPermitidos.Contains(
                    contaminante.Trim(),
                    StringComparer.OrdinalIgnoreCase))
            {
                return BadRequest(new
                {mensaje = "Contaminante inválido. Utiliza PM2.5, PM10 o CO2."});
            }

            var lecturas = await _lecturaService.ConsultarAsync(
                desde,
                hasta,
                contaminante,
                sensorId);

            return Ok(lecturas);
        }

        // GET: api/lecturas/#/clima
        [HttpGet("{id:int}/clima")]
        public async Task<IActionResult> ObtenerClima(
            int id,
            [FromQuery] decimal latitud,
            [FromQuery] decimal longitud)
        {
            if (latitud < -90 || latitud > 90 ||
                longitud < -180 || longitud > 180)
            {
                return BadRequest(new
                {mensaje = "Las coordenadas geográficas no son válidas."});
            }

            try
            {
                var clima = await _climaService.ObtenerClimaAsync(
                    id,
                    latitud,
                    longitud);

                if (clima == null)
                    return NotFound(new { mensaje = "Lectura no encontrada." });

                return Ok(clima);
            }
            catch (HttpRequestException)
            {
                return StatusCode(502, new
                {mensaje = "No fue posible conectar con el servicio meteorológico."});
            }
            catch (System.Text.Json.JsonException)
            {
                return StatusCode(502, new
                {mensaje = "El servicio meteorológico devolvió información no válida."});
            }
        }
    }
}