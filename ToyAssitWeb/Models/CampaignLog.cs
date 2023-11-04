namespace ToyAssist.Web.Pages
{
    public class CampaignLog
    {
        public long LogID { get; set; }
        public System.DateTime LogDate { get; set; }
        public int LogLevel { get; set; }
        public Nullable<int> ProcessID { get; set; }
        public string ProcessName { get; set; }
        public Nullable<int> Status { get; set; }
        public string Message { get; set; }
        public string Description { get; set; }
        public Nullable<int> CampaignID { get; set; }
        public Nullable<int> CampaignStepID { get; set; }
    }
}
