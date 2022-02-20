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
        private readonly IMapper mapperContactDb2VM;
        private readonly IMapper mapperContactVM2Db;
        private readonly IMapper mapperPersonDb2VM;
        private readonly IMapper mapperPersonVM2Db;
        public PersonsProvider(STRDbContext dbContext, ILogger<PersonsProvider> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;

            mapperContactDb2VM = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.Models.Ef.Contact, Contact>();
            }).CreateMapper();

            mapperContactVM2Db = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Contact, Data.Models.Ef.Contact>();
            }).CreateMapper();


            this.mapperPersonDb2VM = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.Models.Ef.Person, Person>()
                 .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => mapperContactDb2VM.Map<List<Data.Models.Ef.Contact>, List<Contact>>(src.Contacts.ToList())));
            }).CreateMapper();

            this.mapperPersonVM2Db = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Person, Data.Models.Ef.Person>()
                 .ForMember(dest => dest.Contacts, opt => opt.MapFrom(src => mapperContactVM2Db.Map<List<Contact>, List<Data.Models.Ef.Contact>>(src.Contacts.ToList())));
            }).CreateMapper();
        }

        public async Task<bool> AddPersonAsync(Person model)
        {
            try
            {

                if (dbContext.Person.Where(x => x.Name == model.Name && x.Surname == model.Surname).Any())
                    throw new ApplicationException("Person is already exists");

                Data.Models.Ef.Person person = mapperPersonVM2Db.Map<Person, Data.Models.Ef.Person>(model);

                person.CreatedTime = DateTime.Now;
                person.Id = Guid.NewGuid();

                dbContext.Person.Add(person);


                if (person.Contacts != null && person.Contacts.Any())
                {

                    foreach (var contact in person.Contacts.ToList())
                    {
                        contact.Id = Guid.NewGuid();
                        contact.PersonId = person.Id;
                        contact.CreatedTime = DateTime.Now;

                        dbContext.Contact.Add(contact);
                    }

                }

                await dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return false;
            }
        }

        public async Task<bool> DeletePersonAsync(Guid id)
        {
            try
            {
                Data.Models.Ef.Person person = await dbContext.Person.Where(o => o.Id == id).FirstOrDefaultAsync();

                if (person == null)
                    return false;

                dbContext.Person.Remove(person);

                await dbContext.SaveChangesAsync();

                return true;

            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return false;
            }
        }

        public async Task<Person> GetPersonAsync(Guid id)
        {
            try
            {
                var person = await dbContext.Person.Where(x => x.Id == id)
                                                    .Include(o => o.Contacts)
                                                    .FirstOrDefaultAsync();

                if (person != null)
                {
                    logger?.LogInformation("Person Found");

                    Person result = mapperPersonDb2VM.Map<Data.Models.Ef.Person, Person>(person);
                    return result;
                }

                return null;
            }
            catch (Exception ex)
            {

                logger?.LogError(ex.StackTrace);
                return null;
            }
        }

        public async Task<IEnumerable<Person>> GetPersonsAsync()
        {
            try
            {
                var persons = await dbContext.Person.ToListAsync();

                if (persons.Any())
                {
                    logger?.LogInformation("Persons Found");

                    var result = mapperPersonDb2VM.Map<List<Data.Models.Ef.Person>, List<Person>>(persons);

                    return  result;

                }

                return new List<Person>();
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.StackTrace);
                return new List<Person>();
            }
        }
    }
}
