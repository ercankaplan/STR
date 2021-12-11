using STR.Api.PhoneBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STR.Api.PhoneBook.Interfaces
{
    public interface IContactsProvider
    {

        /// <summary>
        /// Rehberdeki kişiye iletişim bilgisi ekleme
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        Task<(bool IsSuccess, string Error)> AddContactAsync(Contact model);

        /// <summary>
        /// Rehberdeki kişiden iletişim bilgisi kaldırma
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<(bool IsSuccess, string Error)> DeleteContactAsync(Guid id);

      
    }
}
