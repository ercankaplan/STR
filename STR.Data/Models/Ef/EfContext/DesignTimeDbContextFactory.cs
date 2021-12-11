using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace STR.Data.Models.Ef.EfContext
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<STRDbContext>
    {
        public STRDbContext CreateDbContext(string[] args)
        {
            string connStr = "User ID=postgres;password=1234qqqQ;Host=localhost;port=5432;Database=STRDB";

            var builder = new DbContextOptionsBuilder<STRDbContext>();

            builder.UseNpgsql(connStr);

            return new STRDbContext(builder.Options);

        }
    }
}
