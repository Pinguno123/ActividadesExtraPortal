using System;

namespace ActividadesExtraPortal
{
    public class Asociacion : ActividadExtracurricularBase
    {
        public int IdAsociacion
        {
            get => Id;
            set => Id = value;
        }

        public string Acronimo { get; set; } = string.Empty;
        public int? AnioFundacion { get; set; }
        public string? Formulario { get; set; }
        public string? ImgUrl { get; set; }

        public Asociacion() { }

        public Asociacion(int idAsociacion, string acronimo, string nombre, int? anioFundacion, string? descripcion, string? formulario, string? imgUrl, string estado = "Activo")
            : base(idAsociacion, nombre, descripcion, estado)
        {
            Acronimo = acronimo;
            AnioFundacion = anioFundacion;
            Formulario = formulario;
            ImgUrl = imgUrl;
        }

        public override string ObtenerTipoActividad() => "Asociación Estudiantil";

        public override string ObtenerResumen()
        {
            string fundacion = AnioFundacion.HasValue ? AnioFundacion.Value.ToString() : "N/A";
            return $"{Nombre} ({Acronimo}) - Año Fundación: {fundacion}";
        }
    }
}
