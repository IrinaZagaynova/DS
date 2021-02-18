using System.Collections.Generic;

namespace Valuator
{
    public interface IStorage
    {
        void Store(string key, string value);

        void StoreTextKey(string key);

        bool IsTextExist(string text);

        string Load(string key);  
    }
}