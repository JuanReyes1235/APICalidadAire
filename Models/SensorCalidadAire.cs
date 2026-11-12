using System.ComponentModel.DataAnnotations;

namespace APICalidadAire.Models
{
    public class SensorCalidadAire
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Ubicacion { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TipoGas { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Estado { get; set; } = "Activo";

        // Un sensor puede tener muchas lecturas.
        public ICollection<LecturaAire> Lecturas { get; set; } = new List<LecturaAire>();

        // Un sensor puede generar muchas alertas.
        public ICollection<AlertaAire> Alertas { get; set; } = new List<AlertaAire>();
    }
}
