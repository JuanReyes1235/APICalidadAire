using System.ComponentModel.DataAnnotations;

namespace APICalidadAire.Models
{
    public class AlertaAire
    {
        public int Id { get; set; }

        [Required]
        public int SensorId { get; set; }

        [Required]
        [StringLength(20)]
        public string Nivel { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Mensaje { get; set; } = string.Empty;

        [Required]
        public DateTime FechaHora { get; set; }

        // Relación con el sensor.
        public SensorCalidadAire? Sensor { get; set; }
    }
}
