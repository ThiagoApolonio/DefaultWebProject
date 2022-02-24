using DefaultWebProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace DefaultWebProject.Context
{
    public class SBO_TesteContext : DbContext
    {

        public SBO_TesteContext() : base("SBO_Teste")
        {
            Database.Log = d => System.Diagnostics.Debug.WriteLine(d);
            Database.Connection.Open();
        }       

        public DbSet<ItemModel> items { get; set; }
        public DbSet<BlinkModel> BusinessPartner { get; set; }

        public DbSet<JornalVouchersModel> jornalVouchers { get; set; }

        public DbQuery<ClientesUsuariosModel> clientesUsuarios { get; set; }

    }
}