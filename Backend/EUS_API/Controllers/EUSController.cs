using EUS_Domain;
using Microsoft.AspNetCore.Mvc;

namespace CV_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EUSController : ControllerBase
    {
        private readonly EUSDomain EUSDomain;

        public EUSController(EUSDomain EUSDomain)
        {
            this.EUSDomain = EUSDomain;
        }

        /// <summary>
        /// Gets Json file.
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     GET api/EUS
        ///     {        
        ///       "FileType": "JSON",        
        ///     }
        /// </remarks>
        /// <returns>A Json file</returns>
        /// <response code="200">Returns the jsonString item</response>
        /// <response code="404">If the item is null</response>
        // GET: api/EUS
        [HttpGet("")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult Get()
        {
            //Logic to get Json
            string result = EUSDomain.GetJson();

            if (result != null)
                return Ok(result);
            else
                return NotFound();
        }
    }
}