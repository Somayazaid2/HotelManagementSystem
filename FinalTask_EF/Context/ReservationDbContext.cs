using FinalTask_EF.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalTask_EF.Context
{
    class ReservationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=Reservation-Db;Integrated Security=True;Encrypt=false;");

        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<frontend> frontends { get; set; }
        public virtual DbSet<kitchen> kitchens { get; set; }
    }
}
