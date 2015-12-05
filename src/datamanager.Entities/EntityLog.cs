using System;
using System.Collections.Generic;

namespace datamanager.Entities
{
	public class EntityLog
	{
		public List<BaseLinkLogEntry> Entries = new List<BaseLinkLogEntry>();

		public bool HasEntries { get { return Entries.Count > 0; } }

		public EntityLog ()
		{
		}

		public void Add(BaseLinkLogEntry entry)
		{
			if (!Exists (entry)) {
				Entries.Add (entry);
			}
		}

		public bool Exists(BaseLinkLogEntry entry)
		{
			foreach (var e in Entries) {
				var key = e.ToString ();
				var key2 = entry.ToString ();
				if (key == key2) {
					return true;
				}
			}

			return false;
		}
	}
}

