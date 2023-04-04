using Data.DTO;
using Domain;
using Microsoft.AspNetCore.Mvc;

namespace API_Carga.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CargaController : ControllerBase
    {
        private readonly CVDomain cVDomain;
        private readonly EUSDomain eUSDomain;
        private readonly IBDomain iBDomain;
        private readonly DeleteDomain deleteDomain;
        private readonly ScraperDomain scraperDomain;

        public CargaController(CVDomain cVDomain, EUSDomain eUSDomain, IBDomain iBDomain, DeleteDomain deleteDomain, ScraperDomain scraperDomain)
        {
            this.cVDomain = cVDomain;
            this.eUSDomain = eUSDomain;
            this.iBDomain = iBDomain;
            this.deleteDomain = deleteDomain;
            this.scraperDomain = scraperDomain;
        }

        /// <summary>
        /// Delete database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     DELETE api/Delete
        ///     {        
        ///              
        ///     }
        /// </remarks>
        /// <returns>Response</returns>
        /// <response code="200">Returns response</response>
        /// <response code="500">Internal Server Error, can not delete the database</response>
        // DELETE: api/Delete
        [HttpDelete("Delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteDatabase()
        {
            //Logic to delete database
            try
            {
                string result = deleteDomain.DeleteDatabase();
                return Ok(result);
            }
            catch (Exception e)
            {
                return StatusCode(500, e);
            }
        }
        
        /// <summary>
        /// Store data into Database.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/LoadData
        ///     {        
        ///         "IlasBaleares": true,
        ///         "ComunidadValencia: true,
        ///         "Euskadi": false
        ///     }
        /// </remarks>
        /// <returns>Response and sring with errors</returns>
        /// <response code="200">Returns response with errors</response>
        /// <response code="500">Internal Server Error, can not store data into database</response>
        // GET: api/LoadData
        [HttpGet("LoadData")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult StoreCVData([FromRoute]LoadOptions loadOptions)
        {
            //Logic to store CV data into database
            try
            {
                string? result = null;
                if (loadOptions.ComunidadValenciana)
                    result = cVDomain.StoreJson();
                if (loadOptions.Euskadi)
                    result += result + eUSDomain.StoreJson();
                if (loadOptions.IslasBaleares)
                    result += iBDomain.StoreJson();
                string IncorrectData = scraperDomain.ScrapingData();
                return Ok(result + IncorrectData);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Error - data not processed correctly");
            }
        }
    }
}