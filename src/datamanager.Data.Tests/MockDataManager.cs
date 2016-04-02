using System;

namespace datamanager.Data.Tests
{
    public class MockDataManager : DataManager
    {
        public MockDataManager () : base(new MockRedisClientWrapper())
        {
        }
    }
}

