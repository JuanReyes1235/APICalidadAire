using System.ComponentModel.DataAnnotations;

namespace APICalidadAire.DTOs
{
    public class CrearLecturaDTO
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int SensorId { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        public decimal PM2_5 { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        public decimal PM10 { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        public decimal CO2 { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }
    }

    public class LecturaFiltradaDTO
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
        public string Contaminante { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public string Unidad { get; set; } = string.Empty;
    }

    public class LecturaRespuestaDTO
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public decimal PM2_5 { get; set; }
        public decimal PM10 { get; set; }
        public decimal CO2 { get; set; }
        public DateTime FechaHora { get; set; }
        public AlertaRespuestaDTO? AlertaGenerada { get; set; }
    }
}
