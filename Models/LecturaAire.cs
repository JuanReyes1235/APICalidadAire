using System.ComponentModel.DataAnnotations;

namespace APICalidadAire.Models
{
    public class LecturaAire
    {
        public int Id { get; set; }

        [Required]
        public int SensorId { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        public decimal PM2_5 { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        public decimal PM10 { get; set; }

        [Range(typeof(decimal), "0", "99999999")]
        public decimal CO2 { get; set; }

        [Required]
        public DateTime FechaHora { get; set; }

        // Relación con el sensor.
        public SensorCalidadAire? Sensor { get; set; }
    }
}
