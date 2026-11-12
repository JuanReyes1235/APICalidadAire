using APICalidadAire.DTOs;

namespace APICalidadAire.Interfaces
{
    public interface IClimaService
    {
        Task<ClimaDTO?> ObtenerClimaAsync(
            int lecturaId,
            decimal latitud,
            decimal longitud);
    }
}
