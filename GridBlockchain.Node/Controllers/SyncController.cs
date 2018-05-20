using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GridBlockchain.Node.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using TestCore.Models;

namespace GridBlockchain.Node.Controllers
{
    public class SyncController : Controller
    {
        private IOptions<AppSettings> _settings;

        private readonly GridBlockchainNodeContext _context;

        public SyncController(IOptions<AppSettings> settings, GridBlockchainNodeContext context)
        {
            _settings = settings;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var nodes = _settings.Value.KnownNodes;
            int synced = 0;

            var allNodesBlocks = new List<Block>();

            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Clear();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string url = "api/articles";

            foreach (var node in nodes)
            {
                httpClient.BaseAddress = new Uri(node);

                var response = await httpClient.GetAsync(url);

                response.EnsureSuccessStatusCode();

                string result = await response.Content.ReadAsStringAsync();

                var blocks = JsonConvert.DeserializeObject<List<Block>>(result);

                allNodesBlocks.AddRange(blocks);
            }

            var uniqueArticles = allNodesBlocks.Distinct();

            foreach (var block in allNodesBlocks)
            {
                if (!BlockExists(block.Hash))
                {
                    _context.Blocks.Add(block);
                    await _context.SaveChangesAsync();

                    synced++;
                }
            }

            return View(synced);
        }

        private bool BlockExists(string id)
        {
            return _context.Blocks.Any(e => e.Hash == id);
        }
    }
}