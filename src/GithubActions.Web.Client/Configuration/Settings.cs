namespace GithubActions.Web.Client.Configuration
{
    public interface ISettings
    {
        public Uris Uris { get; set; }
    }

    public class Settings
    {
        public Uris Uris { get; set; }
    }
}