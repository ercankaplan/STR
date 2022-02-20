using Microsoft.EntityFrameworkCore;
using STR.Data.Models.Ef.EfContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace STR.Test
{
    public static class FakeInMemoryDB
    {
        public static STRDbContext GetDbContext()
        {

            var options = new DbContextOptionsBuilder<STRDbContext>()
          .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
          .Options;


            return new STRDbContext(options);
        }
    }
}
