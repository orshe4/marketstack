namespace Marketstack.Services
{
    public class MarketstackOptions
    {
        public string ApiToken { get; set; }
        public int MaxRequestsPerSecond { get; set; } = 1;
        public bool Https { get; set; } = true;
    }
}