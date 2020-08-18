using System;
using System.Data.Entity;
using Shared.Core.Data;

namespace CFData
{
    public class Entities : DbContextBase
    {
        public virtual DbSet<KeyValues> KeyValues { get; set; }

        public Entities() : base("name=Entities")
        {
        }

        public string ConnectionADOString()
        {
            throw new NotImplementedException();
        }
    }
}
