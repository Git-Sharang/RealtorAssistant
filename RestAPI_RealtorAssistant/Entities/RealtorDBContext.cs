using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI_RealtorAssistant.Entities
{
    public partial class RealtorDBContext : DbContext
    {
        public RealtorDBContext()
        {
        }

        public RealtorDBContext(DbContextOptions<RealtorDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Realtors> Realtor { get; set; }
        public virtual DbSet<Asset> Asset { get; set; }
    }
}
