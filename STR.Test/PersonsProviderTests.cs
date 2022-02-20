using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using STR.Api.PhoneBook.Providers;
using STR.Data.Models.Ef;
using STR.Data.Models.Ef.EfContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace STR.Test
{

    public class PersonsProviderTests
    {
        private readonly PersonsProvider _sut;
        private readonly STRDbContext dbContext;
        private readonly Mock<ILogger<PersonsProvider>> _loggerMock = new Mock<ILogger<PersonsProvider>>();


        public PersonsProviderTests()
        {
            var options = new DbContextOptionsBuilder<STRDbContext>()
           .UseInMemoryDatabase(databaseName: "STRDBMock")
           .Options;


            dbContext = new STRDbContext(options);


            // Insert seed data into the database using one instance of the context

            dbContext.Person.Add(new Person { Id = Guid.NewGuid(), Name = "Name 1", Surname = "Surname 1", CompanyName = "CompanyName 1", CreatedTime = DateTime.Now });
            dbContext.Person.Add(new Person { Id = Guid.NewGuid(), Name = "Name 2", Surname = "Surname 2", CompanyName = "CompanyName 1", CreatedTime = DateTime.Now });
            dbContext.Person.Add(new Person { Id = Guid.NewGuid(), Name = "Name 3", Surname = "Surname 3", CompanyName = "CompanyName 1", CreatedTime = DateTime.Now });
            dbContext.SaveChanges();

            _sut = new PersonsProvider(dbContext, _loggerMock.Object);

        }

        [Fact]
        public async Task GetPersonsAsync_ShouldReturnAllPerson_WhenPersonsExists()
        {
            //Arrange
            var context = FakeInMemoryDB.GetDbContext();
            context.Person.Add(new Person { Id = Guid.NewGuid(), Name = "Name 1", Surname = "Surname 1", CompanyName = "CompanyName 1", CreatedTime = DateTime.Now });
            context.Person.Add(new Person { Id = Guid.NewGuid(), Name = "Name 2", Surname = "Surname 2", CompanyName = "CompanyName 1", CreatedTime = DateTime.Now });
            context.Person.Add(new Person { Id = Guid.NewGuid(), Name = "Name 3", Surname = "Surname 3", CompanyName = "CompanyName 1", CreatedTime = DateTime.Now });
            context.SaveChanges();
            PersonsProvider personsProvider = new PersonsProvider(context, _loggerMock.Object);
            //Act
            var personList = await personsProvider.GetPersonsAsync();
            
            //Assert
            
            Assert.NotEmpty(personList);
            Assert.True(personList.Count() > 0);
            Assert.Equal(context.Person.Count(), personList.ToList().Count());
            
            
        }

        [Fact]
        public async Task GetPersonsAsync_ShouldReturnEmptyPersonList_WhenAnyPersonsNotExists()
        {
            //Arrange
            var context = FakeInMemoryDB.GetDbContext();
            PersonsProvider personsProvider = new PersonsProvider(context, _loggerMock.Object);
            //Act
            var personList = await personsProvider.GetPersonsAsync();
            //Assert
            Assert.Empty(personList);
        }

        [Fact]
        public async Task GetPersonAsync_ShouldReturnPerson_WhenPersonExists()
        {
            //Arrange

            var personId = Guid.NewGuid();
            Person person = new Person { Id = personId, Name = "Name x", Surname = "Surname x", CompanyName = "CompanyName x", CreatedTime = DateTime.Now };

            var context = FakeInMemoryDB.GetDbContext();
            context.Add(person);
            context.SaveChanges();

            PersonsProvider personsProvider = new PersonsProvider(context, _loggerMock.Object);

            //Act

            var result = await personsProvider.GetPersonAsync(personId);


            //Assert
            Assert.NotNull(result);
            Assert.Equal(personId.ToString(), result.Id.ToString());
        }


        [Fact]
        public async Task GetPersonAsync_ShouldReturnNull_WhenPersonNotExists()
        {
            //Arrange
            var personId = Guid.NewGuid();
            //Act
            var person = await _sut.GetPersonAsync(personId);
            //Assert
            Assert.Null(person);
        }

        [Fact]
        public async Task AddPersonAsync_ShouldReturnTrue_WhenPersonNotExists()
        {

            //Arrange

            //Act

            var personId = Guid.NewGuid();

            var result = await _sut.AddPersonAsync(new Api.PhoneBook.Models.Person() { Id = personId, Name = "Name x", Surname = "Surname x", CompanyName = "CompanyName x", CreatedTime = DateTime.Now });

            //Assert

            Assert.True(result);
        }

        [Fact]
        public async Task AddPersonAsync_ShouldReturnFalse_WhenPersonExists()
        {
            //Arrange

            var personId = Guid.NewGuid();
            Person person = new Person { Id = personId, Name = "Name x", Surname = "Surname x", CompanyName = "CompanyName x", CreatedTime = DateTime.Now };

            var context = FakeInMemoryDB.GetDbContext();
            context.Add(person);
            context.SaveChanges();

            PersonsProvider personsProvider = new PersonsProvider(context, _loggerMock.Object);

            //Act


            var id = Guid.NewGuid();

            var result = await personsProvider.AddPersonAsync(new Api.PhoneBook.Models.Person() { Id = id, Name = "Name x", Surname = "Surname x", CompanyName = "CompanyName x", CreatedTime = DateTime.Now });


            //Assert

            Assert.False(result);
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldReturnTrue_WhenPersonExists()
        {
            //Arrange
            var personId = Guid.NewGuid();
            Person person = new Person { Id = personId, Name = "Name x", Surname = "Surname x", CompanyName = "CompanyName x", CreatedTime = DateTime.Now };

            var context = FakeInMemoryDB.GetDbContext();
            context.Add(person);
            context.SaveChanges();

            //Act
            PersonsProvider personsProvider = new PersonsProvider(context, _loggerMock.Object);
            var deleteResult = await personsProvider.DeletePersonAsync(personId);
            //Assert
            Assert.True(deleteResult);
            Assert.True(context.Person.Where(o => o.Id == personId).Count() == 0);
        }

        [Fact]
        public async Task DeletePersonAsync_ShouldReturnFalse_WhenPersonNotExists()
        {
            //Arrange
            var personId = Guid.NewGuid();
            //Act
            var deleteResult = await _sut.DeletePersonAsync(personId);
            //Assert
            Assert.False(deleteResult);
        }


    }


}

