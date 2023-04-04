using IB_Domain;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IB_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class IBController : ControllerBase
    {
        private readonly IBDomain IBDomain;

        public IBController(IBDomain IBDomain)
        {
            this.IBDomain = IBDomain;
        }

        /// <summary>
        /// Gets Json from XML file.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/IB
        ///     {        
        ///       "FileType": "XML",        
        ///     }
        /// </remarks>
        /// <returns>A Json from XML file</returns>
        /// <response code="200">Returns the jsonString item</response>
        /// <response code="404">If the item is null</response>
        // GET: api/IB
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            //Logic to convert XML to Json
            string result = IBDomain.ConvertXmlToJson();

            if (result != null)
                return Ok(result);
            else 
                return NotFound();
        }
    }
}