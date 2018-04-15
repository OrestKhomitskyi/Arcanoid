using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Arcanoid.Models
{
    class ModelContext : DbContext
    {
        public ModelContext() : base("Codefirst.Properties.Settings.conn") { }
        public ModelContext(string conn) : base(conn) { }

        public DbSet<Player> Students { get; set; }
    }
}

