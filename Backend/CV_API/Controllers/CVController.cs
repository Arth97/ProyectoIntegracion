using CV_Domain;
using Microsoft.AspNetCore.Mvc;

namespace CV_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CVController : ControllerBase
    {
        private readonly CVDomain CVDomain;

        public CVController(CVDomain CVDomain)
        {
            this.CVDomain = CVDomain;
        }

        /// <summary>
        /// Gets Json from CSV file.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/CV
        ///     {        
        ///       "FileType": "CSV",        
        ///     }
        /// </remarks>
        /// <returns>A Json from CSV file</returns>
        /// <response code="200">Returns the jsonString item</response>
        /// <response code="404">If the item is null</response>
        // GET: api/IB
        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            //Logic to convert CSV to Json
            string result = CVDomain.ConvertCsvToJson();

            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }
    }
}