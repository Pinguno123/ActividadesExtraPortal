using System;

namespace ActividadesExtraPortal
{
    public class Asociacion
    {
        public int IdAsociacion { get; set; }
        public string Acronimo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int? AnioFundacion { get; set; }
        public string? Descripcion { get; set; }
        public string? Formulario { get; set; }
        public string? ImgUrl { get; set; }

        public Asociacion() { }

        public Asociacion(int idAsociacion, string acronimo, string nombre, int? anioFundacion, string? descripcion, string? formulario, string? imgUrl)
        {
            IdAsociacion = idAsociacion;
            Acronimo = acronimo;
            Nombre = nombre;
            AnioFundacion = anioFundacion;
            Descripcion = descripcion;
            Formulario = formulario;
            ImgUrl = imgUrl;
        }
    }
}
