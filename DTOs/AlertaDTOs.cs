namespace APICalidadAire.DTOs
{
    public class AlertaRespuestaDTO
    {
        public int Id { get; set; }
        public int SensorId { get; set; }
        public string Ubicacion { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public string Mensaje { get; set; } = string.Empty;
        public DateTime FechaHora { get; set; }
    }
}
