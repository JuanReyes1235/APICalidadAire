using System.ComponentModel.DataAnnotations;

namespace APICalidadAire.DTOs
{
    public class CrearSensorDTO
    {
        [Required]
        [StringLength(150)]
        public string Ubicacion { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TipoGas { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Estado { get; set; } = "Activo";
    }

    public class ActualizarSensorDTO
    {
        [Required]
        [StringLength(150)]
        public string Ubicacion { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TipoGas { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Estado { get; set; } = string.Empty;
    }

    public class SensorRespuestaDTO
    {
        public int Id { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string TipoGas { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
    }
}
