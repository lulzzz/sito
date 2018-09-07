using Maddalena.Core.Mongo;

namespace Maddalena.Core.Identity.Model
{
	public class MongoRole : MongoObject
	{
		public MongoRole()
		{
		}

		public MongoRole(string name)
		{
			Name = name;
			NormalizedName = name.ToUpperInvariant();
		}

		public string Name { get; set; }
		public string NormalizedName { get; set; }

		public override string ToString()
		{
			return Name;
		}
	}
}