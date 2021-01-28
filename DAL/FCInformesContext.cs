using FCInformesSolucion.DAL.Entities;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FCInformesSolucion.DAL
{
    public class FCInformesContext: IdentityDbContext<ApplicationUser>
    {
        public FCInformesContext() 
            : base("FCInformesConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Request>()
                    .HasRequired(c => c.Province)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            modelBuilder.Entity<Request>()
                    .HasRequired(c => c.City)
                    .WithMany()
                    .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }

        public static FCInformesContext Create()
        {
            return new FCInformesContext();
        }

        public DbSet<Menu> Menus { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<ProccesStatus> ProccesStatus { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Configuration> Configurations { get; set; }
        
    }
}