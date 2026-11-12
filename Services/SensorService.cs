
using APICalidadAire.Data;
using APICalidadAire.DTOs;
using APICalidadAire.Interfaces;
using APICalidadAire.Models;
using Microsoft.EntityFrameworkCore;

namespace APICalidadAire.Services
{
    public class SensorService : ISensorService
    {
        private readonly ApplicationDbContext _context;

        public SensorService(ApplicationDbContext context)
        {_context = context;}

        public async Task<List<SensorRespuestaDTO>> ObtenerTodosAsync()
        {
            return await _context.SensoresCalidadAire.AsNoTracking()
                .Select(s => new SensorRespuestaDTO
                {
                    Id = s.Id,
                    Ubicacion = s.Ubicacion,
                    TipoGas = s.TipoGas,
                    Estado = s.Estado
                }).ToListAsync();
        }

        public async Task<SensorRespuestaDTO?> ObtenerPorIdAsync(int id)
        {
            return await _context.SensoresCalidadAire.AsNoTracking().Where(s => s.Id == id)
                .Select(s => new SensorRespuestaDTO
                {
                    Id = s.Id,
                    Ubicacion = s.Ubicacion,
                    TipoGas = s.TipoGas,
                    Estado = s.Estado
                }).FirstOrDefaultAsync();
        }

        public async Task<SensorRespuestaDTO> CrearAsync(CrearSensorDTO dto)
        {
            var sensor = new SensorCalidadAire
            {
                Ubicacion = dto.Ubicacion.Trim(),
                TipoGas = dto.TipoGas.Trim(),
                Estado = dto.Estado.Trim()
            };

            _context.SensoresCalidadAire.Add(sensor);
            await _context.SaveChangesAsync();

            return new SensorRespuestaDTO
            {
                Id = sensor.Id,
                Ubicacion = sensor.Ubicacion,
                TipoGas = sensor.TipoGas,
                Estado = sensor.Estado
            };
        }

        public async Task<bool> ActualizarAsync(int id, ActualizarSensorDTO dto)
        {
            var sensor = await _context.SensoresCalidadAire.FindAsync(id);

            if (sensor == null)
                return false;

            sensor.Ubicacion = dto.Ubicacion.Trim();
            sensor.TipoGas = dto.TipoGas.Trim();
            sensor.Estado = dto.Estado.Trim();

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var sensor = await _context.SensoresCalidadAire.FindAsync(id);

            if (sensor == null)
                return false;

            _context.SensoresCalidadAire.Remove(sensor);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}

