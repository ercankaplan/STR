using Microsoft.Extensions.Logging;
using Moq;
using STR.Api.PhoneBook.Providers;
using STR.Data.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace STR.Test
{
    public class ContactsProviderTests
    {
      
        private readonly Mock<ILogger<ContactsProvider>> _loggerMock = new Mock<ILogger<ContactsProvider>>();

        public ContactsProviderTests()
        {
          
        }

        [Fact]
        public async Task AddContactAsync_ShouldReturnTrue_WhenContactNotExists()
        {
            //Arrange

            Guid contactId = Guid.NewGuid();
            Guid personId = Guid.NewGuid();
            var context = FakeInMemoryDB.GetDbContext();
            context.Person.Add(new Data.Models.Ef.Person() { Id = personId, Name = "Name 1", Surname = "Surname 1", CompanyName = "CompanyName", CreatedTime = DateTime.Now });
            context.SaveChanges();
            ContactsProvider contactsProvider = new ContactsProvider(context, _loggerMock.Object);

            //Act

            var result = await contactsProvider.AddContactAsync(new Api.PhoneBook.Models.Contact() { Id = contactId, PersonId = personId, ContactType = (byte)EnumContactType.Email, ContactInfo = "abc@xyz.com" , CreatedTime = DateTime.Now});

            //Assert

            Assert.True(result);
            Assert.True(context.Contact.Where(o => o.Id == contactId).Any());


        }

        [Fact]
        public async Task AddContactAsync_ShouldReturnFalse_WhenContactExists()
        {
            //Arrange

            Guid contactId = Guid.NewGuid();
            Guid personId = Guid.NewGuid();
            var context = FakeInMemoryDB.GetDbContext();
            context.Person.Add(new Data.Models.Ef.Person() { Id = personId, Name = "Name 1", Surname = "Surname 1", CompanyName = "CompanyName", CreatedTime = DateTime.Now });
            context.Contact.Add(new Data.Models.Ef.Contact() { Id = contactId, PersonId = personId, ContactType = (byte)EnumContactType.Email, ContactInfo = "abc@xyz.com", CreatedTime = DateTime.Now });
            context.SaveChanges();
            ContactsProvider contactsProvider = new ContactsProvider(context, _loggerMock.Object);

            Api.PhoneBook.Models.Contact model = new Api.PhoneBook.Models.Contact() { Id = Guid.NewGuid(), PersonId = Guid.NewGuid(), ContactType = (byte)EnumContactType.Email, ContactInfo = "abc@xyz.com", CreatedTime = DateTime.Now };

            //Act

            var result = await contactsProvider.AddContactAsync(model);

            //Assert

            Assert.False(result);
            Assert.True(context.Contact.Where(o => o.ContactInfo == model.ContactInfo && o.ContactType == model.ContactType).Any());

        }

        [Fact]
        public async Task AddContactAsync_ShouldReturnFalse_WhenPersonNotExists()
        {
            //Arrange
            var context = FakeInMemoryDB.GetDbContext();
            ContactsProvider contactsProvider = new ContactsProvider(context, _loggerMock.Object);

            Api.PhoneBook.Models.Contact model = new Api.PhoneBook.Models.Contact() { Id = Guid.NewGuid(), PersonId = Guid.NewGuid(), ContactType = (byte)EnumContactType.Email, ContactInfo = "abc@xyz.com", CreatedTime = DateTime.Now };

            //Act

            var result = await contactsProvider.AddContactAsync(model);

            //Assert
            Assert.False(result);
            Assert.False(context.Person.Where(o => o.Id == model.PersonId).Any());

        }

        [Fact]
        public async Task DeleteContactAsync_ShouldReturnTrue_WhenContactExists()
        {
            //Arrange

            Guid contactId = Guid.NewGuid();
            Guid personId = Guid.NewGuid();
            var context = FakeInMemoryDB.GetDbContext();
            context.Person.Add(new Data.Models.Ef.Person() { Id = personId, Name = "Name 1", Surname = "Surname 1", CompanyName = "CompanyName", CreatedTime = DateTime.Now });
            context.SaveChanges();
            ContactsProvider contactsProvider = new ContactsProvider(context, _loggerMock.Object);

            //Act

            var result = await contactsProvider.DeleteContactAsync(contactId);

            //Assert
            Assert.False(result);
            Assert.False(context.Contact.Where(o => o.Id == contactId).Any());
        }

        [Fact]
        public async Task DeleteContactAsync_ShouldReturnFalse_WhenContactNotExists()
        {
            //Arrange

            Guid contactId = Guid.NewGuid();
            Guid personId = Guid.NewGuid();
            var context = FakeInMemoryDB.GetDbContext();
            context.Person.Add(new Data.Models.Ef.Person() { Id = personId, Name = "Name 1", Surname = "Surname 1", CompanyName = "CompanyName", CreatedTime = DateTime.Now });
            context.SaveChanges();
            ContactsProvider contactsProvider = new ContactsProvider(context, _loggerMock.Object);

            //Act

            var result = await contactsProvider.DeleteContactAsync(contactId);

            //Assert
            Assert.False(result);
            Assert.False(context.Contact.Where(o => o.Id == contactId).Any());
        }

    }
}
