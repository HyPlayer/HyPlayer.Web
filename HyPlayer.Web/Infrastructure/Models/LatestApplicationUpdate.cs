namespace HyPlayer.Web.Infrastructure.Models
{

    public class LatestApplicationUpdate
    {
        public required string Version { get; set; }
        public required DateTime Date { get; set; }
        public required bool Mandatory { get; set; }
        public required string ViewUrl { get; set; }
        public required string UpdateLog { get; set; }
    }
}