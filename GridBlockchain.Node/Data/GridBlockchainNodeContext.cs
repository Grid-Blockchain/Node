using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TestCore.Models;

namespace GridBlockchain.Node.Models
{
    public class GridBlockchainNodeContext : DbContext
    {
        public GridBlockchainNodeContext (DbContextOptions<GridBlockchainNodeContext> options)
            : base(options)
        {
        }

        public DbSet<Block> Blocks { get; set; }
    }
}
