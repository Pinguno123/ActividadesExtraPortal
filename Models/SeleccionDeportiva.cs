using System;

namespace ActividadesExtraPortal
{
    public class SeleccionDeportiva
    {
        public int IdSeleccion { get; set; }
        public string Disciplina { get; set; } = string.Empty;
        public string Rama { get; set; } = string.Empty;
        public string NombreEquipo { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }

        public SeleccionDeportiva() { }

        public SeleccionDeportiva(int idSeleccion, string disciplina, string rama, string nombreEquipo, string? imgUrl)
        {
            IdSeleccion = idSeleccion;
            Disciplina = disciplina;
            Rama = rama;
            NombreEquipo = nombreEquipo;
            ImgUrl = imgUrl;
        }
    }
}
