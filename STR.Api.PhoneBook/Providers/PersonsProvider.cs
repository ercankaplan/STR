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

        public async Task<(bool IsSuccess, string Error)> AddPersonAsync(Person model)
        {
            try
            {
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

                await dbContext.SaveChangesAsync();

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
                var person = await dbContext.Person.Where(x => x.Id == id)
                                                    .Include(o => o.Contacts)
                                                    .FirstOrDefaultAsync();

                if (person != null)
                {
                    logger?.LogInformation("Person Found");

                    Person result = mapperPersonDb2VM.Map<Data.Models.Ef.Person, Person>(person);
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

                    var result = mapperPersonDb2VM.Map<List<Data.Models.Ef.Person>, List<Person>>(persons);

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
