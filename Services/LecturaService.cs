using APICalidadAire.Data;
using APICalidadAire.DTOs;
using APICalidadAire.Interfaces;
using APICalidadAire.Models;
using Microsoft.EntityFrameworkCore;

namespace APICalidadAire.Services
{
    public class LecturaService : ILecturaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAlertaService _alertaService;

        public LecturaService(
            ApplicationDbContext context,
            IAlertaService alertaService)
        {
            _context = context;
            _alertaService = alertaService;
        }

        public async Task<LecturaRespuestaDTO?> RegistrarAsync(
            CrearLecturaDTO dto)
        {
            var sensor = await _context.SensoresCalidadAire.FirstOrDefaultAsync(s => s.Id == dto.SensorId);

            if (sensor == null || sensor.Estado != "Activo")
                return null;

            var lectura = new LecturaAire
            {
                SensorId = dto.SensorId,
                PM2_5 = dto.PM2_5,
                PM10 = dto.PM10,
                CO2 = dto.CO2,
                FechaHora = dto.FechaHora
            };

            _context.LecturasAire.Add(lectura);
            await _context.SaveChangesAsync();

            // Generar automáticamente la alerta correspondiente.
            var alerta = await _alertaService.GenerarAlertaAsync(lectura);

            return new LecturaRespuestaDTO
            {
                Id = lectura.Id,
                SensorId = lectura.SensorId,
                PM2_5 = lectura.PM2_5,
                PM10 = lectura.PM10,
                CO2 = lectura.CO2,
                FechaHora = lectura.FechaHora,
                AlertaGenerada = alerta == null ? null : new AlertaRespuestaDTO
                {
                    Id = alerta.Id,
                    SensorId = alerta.SensorId,
                    Ubicacion = sensor.Ubicacion,
                    Nivel = alerta.Nivel,
                    Mensaje = alerta.Mensaje,
                    FechaHora = alerta.FechaHora
                }
            };
        }

        public async Task<LecturaRespuestaDTO?> ObtenerPorIdAsync(int id)
        {
            return await _context.LecturasAire.AsNoTracking().Where(l => l.Id == id)
                .Select(l => new LecturaRespuestaDTO
                {
                    Id = l.Id,
                    SensorId = l.SensorId,
                    PM2_5 = l.PM2_5,
                    PM10 = l.PM10,
                    CO2 = l.CO2,
                    FechaHora = l.FechaHora
                }).FirstOrDefaultAsync();
        }

        public async Task<List<LecturaFiltradaDTO>> ConsultarAsync(
            DateTime? desde,
            DateTime? hasta,
            string? contaminante,
            int? sensorId)
        {
            var consulta = _context.LecturasAire.AsNoTracking().AsQueryable();

            if (desde.HasValue)
                consulta = consulta.Where(l => l.FechaHora >= desde.Value);

            if (hasta.HasValue)
                consulta = consulta.Where(l => l.FechaHora <= hasta.Value);

            if (sensorId.HasValue)
                consulta = consulta.Where(l => l.SensorId == sensorId.Value);

            var lecturas = await consulta
                .Select(l => new
                {
                    l.Id,
                    l.SensorId,
                    Ubicacion = l.Sensor!.Ubicacion,
                    l.FechaHora,
                    l.PM2_5,
                    l.PM10,
                    l.CO2
                }).OrderByDescending(l => l.FechaHora).ToListAsync();

            var resultado = new List<LecturaFiltradaDTO>();

            foreach (var lectura in lecturas)
            {
                if (string.IsNullOrWhiteSpace(contaminante) ||
                    contaminante.Equals("PM2.5", StringComparison.OrdinalIgnoreCase))
                {
                    resultado.Add(new LecturaFiltradaDTO
                    {
                        Id = lectura.Id,
                        SensorId = lectura.SensorId,
                        Ubicacion = lectura.Ubicacion,
                        FechaHora = lectura.FechaHora,
                        Contaminante = "PM2.5",
                        Valor = lectura.PM2_5,
                        Unidad = "µg/m³"
                    });
                }

                if (string.IsNullOrWhiteSpace(contaminante) ||
                    contaminante.Equals("PM10", StringComparison.OrdinalIgnoreCase))
                {
                    resultado.Add(new LecturaFiltradaDTO
                    {
                        Id = lectura.Id,
                        SensorId = lectura.SensorId,
                        Ubicacion = lectura.Ubicacion,
                        FechaHora = lectura.FechaHora,
                        Contaminante = "PM10",
                        Valor = lectura.PM10,
                        Unidad = "µg/m³"
                    });
                }

                if (string.IsNullOrWhiteSpace(contaminante) ||
                    contaminante.Equals("CO2", StringComparison.OrdinalIgnoreCase))
                {
                    resultado.Add(new LecturaFiltradaDTO
                    {
                        Id = lectura.Id,
                        SensorId = lectura.SensorId,
                        Ubicacion = lectura.Ubicacion,
                        FechaHora = lectura.FechaHora,
                        Contaminante = "CO2",
                        Valor = lectura.CO2,
                        Unidad = "ppm"
                    });
                }
            }

            return resultado;
        }
    }
}

