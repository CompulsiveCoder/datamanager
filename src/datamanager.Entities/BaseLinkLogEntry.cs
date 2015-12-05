using System;
using System.Collections.Generic;

namespace datamanager.Entities
{
	public abstract class BaseLinkLogEntry
	{
		public EntityLink Link { get;set; }

		public BaseLinkLogEntry(EntityLink link)
		{
			Link = link;
		}

		public override string ToString ()
		{
			return string.Format ("[{0}: Id={1}]", GetType().Name, Link.Target.Id);
		}
	}

}

