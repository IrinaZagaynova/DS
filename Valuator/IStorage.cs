using System.Collections.Generic;

namespace Valuator
{
    public interface IStorage
    {
        void Store(string key, string value);

        void StoreTextKey(string key);
        List<string> GetTextsKeys();

        string Load(string key);  
    }
}