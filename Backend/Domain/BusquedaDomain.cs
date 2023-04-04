using Data.DTO;
using Data.Model;
using Repository;

namespace Domain
{
    public class BusquedaDomain
    {
        private readonly DataRepository dataRepository;
        private readonly TipoRepository tipoRepository;

        public BusquedaDomain(DataRepository dataRepository, TipoRepository tipoRepository)
        {
            this.dataRepository = dataRepository;
            this.tipoRepository = tipoRepository;
        }

        public List<GeoJson> SearchEstablecimientosSanitarios(SearchOptions searchOptions)
        {
            List<EstablecimientoSanitario> establecimientosSanitarios = dataRepository.SearchEstablecimientosSanitarios(searchOptions);

            List<GeoJson> geoJsons = new();
            foreach (EstablecimientoSanitario establecimientoSanitario in establecimientosSanitarios)
            {
                GeoJson geoJson = new()
                {
                    Type = "Feature",
                    Geometry = new()
                    {
                        Type = "Point",
                        Coordinates = { [0] = establecimientoSanitario.Latitud, [1] = establecimientoSanitario.Longitud }
                    },
                    Properties = new()
                    {
                        Nombre = establecimientoSanitario.Nombre,
                        Localidad = establecimientoSanitario.Localidad.Nombre,
                        Provincia = establecimientoSanitario.Localidad.Provincia.Nombre,
                        CodigoPostal = establecimientoSanitario.CodigoPostal,
                        Tipo = establecimientoSanitario.Tipo.Nombre
                    }
                };
                geoJsons.Add(geoJson);
            }
            return geoJsons;
        }
    }
}
