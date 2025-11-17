using LinguaCorp.API.Models;
using LinguaCorp.API.Services;
using Microsoft.AspNetCore.Mvc;


namespace LinguaCorp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhrasesController : Controller
    {

        private readonly IPhraseService _phraseService;

        // The service is automatically injected via dependency injection
        public PhrasesController(IPhraseService phraseService)
        {
            _phraseService = phraseService;
        }


        //Get returns a list of all phrases.
        [HttpGet]
        public IActionResult GetAllPhrases()
        {

            var phrases = _phraseService.GetAllPhrases();


            if (phrases.Count == 0)
            {
                return NoContent();
            }

            return Ok(phrases);

        }

        //Get returns the phrase with the specified if. It is simplified for now, validation will be added later
        [HttpGet("{id}")]
        public IActionResult GetPhraseById(int id)
        {


            if (id <= 0)
            {
                return BadRequest("ID must be a positive integer.");
            }

            var phrase = _phraseService.GetPhraseById(id);

            if (phrase == null)
            {
                return NotFound($"Phrase with ID {id} not found.");
            }

            return Ok(phrase);
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