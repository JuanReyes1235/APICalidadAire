using APICalidadAire.Data;
using APICalidadAire.DTOs;
using APICalidadAire.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text.Json;

namespace APICalidadAire.Services
{
    public class ClimaService : IClimaService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public ClimaService(
            ApplicationDbContext context,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<ClimaDTO?> ObtenerClimaAsync(
            int lecturaId,
            decimal latitud,
            decimal longitud)
        {
            var lectura = await _context.LecturasAire.AsNoTracking().FirstOrDefaultAsync(l => l.Id == lecturaId);

            if (lectura == null)
                return null;

            var lat = latitud.ToString(CultureInfo.InvariantCulture);
            var lon = longitud.ToString(CultureInfo.InvariantCulture);

            var url = $"https://api.open-meteo.com/v1/forecast?latitude={lat}&longitude={lon}&current=temperature_2m,relative_humidity_2m";

            var client = _httpClientFactory.CreateClient();

            using var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();

            using var documento = JsonDocument.Parse(
                await response.Content.ReadAsStringAsync());

            var actual = documento.RootElement.GetProperty("current");

            var temperatura = actual.GetProperty("temperature_2m").GetDecimal();

            var humedad = actual.GetProperty("relative_humidity_2m").GetInt32();

            return new ClimaDTO
            {
                LecturaId = lectura.Id,
                SensorId = lectura.SensorId,
                FechaLectura = lectura.FechaHora,
                Temperatura = temperatura,
                Humedad = humedad,
                FechaConsultaClima = DateTime.UtcNow,
                Fuente = "Open-Meteo",
                Nota = "Datos meteorológicos actuales de las coordenadas indicadas; no representan necesariamente las condiciones históricas de la lectura."
            };
        }
    }
}
