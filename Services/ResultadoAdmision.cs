namespace ActividadesExtraPortal.Services
{
    /// <summary>
    /// Objeto de transferencia de datos (DTO) que transporta el resultado de la evaluación
    /// de admisión desde el servicio técnico de automatización hacia la capa de presentación.
    /// Preserva el Principio de Responsabilidad Única (SRP).
    /// </summary>
    public class ResultadoAdmision
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public double? NotaFinal { get; set; }
        public string EstadoFinal { get; set; } = "Rechazada";
    }
}
