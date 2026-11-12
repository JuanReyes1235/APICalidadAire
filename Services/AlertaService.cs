using APICalidadAire.Data;
using APICalidadAire.DTOs;
using APICalidadAire.Interfaces;
using APICalidadAire.Models;
using Microsoft.EntityFrameworkCore;

namespace APICalidadAire.Services
{
    public class AlertaService : IAlertaService
    {
        private readonly ApplicationDbContext _context;

        public AlertaService(ApplicationDbContext context)
        {_context = context;}

        public async Task<AlertaAire?> GenerarAlertaAsync(LecturaAire lectura)
        {
            string? nivel = null;
            string? mensaje = null;

            if (lectura.CO2 > 5000 || lectura.PM2_5 > 250)
            {
                nivel = "Extrema";
                mensaje = "Nivel de contaminación extremadamente alto. Riesgo severo para la salud.";
            }
            else if (lectura.PM2_5 > 150 || lectura.PM10 > 200)
            {
                nivel = "Crítica";
                mensaje = "La calidad del aire es peligrosa. Se recomienda permanecer en interiores y usar mascarilla.";
            }
            else if ((lectura.PM2_5 >= 51 && lectura.PM2_5 <= 100)
                     || lectura.CO2 > 1000)
            {
                nivel = "Moderada";
                mensaje = "La calidad del aire es poco saludable para grupos sensibles (niños, adultos mayores, personas con enfermedades respiratorias).";
            }
            else if (lectura.PM2_5 >= 25 && lectura.PM2_5 <= 50)
            {
                nivel = "Leve";
                mensaje = "La calidad del aire es moderada, se recomienda reducir actividades al aire libre prolongadas.";
            }

            if (nivel == null)
                return null;

            var alerta = new AlertaAire
            {
                SensorId = lectura.SensorId,
                Nivel = nivel,
                Mensaje = mensaje!,
                FechaHora = lectura.FechaHora
            };

            _context.AlertasAire.Add(alerta);
            await _context.SaveChangesAsync();

            return alerta;
        }

        public async Task<List<AlertaRespuestaDTO>> ObtenerTodasAsync(
            string? nivel,
            int? sensorId,
            DateTime? desde,
            DateTime? hasta)
        {
            var consulta = _context.AlertasAire.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(nivel))
            {
                var nivelNormalizado = nivel.Trim().ToLowerInvariant();

                if (nivelNormalizado == "critica")
                    nivelNormalizado = "crítica";

                consulta = consulta.Where(a => a.Nivel.ToLower() == nivelNormalizado);
            }

            if (sensorId.HasValue)
                consulta = consulta.Where(a => a.SensorId == sensorId.Value);

            if (desde.HasValue)
                consulta = consulta.Where(a => a.FechaHora >= desde.Value);

            if (hasta.HasValue)
                consulta = consulta.Where(a => a.FechaHora <= hasta.Value);

            return await consulta.OrderByDescending(a => a.FechaHora)
                .Select(a => new AlertaRespuestaDTO
                {
                    Id = a.Id,
                    SensorId = a.SensorId,
                    Ubicacion = a.Sensor!.Ubicacion,
                    Nivel = a.Nivel,
                    Mensaje = a.Mensaje,
                    FechaHora = a.FechaHora
                }).ToListAsync();
        }

        public async Task<AlertaRespuestaDTO?> ObtenerPorIdAsync(int id)
        {
            return await _context.AlertasAire.AsNoTracking().Where(a => a.Id == id)
                .Select(a => new AlertaRespuestaDTO
                {
                    Id = a.Id,
                    SensorId = a.SensorId,
                    Ubicacion = a.Sensor!.Ubicacion,
                    Nivel = a.Nivel,
                    Mensaje = a.Mensaje,
                    FechaHora = a.FechaHora
                }).FirstOrDefaultAsync();
        }
    }
}
