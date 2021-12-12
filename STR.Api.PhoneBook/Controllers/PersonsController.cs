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

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddPersonAsync(Person model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Model");

            var result = await mPersonsProvider.AddPersonAsync(model);

            return Ok(result.IsSuccess);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePersonAsync(Guid id)
        {
            var result = await mPersonsProvider.DeletePersonAsync(id);

            return Ok(result.IsSuccess);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPersonsAsync()
        {
            var result = await mPersonsProvider.GetPersonsAsync();

            if (result.IsSuccess)
                return Ok(result.Persons);

            return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonAsync(Guid id)
        {
            var result = await mPersonsProvider.GetPersonAsync(id);

            if (result.IsSuccess)
                return Ok(result.Person);

            return NotFound();
        }
    }
}
