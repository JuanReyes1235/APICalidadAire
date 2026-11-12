namespace APICalidadAire.DTOs
{
    public class ClimaDTO
    {
        public int LecturaId { get; set; }
        public int SensorId { get; set; }
        public DateTime FechaLectura { get; set; }

        public decimal Temperatura { get; set; }
        public int Humedad { get; set; }

        public DateTime FechaConsultaClima { get; set; }

        public string Fuente { get; set; } = "Open-Meteo";
        public string Nota { get; set; } = string.Empty;
    }
}
