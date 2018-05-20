using GridBlockchain.Node.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestCore.Models;

namespace GridBlockchain.Node.Data
{
    public class DataInitializer
    {
        public static async Task Seed(GridBlockchainNodeContext serviceProvider)
        {
            await SeedGenesisBlock(serviceProvider);
            
        }

        private static async Task SeedGenesisBlock(GridBlockchainNodeContext dbContext)
        {
            if (!dbContext.Blocks.Any())
            {
                await dbContext.Blocks.AddAsync(new Block
                {
                    Hash = "1",
                    PreviousBlockHash = "0"
                });
            }

            await dbContext.SaveChangesAsync();
        }

    }
}
