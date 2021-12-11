using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using STR.Api.PhoneBook.Interfaces;
using STR.Api.PhoneBook.Models;
using STR.Data.Models.Ef.EfContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.PhoneBook.Providers
{
    public class ContactsProvider : IContactsProvider
    {
        private readonly STRDbContext dbContext;
        private readonly ILogger<ContactsProvider> logger;
        private readonly IMapper mapper;
        public ContactsProvider(STRDbContext dbContext, ILogger<ContactsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;


        }

        public async Task<(bool IsSuccess, string Error)> AddContactAsync(Contact model)
        {
            try
            {
                Data.Models.Ef.Contact contact = mapper.Map<Contact, Data.Models.Ef.Contact>(model);

                contact.Id = Guid.NewGuid();
                contact.CreatedTime = DateTime.Now;

                dbContext.Contact.Add(contact);

                await dbContext.SaveChangesAsync();

                return (true, "Added Contact");
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string Error)> DeleteContactAsync(Guid id)
        {
            try
            {
                Data.Models.Ef.Contact contact = await dbContext.Contact.Where(o => o.Id == id).FirstOrDefaultAsync();

                if (contact == null)
                    return (false, "Not Found");

                dbContext.Contact.Remove(contact);

                return (true, "Deleted Contact");

            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return (false, ex.Message);
            }
        }
    }
}
