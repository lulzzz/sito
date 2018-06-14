namespace ServerSideAnalytics
{
    public class UserAgent
    {
        public ClientBrowser Browser { get; set; }
        public ClientOs Os { get; set; }

        public UserAgent()
        {
        }

        public UserAgent(string rawAgent)
        {
            Browser = new ClientBrowser(rawAgent);
            Os = new ClientOs(rawAgent);
        }
    }
}