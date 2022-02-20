using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using STR.Api.PhoneBook.Interfaces;
using STR.Api.PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.PhoneBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonsProvider mPersonsProvider;

        public PersonsController(IPersonsProvider personProvider)
        {
            mPersonsProvider = personProvider;
        }

        [HttpPost("Persons")]
        [AllowAnonymous]
        public async Task<IActionResult> AddPersonAsync([FromBody]Person model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Model");

            var result = await mPersonsProvider.AddPersonAsync(model);

            return Ok(result);
        }

        [HttpDelete("Persons/{id}")]
        public async Task<IActionResult> DeletePersonAsync(Guid id)
        {
            var result = await mPersonsProvider.DeletePersonAsync(id);

            return Ok(result);
        }

        [HttpGet("Persons")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPersonsAsync()
        {
            var result = await mPersonsProvider.GetPersonsAsync();

            if (result!=null)
                return Ok(result);

            return NotFound();
        }

        [HttpGet("Persons/{id}")]
        public async Task<IActionResult> GetPersonAsync(Guid id)
        {
            var result = await mPersonsProvider.GetPersonAsync(id);

            if (result!=null)
                return Ok(result);

            return NotFound();
        }
    }
}
