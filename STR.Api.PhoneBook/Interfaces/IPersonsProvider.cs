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
        Task<bool> AddPersonAsync(Person model);

        /// <summary>
        /// Rehberde kişi kaldırma
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeletePersonAsync(Guid id);

        /// <summary>
        /// Rehberdeki kişilerin listelenmesi
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Person>> GetPersonsAsync();

        /// <summary>
        /// Rehberdeki bir kişiyle ilgili iletişim bilgilerinin de yer aldığı detay bilgilerin getirilmesi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Person> GetPersonAsync(Guid id);
    }
}
