using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using datamanager.Data.Providers;

namespace datamanager.Data
{
	public class DataTypeManager
	{
        public TypeNamesParser TypeNamesParser = new TypeNamesParser();

        public string TypesKey = "Types";

        public DataManagerSettings Settings { get;set; }

        public BaseDataProvider Provider { get;set; }

        public DataTypeManager (DataManagerSettings settings, BaseDataProvider provider)
        {
            if (settings == null)
                throw new ArgumentNullException ("settings");
            
            if (provider == null)
                throw new ArgumentNullException ("provider");

            Settings = settings;

            Provider = provider;
		}

		public string[] GetTypeNames()
		{
            return (from type in GetTypes ().Keys
                select type).ToArray();
		}

		public Dictionary<string, string> GetTypes()
        {
            if (Settings.IsVerbose) {
                Console.WriteLine ("    Getting types");
            }

            var fullTypesKey = Settings.Prefix + "-" + TypesKey;

            var typesString = Provider.Get (fullTypesKey);

            if (Settings.IsVerbose) {
                Console.WriteLine ("      Types key: " + fullTypesKey);
                Console.WriteLine ("      Types string: " + typesString);
            }

            var typeDefinitions = TypeNamesParser.ParseTypeDefinitions (typesString);

            if (Settings.IsVerbose) {
                Console.WriteLine ("      Definitions found: " + typeDefinitions.Count);
            }

            return typeDefinitions;
		}

		public void Add(Type type)
		{
            Add (type.Name, type.AssemblyQualifiedName);
		}

		public void Add(string typeName, string typeFullName)
		{
			var types = GetTypes();

			if (!types.ContainsKey (typeName))
				types.Add (typeName, typeFullName);

			SetTypes (types);
		}

		public void SetTypes(Dictionary<string, string> typeDefinitions)
        {
            var typesString = TypeNamesParser.CompileTypeDefinitions (typeDefinitions);

            Provider.Set(Settings.Prefix + "-" + TypesKey, typesString);
		}

		public bool Exists(string typeName)
		{
			var types = GetTypes ();
			return types.ContainsKey (typeName)
			|| types.ContainsValue (typeName);
		}

		public void EnsureExists(Type type)
		{
            EnsureExists (type.Name, type.AssemblyQualifiedName);
		}

		public void EnsureExists(string typeName, string typeFullName)
		{
			if (!Exists (typeName)) {
				Add (typeName, typeFullName);
			}
		}

		public Type GetType(string typeName)
		{
            if (Settings.IsVerbose) {
                Console.WriteLine ("    Getting type '" + typeName + "'.");
            }

            Type type = null;
            
			var types = GetTypes ();
			if (types.ContainsKey (typeName)) {
                var typeFullName = types [typeName];	
                if (Settings.IsVerbose) {
                    Console.WriteLine ("      Full name: " + typeFullName);
                }		
                type = Type.GetType (typeFullName);
			} else
				throw new InvalidOperationException ("The type '" + typeName + "' was not found.");

            if (Settings.IsVerbose) {
                Console.WriteLine ("      Matched: " + type.AssemblyQualifiedName);
            }
            return type;
		}

	}
}

