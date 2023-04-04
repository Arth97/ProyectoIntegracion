using Data.DTO;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API_Carga.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargaController : ControllerBase
    {
        private readonly BusquedaDomain busquedaDomain;
        public CargaController(BusquedaDomain busquedaDomain)
        {
            this.busquedaDomain = busquedaDomain;
        }

        /// <summary>
        /// Search Establecimientos Sanitarios into Database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/Busqueda
        ///     {        
        ///              
        ///     }
        /// </remarks>
        /// <returns>List GeoJson</returns>
        /// <response code="200">Return list GeoJson</response>
        /// <response code="404">If no results</response>
        // GET: api/Busqueda
        [HttpGet("Busqueda")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SearchEstablecimientosSanitarios([FromRoute]SearchOptions searchOptions)
        {
            //Logic to delete database
            List<GeoJson> result = busquedaDomain.SearchEstablecimientosSanitarios(searchOptions);
            if (result.Count != 0)
                return Ok(result);
            else
                return NotFound("No results with this criteria");  
        }
    }
}