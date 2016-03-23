using System;
using System.Collections.Generic;
using Sider;

namespace datamanager.Data
{
	public class DataTypeManager
	{
		public DataKeys Keys { get;set; }

		public BaseRedisClientWrapper Client { get;set; }

		public DataTypeManager (DataKeys keys, BaseRedisClientWrapper client)
		{
			if (keys == null)
				throw new ArgumentNullException ("keys");
			
			if (client == null)
				throw new ArgumentNullException ("client");
			
			Keys = keys;
			Client = client;
		}

		public string[] GetTypeNames()
		{
			var typesKey = Keys.GetTypesKey ();

			var typesString = Client.Get (typesKey);

			var typeNames = new List<string>();

			if (!String.IsNullOrEmpty (typesString)) {
				var typeDefinitionStrings = typesString.Split (';');
				foreach (var typeDefinitionString in typeDefinitionStrings) {
					if (!String.IsNullOrEmpty (typeDefinitionString)) {
						var parts = typeDefinitionString.Split ('=');
						var typeName = parts [0];
						typeNames.Add (typeName);
					}
				}
			}

			return typeNames.ToArray();
		}

		public Dictionary<string, string> GetTypes()
		{
			var typesKey = Keys.GetTypesKey ();

			var typesString = Client.Get (typesKey);

			var typeDefinitions = new Dictionary<string, string> ();

			if (!String.IsNullOrEmpty (typesString)) {
				var typeDefinitionStrings = typesString.Split (';');
				foreach (var typeDefinitionString in typeDefinitionStrings) {
					if (!String.IsNullOrEmpty (typeDefinitionString)
						&& typeDefinitionString.Contains("=")) {
						var parts = typeDefinitionString.Split ('=');
						var typeName = parts [0];
						var typeFullName = parts [1];
						typeDefinitions.Add (typeName, typeFullName);
					}
				}
			}

			return typeDefinitions;
		}

		public void Add(string typeName, string typeFullName)
		{
			var types = GetTypes();

			if (!types.ContainsKey (typeName))
				types.Add (typeName, typeFullName);

			SetTypes (types);
		}

		public void SetTypes(Dictionary<string, string> typeNames)
		{
			var typesKey = Keys.GetTypesKey ();
			var typesString = String.Join (",", typeNames);
			Client.Set(typesKey, typesString);
		}

		public bool Exists(string typeName)
		{
			var types = GetTypes ();
			return types.ContainsKey (typeName)
			|| types.ContainsValue (typeName);
		}

		public void EnsureExists(Type type)
		{
			EnsureExists (type.Name, type.FullName);
		}

		public void EnsureExists(string typeName, string typeFullName)
		{
			if (!Exists (typeName)) {
				Add (typeName, typeFullName);
			}
		}

		public Type GetType(string typeName)
		{
			var types = GetTypes ();
			if (types.ContainsKey (typeName)) {
				var typeFullName = types [typeName];			
				return Type.GetType (typeFullName);
			} else
				throw new InvalidOperationException ("The type '" + typeName + "' was not found.");
		}

	}
}

