namespace Maddalena.Core.Mongo
{
	public class DatabaseOptions
	{
		public string ConnectionString { get; set; }
		public string UsersCollection { get; set; } = "Users";
		public string RolesCollection { get; set; } = "Roles";
	}
}