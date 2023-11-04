namespace DeveloperTools.Pages
{
    public class CampaignLogGap
    {
        public long LogID { get; set; }
        public DateTime LogDate { get; set; }
        public DateTime PreviousLogDate { get; set; }
        public int SecondsGap { get; set; }
    }
}
