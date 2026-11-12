using APICalidadAire.DTOs;

namespace APICalidadAire.Interfaces
{
    public interface ISensorService
    {
        Task<List<SensorRespuestaDTO>> ObtenerTodosAsync();
        Task<SensorRespuestaDTO?> ObtenerPorIdAsync(int id);
        Task<SensorRespuestaDTO> CrearAsync(CrearSensorDTO dto);
        Task<bool> ActualizarAsync(int id, ActualizarSensorDTO dto);
        Task<bool> EliminarAsync(int id);
    }
}
