namespace DeveloperTools.Pages
{
    public partial class Index
    {
        public class AnalyzeReportItem
        {
            public AnalyzeReportItem()
            {

            }
            public AnalyzeReportItem(int id, string groupName, string operatorName, string description, DateTime dateAndTime, ReportStatusEnum reportStatus, bool isExecuteError, string? tableHtml)
            {
                Id = id;
                GroupName = groupName;
                OperatorName = operatorName;
                Description = description;
                DateAndTime = dateAndTime;
                ReportStatus = reportStatus;
                IsExecuteError = isExecuteError;
                TableHtml = tableHtml;
            }
            public int Id { get; set; }
            public DateTime DateAndTime { get; set; }
            public string? Description { get; set; }
            public string? GroupName { get; set; }
            public string? OperatorName { get; set; }
            public bool IsExecuteError { get; set; }
            public ReportStatusEnum ReportStatus { get; set; }
            public string TableHtml { get; set; }
        }

    }
}
