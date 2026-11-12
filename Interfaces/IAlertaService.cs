using APICalidadAire.DTOs;
using APICalidadAire.Models;

namespace APICalidadAire.Interfaces
{
    public interface IAlertaService
    {
        Task<AlertaAire?> GenerarAlertaAsync(LecturaAire lectura);
        Task<List<AlertaRespuestaDTO>> ObtenerTodasAsync(
            string? nivel,
            int? sensorId,
            DateTime? desde,
            DateTime? hasta);
        Task<AlertaRespuestaDTO?> ObtenerPorIdAsync(int id);
    }
}
