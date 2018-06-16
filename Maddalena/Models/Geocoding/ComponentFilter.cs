namespace Maddalena.Models.Geocoding
{
    public class ComponentFilter
    {
        public ComponentFilter(string component, string value)
        {
            Filter = string.Format("{0}:{1}", component, value);
        }

        public string Filter { get; set; }
    }
}