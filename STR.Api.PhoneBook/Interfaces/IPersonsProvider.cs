using STR.Api.PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.PhoneBook.Interfaces
{
    public interface IPersonsProvider
    {
      
        /// <summary>
        /// Rehberde kişi oluşturma
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task<(bool IsSuccess, string Error)> AddPersonAsync(Person model);

        /// <summary>
        /// Rehberde kişi kaldırma
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(bool IsSuccess, string Error)> DeletePersonAsync(Guid id);

        /// <summary>
        /// Rehberdeki kişilerin listelenmesi
        /// </summary>
        /// <returns></returns>
        Task<(bool IsSuccess, IEnumerable<Person> Persons, string Error)> GetPersonsAsync();

        /// <summary>
        /// Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(bool IsSuccess, Person Person, string Error)> GetPersonAsync(Guid id);
    }
}
