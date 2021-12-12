using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Extensions.DependencyInjection;

namespace STR.Data.Models.Ef.EfContext
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<STRDbContext>
    {
        public STRDbContext CreateDbContext(string[] args)
        {



            IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            string connectionString = configuration.GetConnectionString("ConnStr");

            //string connStr = "User ID=postgres;password=1234qqqQ;Host=localhost;port=5432;Database=STRDB";

            var builder = new DbContextOptionsBuilder<STRDbContext>();

            builder.UseNpgsql(connectionString);

            return new STRDbContext(builder.Options);

        }
    }
}
