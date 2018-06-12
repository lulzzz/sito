namespace ServerSideAnalytics
{
    public class UserAgent
    {
        public ClientBrowser Browser { get; set; }
        public ClientOS OS { get; set; }

        public UserAgent()
        {
        }

        public UserAgent(string rawAgent)
        {
            Browser = new ClientBrowser(rawAgent);
            OS = new ClientOS(rawAgent);
        }
    }
}