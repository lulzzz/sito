namespace Maddalena.Modules.Geocoding
{
	public class ComponentFilter
	{
		public string Filter { get; set; }

		public ComponentFilter(string component, string value)
		{
			Filter = string.Format("{0}:{1}", component, value);
		}
	}
}
