using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

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

            string rankKey = "RANK-" + id;
            string rank = GetRank(text).ToString();

            _storage.Store(rankKey, rank);

            string similarityKey = "SIMILARITY-" + id;
            string similarity = GetSimilarity(text).ToString();

            _storage.Store(similarityKey, similarity);

            string textKey = "TEXT-" + id;     

            _storage.Store(textKey, text);

            return Redirect($"summary?id={id}");
        }

        private double GetRank(string text)
        {
            int lettersCount = text.Count(char.IsLetter);

            return Math.Round(((text.Length - lettersCount) / (double)text.Length), 3);
        }

        private int GetSimilarity(string text)
        {
            List<string> keys = _storage.GetKeys();

            foreach (var key in keys)
            {
                if (key.Substring(0, 5) == "TEXT-" && _storage.Load(key) == text)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
