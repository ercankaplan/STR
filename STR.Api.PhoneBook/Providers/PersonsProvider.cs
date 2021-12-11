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
    public class PersonsProvider : IPersonsProvider
    {

        private readonly STRDbContext dbContext;
        private readonly ILogger<PersonsProvider> logger;
        private readonly IMapper mapper;
        public PersonsProvider(STRDbContext dbContext, ILogger<PersonsProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;


        }

        public  async Task<(bool IsSuccess, string Error)> AddPersonAsync(Person model)
        {
            try
            {
                Data.Models.Ef.Person person = mapper.Map<Person, Data.Models.Ef.Person>(model);

                person.CreatedTime = DateTime.Now;
                person.Id = Guid.NewGuid();
  
                dbContext.Person.Add(person);
               

                if (model.Contacts.Any())
                {
                    ICollection<Data.Models.Ef.Contact> contacts = mapper.Map<ICollection<Contact>, ICollection<Data.Models.Ef.Contact>>(model.Contacts);

                    foreach (var contact in contacts.ToList())
                    {
                        contact.Id = Guid.NewGuid();
                        contact.PersonId = person.Id;
                        contact.CreatedTime = DateTime.Now;

                        dbContext.Contact.Add(contact);
                    }
                    
                }

                await dbContext.SaveChangesAsync();

                return (true, "Added Person");
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, string Error)> DeletePersonAsync(Guid id)
        {
            try
            {
                Data.Models.Ef.Person person = await dbContext.Person.Where(o => o.Id == id).FirstOrDefaultAsync();

                if (person == null)
                    return (false, "Not Found");

                dbContext.Person.Remove(person);

                return (true, "Deleted Person");

            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return (false, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, Person Person, string Error)> GetPersonAsync(Guid id)
        {
            try
            {
                var person = await dbContext.Person.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (person != null)
                {
                    logger?.LogInformation("ReportRequest Found");

                    Person result = mapper.Map<Data.Models.Ef.Person, Person>(person);
                    return (true, result, null);
                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return (false, null, ex.Message);
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Person> Persons, string Error)> GetPersonsAsync()
        {
            try
            {
                var persons = await dbContext.Person.ToListAsync();

                if (persons.Any())
                {
                    logger?.LogInformation("Persons Found");

                    var result = mapper.Map<List<Data.Models.Ef.Person>, List<Person>>(persons);

                    return (true, result, null);

                }

                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.StackTrace);
                return (false, null, ex.Message);
            }
        }
    }
}
