using Data;
using Data.Model;
using Microsoft.Extensions.Logging;

namespace Repository
{
    public class TipoRepository
    {
        private readonly HealthCenterContext healthCenterContext;

        public TipoRepository(ILogger logger, HealthCenterContext healthCenterContext)
        {
            this.healthCenterContext = healthCenterContext;
        }
        public Tipo GetTipo(int id)
        {
            return healthCenterContext.Tipo.Where(t => t.ID == id).FirstOrDefault();
        }

        public int SetTipo(string tipoDeCentro)
        {
            return tipoDeCentro switch
            {
                "Hospital" => 1,
                "HOSPITALES DE MEDIA Y LARGA ESTANCIA" => 1,
                "HOSPITALES DE SALUD MENTAL Y TRATAMIENTO DE TOXICOMANÍAS" => 1,
                "HOSPITALES ESPECIALIZADOS" => 1,
                "HOSPITALES GENERALES" => 1,
                "Centro de Salud" => 2,
                "Centro de Salud Mental" => 2,
                "Ambulatorio" => 2,
                "Consultorio" => 2,
                "CENTRE SANITARI" => 2,
                "UNITAT BÀSICA" => 2,
                "CENTROS DE SALUD" => 2,
                "CENTROS DE SALUD MENTAL" => 2,
                "CENTROS POLIVALENTES" => 2,
                "CENTROS SANITARIOS INTEGRADOS" => 2,
                "CENTRO/SERVICIO DE URGENCIAS Y EMERGENCIAS" => 2,
                "CENTROS DE CIRUGIA MAYOR AMBULATORIA" => 2,
                "CENTROS DE ESPECIALIDADES" => 2,
                "CONSULTORIOS DE ATENCIÓN PRIMARIA" => 2,
                _ => 3,
            };
        }
    }
}
