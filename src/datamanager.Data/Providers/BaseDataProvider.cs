using System;

namespace datamanager.Data.Providers
{
    public abstract class BaseDataProvider
    {
        public abstract string Get(string key);
        public abstract string[] GetKeys();
        public abstract string[] GetKeys(string pattern);

        public abstract void Set(string key, string value);

        public virtual void Append (string key, string value)
        {
            var existingValue = Get (key);

            var newValue = existingValue += value;

            Set (key, newValue);
        }

        public abstract void Delete(string key);

        public abstract void DeleteAll();

        public abstract void Quit();
    }
}

