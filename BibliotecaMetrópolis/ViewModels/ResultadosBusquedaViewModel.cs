using BibliotecaMetrópolis.Models;

namespace BibliotecaMetrópolis.ViewModels
{
    public class ResultadosBusquedaViewModel
    {
        public List<Recurso> Resultados { get; set; } = new List<Recurso>();
        public BusquedaViewModel TerminosBusqueda { get; set; } = new BusquedaViewModel();
        public int TotalResultados { get; set; }
    }
}