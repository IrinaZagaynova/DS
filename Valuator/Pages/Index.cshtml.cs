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

        private static double GetRank(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return 0;
            }

            int lettersCount = text.Count(char.IsLetter);

            return Math.Round(((text.Length - lettersCount) / (double)text.Length), 3);
        }

        private int GetSimilarity(string text)
        {
            Dictionary<string, string> data = _storage.LoadAll();

            foreach (var item in data)
            {
                if (item.Key.Substring(0, 4) == "TEXT-" && item.Value == text)
                {
                    return 1;
                }
            }

            return 0;
        }
    }
}
