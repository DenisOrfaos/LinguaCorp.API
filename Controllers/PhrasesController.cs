using LinguaCorp.API.Models;
using LinguaCorp.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace LinguaCorp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhrasesController : Controller
    {

        private readonly IPhraseService _phraseService;

        // The service is automatically injected via dependency injection

        //logging service dependency injection:      
        private readonly ILogger<PhrasesController> _logger;

        // Constructor:
        public PhrasesController(ILogger<PhrasesController> logger, IPhraseService phraseService)
        {
            _logger = logger;
            _phraseService = phraseService;
        }




        /// <summary>
        /// Retrieves all stored phrases.
        /// </summary>
        /// <returns>List of phrases or 204 if empty.</returns>
        [HttpGet]
        public IActionResult GetAllPhrases()
        {


            try
            {
                var phrases = _phraseService.GetAllPhrases();

                if (phrases.Count == 0)
                {
                    return NoContent();
                }

                return Ok(phrases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all phrases.");
                return StatusCode(500, "An error occurred while retrieving phrases.");
            }


        }


        /// <summary>
        /// Retrieves a specific phrase by its unique identifier.
        /// </summary>
        /// <param name="id">Phrase ID</param>
        /// <returns>Returns the phrase if found.</returns>
        [HttpGet("{id}")]
        public IActionResult GetPhraseById(int id)
        {


            _logger.LogInformation("Request received to get phrase with ID {Id}", id);


            if (id <= 0)
            {
                _logger.LogWarning("Invalid ID {Id} provided", id);
                return BadRequest("ID must be a positive integer.");
            }


            try
            {
                var phrase = _phraseService.GetPhraseById(id);

                _logger.LogInformation("Phrase with ID {Id} retrieved successfully", id);
                return Ok(phrase);
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving phrase with ID {Id}", id);
                return StatusCode(500, "An error occurred while retrieving the phrase.");
            }

        }


        [HttpPost]
        public IActionResult Create([FromBody] Phrase phrase)
        {
            //This verifies if the Model is filled correctly. If not, the error message that was defined is returned
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var created = _phraseService.CreatePhrase(phrase);

            return CreatedAtAction(nameof(GetPhraseById), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePhrase(int id, [FromBody] Phrase updatedPhrase)
        {
            //This verifies if the Model is filled correctly. If not, the error message that was defined is returned
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var success = _phraseService.UpdatePhrase(id, updatedPhrase);
            if (!success)
            {
                return NotFound($"Phrase with ID {id} not found.");
            }

            return Ok(updatedPhrase);

        }

        [HttpDelete("{id}")]
        public IActionResult DeletePhrase(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID must be a positive integer.");
            }

            var success = _phraseService.DeletePhrase(id);

            if (!success)
            {
                return NotFound($"Phrase with ID {id} not found.");
            }

            return NoContent();
        }

    }
}