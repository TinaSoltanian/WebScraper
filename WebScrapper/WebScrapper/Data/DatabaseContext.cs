using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using WebScrapper.Models;

namespace WebScrapper.Data
{
    public class DatabaseContext: DbContext
    {

        public DatabaseContext() : base("DatabaseContext")
        {
        }

        public DbSet<Request> Requests { get; set; }
        public DbSet<Response> Responses { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}