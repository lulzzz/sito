namespace Maddalena.Core
{
    public class SiteSettings
    {
        public string Name { get; set; } = "Matteo Fabbri";
        public string Description { get; set; } = "In Italian nerd in Prague";
        public string Owner { get; set; } = "Matteo Fabbri";

        public string GoogleClientId { get; set; } = "";
        public string GoogleClientSecret { get; set; } = "";

        public string TwitterConsumerKey { get; set; } = "";
        public string TwitterConsumerSecret { get; set; } = "";
    }
}
