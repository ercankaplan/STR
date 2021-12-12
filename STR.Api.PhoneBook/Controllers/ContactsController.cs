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

        [HttpPost]
        public async Task<IActionResult> AddContactAsync(Contact model)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid Model");

            var result = await mContactsProvider.AddContactAsync(model);

            return Ok(result.IsSuccess);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactAsync(Guid id)
        {
            var result = await mContactsProvider.DeleteContactAsync(id);

            return Ok(result.IsSuccess);

        }
    }
}
