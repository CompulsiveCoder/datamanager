using System;

namespace datamanager.Entities
{
    /// <summary>
    /// Tells the data manager to index the entity according to the specified base type. This enables Get queries.
    /// </summary>
    public class IndexTypeAttribute : Attribute
    {
        public Type IndexType;

        public IndexTypeAttribute (Type indexType)
        {
            IndexType = indexType;
        }
    }
}

