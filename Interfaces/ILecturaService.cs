using APICalidadAire.DTOs;

namespace APICalidadAire.Interfaces
{
    public interface ILecturaService
    {
        Task<LecturaRespuestaDTO?> ObtenerPorIdAsync(int id);

        Task<LecturaRespuestaDTO?> RegistrarAsync(CrearLecturaDTO dto);

        Task<List<LecturaFiltradaDTO>> ConsultarAsync(
            DateTime? desde,
            DateTime? hasta,
            string? contaminante,
            int? sensorId);
    }
}

