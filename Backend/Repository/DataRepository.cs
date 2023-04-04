using Data;
using Data.DTO;
using Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Repository
{
    public class DataRepository
    {
        private readonly HealthCenterContext healthCenterContext;

        public DataRepository(ILogger logger, HealthCenterContext healthCenterContext)
        {
            this.healthCenterContext = healthCenterContext;
        }
        
        //Add @provincia to Database
        public void AddProvincia(Provincia provincia)
        {
            //Check if @provincia already exists in Database
            var resultProvincia = healthCenterContext.Provincia.Where(p => p.Nombre == provincia.Nombre).FirstOrDefault();
            
            //If @provincia not exists, add it to Database
            if (resultProvincia == null)
            {
                healthCenterContext.Provincia.Add(provincia);
                healthCenterContext.SaveChanges();
            }
        }

        //Get @provincia from Database
        public Provincia GetProvincia(Provincia provincia)
        {
            return healthCenterContext.Provincia.Where(p => p.Nombre == provincia.Nombre).FirstOrDefault();
        }

        //Add @localidad to Database
        public void AddLocalidad(Localidad localidad)
        {
            //Check if @localiad already exists in Database
            var resultLocalidad = healthCenterContext.Localidad.Where(l => l.Nombre == localidad.Nombre).FirstOrDefault();
            
            //If @provincia not exists, add it to Database
            if (resultLocalidad == null)
            {
                healthCenterContext.Localidad.Add(localidad);
                healthCenterContext.SaveChanges();
            }
        }

        //Get @localidad from Database
        public Localidad GetLocalidad(Localidad localidad)
        {
            return healthCenterContext.Localidad.Where(p => p.Nombre == localidad.Nombre).FirstOrDefault();
        }

        //Add @establecimientoSanitario to Database
        public void AddEstablecimientoSanitario(EstablecimientoSanitario establecimientoSanitario)
        {
            //Check if @establecimientoSanitario exists in Database
            var resultEstablecimiento = healthCenterContext.EstablecimientoSanitario.Where(e => e.Nombre == establecimientoSanitario.Nombre).FirstOrDefault();
            
            //If @establecimientoSanitario not exists, add it to Database
            if (resultEstablecimiento == null)
            {
                healthCenterContext.EstablecimientoSanitario.Add(establecimientoSanitario);
                healthCenterContext.SaveChanges();
            }
        }

        //Get List<EstablecimientoSanitario> without Latitud && Longitud
        public List<EstablecimientoSanitario> GetEstablecimientoSanitarioNoLatLon()
        {
            return healthCenterContext.EstablecimientoSanitario.Where(e => e.Latitud == null || e.Longitud == null).Include(e => e.Localidad).ThenInclude(l => l.Provincia).ToList();
        }

        //Update @establecimientoSanitario with new data
        public void UpdateEstablecimientoSanitario(EstablecimientoSanitario establecimientoSanitario)
        {
            healthCenterContext.EstablecimientoSanitario.Update(establecimientoSanitario);
            healthCenterContext.SaveChanges();
        }

        //Get List<EstablecimientoSanitario> without CodigoPostal
        public List<EstablecimientoSanitario> GetEstablecimientoSanitarioNoCodPostal()
        {
            return healthCenterContext.EstablecimientoSanitario.Where(e => e.CodigoPostal == null).Include(e => e.Localidad).ThenInclude(l => l.Provincia).ToList();
        }

        //Update Localidad with new data
        public void UpdateLocalidad(Localidad localidadToUpdate, EstablecimientoSanitario establecimientoSanitario)
        {
            //Get @localidad from Database
            Localidad localidad = healthCenterContext.Localidad.Where(l => l.ID == establecimientoSanitario.LocalidadId).FirstOrDefault();
            
            //If @localidad has not CodigoPostal, update @localidad with new data
            if (localidad.CodigoPostal == null)
            {
                healthCenterContext.Localidad.Update(localidadToUpdate);
                healthCenterContext.SaveChanges();
            }
        }

        //Update Provincia with new data
        public void UpdateProvincia(Provincia provinciaToUpdate, Localidad localidad)
        {
            //Get @provincia from Database
            Provincia provincia = healthCenterContext.Provincia.Where(p => p.ID == localidad.ProvinciaId).FirstOrDefault();
           
            //If @provincia has not CodigoPostal, update @provincia with new data
            if (provincia.CodigoPostal == null)
            {
                healthCenterContext.Provincia.Update(provinciaToUpdate);
                healthCenterContext.SaveChanges();
            }
        }

        //Delete database
        public string DeleteDatabase()
        {
            //Delete all data from database and save changes
            healthCenterContext.Database.ExecuteSqlRaw("DELETE [EstablecimientoSanitario]");
            healthCenterContext.Database.ExecuteSqlRaw("DELETE [Localidad]");
            healthCenterContext.Database.ExecuteSqlRaw("DELETE [Provincia]");
            //Save changes 
            healthCenterContext.SaveChanges();


            //Return string if it works
            return "Database deleted correctly";
        }

        public List<EstablecimientoSanitario> SearchEstablecimientosSanitarios(SearchOptions searchOptions)
        {
            List<EstablecimientoSanitario> establecimientosSanitarios = healthCenterContext.EstablecimientoSanitario.Where(e => e.Latitud != null && e.Longitud != null)
                                                                                                                    .Include(e => e.Tipo)
                                                                                                                    .Include(e => e.Localidad)
                                                                                                                    .ThenInclude(l => l.Provincia)
                                                                                                                    .ToList();
            if (searchOptions.Tipo != 0)
                establecimientosSanitarios = establecimientosSanitarios.Where(e => e.TipoId == searchOptions.Tipo).ToList();

            if (searchOptions.Provincia != null)
                establecimientosSanitarios = establecimientosSanitarios.Where(e => e.Localidad.Provincia.Nombre.Contains(searchOptions.Provincia)).ToList();

            if (searchOptions.CodigoPostal != null)
                establecimientosSanitarios = establecimientosSanitarios.Where(e => e.CodigoPostal == searchOptions.CodigoPostal).ToList();

            if (searchOptions.Localidad != null)
                establecimientosSanitarios = establecimientosSanitarios.Where(e => e.Localidad.Nombre.Contains(searchOptions.Localidad)).ToList();

            return establecimientosSanitarios;
        }
    }
}
