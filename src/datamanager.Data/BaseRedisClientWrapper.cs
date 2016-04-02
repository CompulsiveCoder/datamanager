using System;

namespace datamanager.Data
{
	public abstract class BaseRedisClientWrapper
	{
		public abstract string Get(string key);

		public abstract void Set(string key, string value);

		public abstract void Del(string key);

		public abstract void Quit();

        public virtual void Append (string key, string value)
        {
            var existingValue = Get (key);

            var newValue = existingValue += value;

            Set (key, newValue);
        }

		public abstract void FlushAll();
	}
}

