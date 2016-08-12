using System;

namespace datamanager.Entities
{
    [IndexType(typeof(InheritedEntity))]
    public class DerivedEntity : InheritedEntity
    {
        public DerivedEntity ()
        {
        }
    }
}

