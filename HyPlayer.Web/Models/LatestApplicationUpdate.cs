namespace HyPlayer.Web.Models
{

    public class LatestApplicationUpdate
    {
        public required string Version { get; set; }
        public required DateTime Date { get; set; }
        public required bool Mandatory { get; set; }
        public required string DownloadUrl { get; set; }
        public required string UpdateLog { get; set; }
        public required int Size { get; set; }
    }
}