using System;

namespace ActividadesExtraPortal
{
    public class SeleccionDeportiva : ActividadExtracurricularBase
    {
        public int IdSeleccion
        {
            get => Id;
            set => Id = value;
        }

        public string NombreEquipo
        {
            get => Nombre;
            set => Nombre = value;
        }

        public string Disciplina { get; set; } = string.Empty;
        public string Rama { get; set; } = string.Empty;
        public string? ImgUrl { get; set; }

        public SeleccionDeportiva() { }

        public SeleccionDeportiva(int idSeleccion, string disciplina, string rama, string nombreEquipo, string? imgUrl, string estado = "Activo")
            : base(idSeleccion, nombreEquipo, $"{disciplina} ({rama})", estado)
        {
            Disciplina = disciplina;
            Rama = rama;
            ImgUrl = imgUrl;
        }

        public override string ObtenerTipoActividad() => "Selección Deportiva";

        public override string ObtenerResumen()
        {
            return $"{NombreEquipo} - Disciplina: {Disciplina} | Rama: {Rama}";
        }
    }
}
