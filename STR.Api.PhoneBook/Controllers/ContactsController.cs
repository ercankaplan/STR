using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
    public class ContactsController : ControllerBase
    {
        private readonly IContactsProvider mContactsProvider;

        public ContactsController(IContactsProvider contactProvider)
        {
            mContactsProvider = contactProvider;
        }

        [HttpPost("Contacts")]
        public async Task<IActionResult> AddContactAsync([FromBody] Contact model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Model");

            var result = await mContactsProvider.AddContactAsync(model);

            return Ok(result);

        }

        [HttpDelete("Contacts/{id}")]
        public async Task<IActionResult> DeleteContactAsync(Guid id)
        {
            var result = await mContactsProvider.DeleteContactAsync(id);

            return Ok(result);

        }
    }
}
