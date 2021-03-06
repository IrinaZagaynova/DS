using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using NATS.Client;

namespace Valuator.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IStorage _storage;

        public IndexModel(ILogger<IndexModel> logger, IStorage storage)
        {
            _logger = logger;
            _storage = storage;
        }

        public void OnGet()
        {
            
        }

        public IActionResult OnPost(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return Redirect("/");
            }

            _logger.LogDebug(text);

            string id = Guid.NewGuid().ToString();   
            string textKey = Constants.TextKeyPrefix + id;

            string similarityKey = Constants.SimilarityKeyPrefix + id;
            double similarity = GetSimilarity(text);

            _storage.Store(similarityKey, similarity.ToString());

            _storage.Store(textKey, text);
            _storage.StoreTextKey(textKey);

            CancellationTokenSource cts = new CancellationTokenSource();
            Task.Factory.StartNew(() => CalculateAndStoreRank(cts.Token, id), cts.Token);

            return Redirect($"summary?id={id}");
        }

        static async Task CalculateAndStoreRank(CancellationToken ct, string id)
        {
            ConnectionFactory cf = new ConnectionFactory();

            using (IConnection c = cf.CreateConnection())
            {
                while (!ct.IsCancellationRequested)
                {
                    byte[] data = Encoding.UTF8.GetBytes(id);
                    c.Publish("valuator.processing.rank", data);
                    await Task.Delay(1000);
                }

                c.Drain();
                c.Close();
            }
        }

        private double GetSimilarity(string text)
        {
            var keys = _storage.GetTextKeys();
            
            foreach (var key in keys)
            {
                if (_storage.Load(key) == text)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
