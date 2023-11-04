using Microsoft.Extensions.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data;
using DeveloperTools.Helpers;
using DeveloperTools.DatabaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.VisualBasic;
using System.Runtime.CompilerServices;
using System;
using System.Text;
using System.Reflection;

namespace DeveloperTools.Pages
{
    public partial class Index
    {
        public string ConnectionString;
        private List<string> results = new List<string>();

        public IConfiguration _configuration { get; }

        List<AnalyzeReportItem> AnalyseReports = new List<AnalyzeReportItem>();
        private readonly SmilerContext _smilerContext;

        private List<string> Operators = new List<string>()
        {
            "SmilerDB_RedbetNyx_prod",
            "SmilerDB_Vinnarum_prod",
            "SmilerDB_Mamamia_prod",
            "SmilerDB_Bertil_SE_prod"
        };

        public Index()
        {
            AnalyseReports = new List<AnalyzeReportItem>();

            ConnectionString = (new ConfigurationReader()).GetSetting("ConnectionStrings:MainConnection");

            if (ConnectionString.Contains("SmilerIntegrationTests"))
            {
                Operators = new List<string>()
                {
                    "SmilerIntegrationTests",
                };
            }

            var options = SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder<SmilerContext>(), ConnectionString).Options;

            _smilerContext = new SmilerContext(options);

        }

        public bool btnStartAnalyze_Enabled = true;
        private async Task btnStartAnalyze_OnClick(MouseEventArgs e)
        {
            if (!btnStartAnalyze_Enabled) return;
            try
            {
                btnStartAnalyze_Enabled = false;
                foreach (var @Operator in Operators)
                {
                    await VerifyCampaignRunningStatus("Campaign Running Status", @Operator);
                }

                foreach (var @Operator in Operators)
                {
                    await VerifyCampaignLogErrorCount("Verify campaign error log counts", @Operator);
                }

                foreach (var @Operator in Operators)
                {
                    await VerifyCampaignLogGaps("Verify Gap in campaign log", @Operator);
                }

                foreach (var @Operator in Operators)
                {
                    await VerifyCampaignStepLog("CampaignStepLog count check", @Operator);
                }

                foreach (var @Operator in Operators)
                {
                    await VerifyTransactionLog("TransactionLog count check", @Operator);
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                btnStartAnalyze_Enabled = true;
            }
        }

        public ReportStatusEnum GetMainGroupStatus(string? mainGroupName)
        {
            var failedCount = AnalyseReports.Count(x => x.GroupName == mainGroupName && x.ReportStatus == ReportStatusEnum.Failed);
            var runningCount = AnalyseReports.Count(x => x.GroupName == mainGroupName && x.ReportStatus == ReportStatusEnum.Running);
            var successCount = AnalyseReports.Count(x => x.GroupName == mainGroupName && x.ReportStatus == ReportStatusEnum.Success);
            if (runningCount > 0)
            {
                return ReportStatusEnum.Running;
            }
            else if (failedCount > 0)
            {
                return ReportStatusEnum.Failed;
            }
            else if (successCount > 0)
            {
                return ReportStatusEnum.Success;
            }
            else
            {
                return ReportStatusEnum.Undefined;
            }
        }

        public string GetStatusImage(ReportStatusEnum reportStatus)
        {
            switch (reportStatus)
            {
                case ReportStatusEnum.Success:
                    return "/img/success.png";
                case ReportStatusEnum.Running:
                    return "/gif/loading1.gif";
                case ReportStatusEnum.Failed:
                    return "/img/failed.jpg";
                case ReportStatusEnum.Warning:
                    return "/img/warning.jpg";
                case ReportStatusEnum.InQueue:
                    return "/img/inqueue.jpg";
            }
            return string.Empty;
        }

        public async Task VerifyCampaignRunningStatus(string groupName, string operatorName)
        {
            try
            {


                var tableName = $"{operatorName}.Campaign.CampaignLog_{DateTime.UtcNow.Year}_{DateTime.UtcNow.Month.ToString("d2")}_{DateTime.UtcNow.Day.ToString("d2")}";

                var log1 = await AddLog(groupName, operatorName, $"Fetching [{tableName}] Count1", ReportStatusEnum.Running);

                var sql = $"SELECT COUNT(*) FROM {tableName} WITH (NOLOCK)";
                var count1 = ExecuteSQL<int>(sql).FirstOrDefault();

                await AddLog(groupName, operatorName, $"Fetched [{tableName}] Count1: {count1}", ReportStatusEnum.Success);
                await ChangeLogStatus(log1, ReportStatusEnum.Success);

                var logWait1 = await AddLog(groupName, operatorName, $"Waiting 5 seconds:", ReportStatusEnum.Running);
                await Task.Delay(5000);
                await ChangeLogStatus(logWait1, ReportStatusEnum.Success);

                var log2 = await AddLog(groupName, operatorName, $"Fetching [{tableName}] Count2", ReportStatusEnum.Running);
                var count2 = ExecuteSQL<int>(sql).FirstOrDefault();
                await AddLog(groupName, operatorName, $"Fetched [{tableName}] Count2: {count2}", ReportStatusEnum.Success);
                await ChangeLogStatus(log2, ReportStatusEnum.Success);

                if (count2 > count1)
                {
                    await AddLog(groupName, operatorName, $"Count2 > Count1: {count2}>{count1}={count2 - count1}, {(count2 - count1) / 5} logs per second", ReportStatusEnum.Success);
                }
                else
                {
                    await AddLog(groupName, operatorName, $"Counts are not matching, Campaign engine not running - Failed", ReportStatusEnum.Failed);
                }
            }
            catch (Exception ex)
            {
                await AddLog(groupName, operatorName, $"Error: " + ex.ToString(), ReportStatusEnum.Success);
            }
        }

        public async Task VerifyCampaignLogErrorCount(string groupName, string operatorName)
        {
            try
            {
                var tableName = $"{operatorName}.Campaign.CampaignLog_{DateTime.UtcNow.Year}_{DateTime.UtcNow.Month.ToString("d2")}_{DateTime.UtcNow.Day.ToString("d2")}";

                var log1 = await AddLog(groupName, operatorName, $"Fetching error count: [{tableName}]", ReportStatusEnum.Running);

                var sql = $"Select Count(*) From {tableName} WITH (NOLOCK) Where [Status] > 50";
                var count = ExecuteSQL<int>(sql).FirstOrDefault();

                await AddLog(groupName, operatorName, $"Fetched error count: [{tableName}], Count: {count}", ReportStatusEnum.Success);
                await ChangeLogStatus(log1, ReportStatusEnum.Success);

                if (count > 0)
                {
                    await AddLog(groupName, operatorName, $"Error count is {count}, verify manually.", ReportStatusEnum.Failed);
                    var campaignLogsSql = $"Select * From {tableName} WITH (NOLOCK) Where LogLevel >= 50 order by LogDate DESC";
                    var campaignLogs = ExecuteSQL<CampaignLog>(campaignLogsSql).ToList();
                    var columnWidths = new List<int>()
                {
                    100, 300, 200, 200, 200, 200, 200, 200, 200, 200
                };
                    var html = GenerateHtmlTable(campaignLogs, columnWidths);
                    await AddLog(groupName, operatorName, $"Campaign Error Logs: {count}", ReportStatusEnum.Failed, false, html);
                }
                else
                {
                    await AddLog(groupName, operatorName, $"No Errors", ReportStatusEnum.Success);
                }
            }
            catch (Exception ex)
            {
                await AddLog(groupName, operatorName, $"Error: " + ex.ToString(), ReportStatusEnum.Success);
            }

        }



        public async Task VerifyCampaignLogGaps(string groupName, string operatorName)
        {
            try
            {



                var gapInSeconds = 30;
                var tableName = $"{operatorName}.Campaign.CampaignLog_{DateTime.UtcNow.Year}_{DateTime.UtcNow.Month.ToString("d2")}_{DateTime.UtcNow.Day.ToString("d2")}";

                var log1 = await AddLog(groupName, operatorName, $"Fetching {gapInSeconds} sec gap between campaign logs: [{tableName}]", ReportStatusEnum.Running);

                var sql = @$"
WITH LogWithLag AS (
    SELECT
        LogID,
        LogDate,
        LAG(LogDate) OVER (ORDER BY LogDate) AS PreviousLogDate
    FROM
        {tableName} WITH (NOLOCK)
)
SELECT
    LogID,
    LogDate,
    PreviousLogDate,
	DATEDIFF(SECOND, PreviousLogDate, LogDate) SecondsGap
FROM
    LogWithLag
WHERE
    DATEDIFF(SECOND, PreviousLogDate, LogDate) > {gapInSeconds}
ORDER BY
    LogDate;
";
                var campaignLogGaps = ExecuteSQL<CampaignLogGap>(sql).ToList();

                await AddLog(groupName, operatorName, $"Fetched {gapInSeconds} seconds gap between campaign logs: [{tableName}], Count: {campaignLogGaps.Count}", ReportStatusEnum.Success);
                await ChangeLogStatus(log1, ReportStatusEnum.Success);

                if (campaignLogGaps.Count > 0)
                {
                    await AddLog(groupName, operatorName, $"{gapInSeconds} seconds Gap between logs count is {campaignLogGaps.Count}, verify manually.", ReportStatusEnum.Failed);
                    var columnWidths = new List<int>()
                {
                    100, 300, 300, 300
                };
                    var html = GenerateHtmlTable(campaignLogGaps, columnWidths);
                    await AddLog(groupName, operatorName, $" {gapInSeconds} seconds Campaign log gap detected, may be campaign engine stopped working: {campaignLogGaps.Count}", ReportStatusEnum.Failed, false, html);
                }
                else
                {
                    await AddLog(groupName, operatorName, $"No gap between campaign logs Errors", ReportStatusEnum.Success);
                }
            }
            catch (Exception ex)
            {
                await AddLog(groupName, operatorName, $"Error: " + ex.ToString(), ReportStatusEnum.Success);
            }
        }


        public async Task VerifyCampaignStepLog(string groupName, string operatorName)
        {
            try
            {
                var log1 = await AddLog(groupName, operatorName, $"Fetching CampaignStepLog table count", ReportStatusEnum.Running);

                var sql = @$"Select Count(*) from {operatorName}.Campaign.CampaignStepLog Where WaitCriteriaFromDate > Convert(DATETIME, Convert(DATE, GETUTCDATE()))";
                var campaignLogCount = ExecuteSQL<int>(sql).FirstOrDefault();

                await AddLog(groupName, operatorName, $"Fetched CampaignStepLog table Count: {campaignLogCount}", ReportStatusEnum.Success);
                await ChangeLogStatus(log1, ReportStatusEnum.Success);

                if (campaignLogCount < 0)
                {
                    await AddLog(groupName, operatorName, $"CampaignStepLog count is zero, verify manually.", ReportStatusEnum.Failed);
                }
                else
                {
                    await AddLog(groupName, operatorName, $"CampaignStepLog count today is :{campaignLogCount}, looks correct", ReportStatusEnum.Success);
                }
            }
            catch (Exception ex)
            {
                await AddLog(groupName, operatorName, $"Error: " + ex.ToString(), ReportStatusEnum.Success);
            }

        }

        public async Task VerifyTransactionLog(string groupName, string operatorName)
        {
            try
            {

                var log1 = await AddLog(groupName, operatorName, $"Fetching TransactionLog table count", ReportStatusEnum.Running);

                var sql = @$"Select Count(*) from {operatorName}.Store.[Transaction] WITH (NOLOCK) Where TransactionDateTime > Convert(DATETIME, Convert(DATE, GETUTCDATE()))";
                var count = ExecuteSQL<int>(sql).FirstOrDefault();

                await AddLog(groupName, operatorName, $"Fetched TransactionLog table Count: {count}", ReportStatusEnum.Success);
                await ChangeLogStatus(log1, ReportStatusEnum.Success);

                if (count < 0)
                {
                    await AddLog(groupName, operatorName, $"TransactionLog count is zero, verify manually.", ReportStatusEnum.Failed);
                }
                else
                {
                    await AddLog(groupName, operatorName, $"TransactionLog count today is :{count}, looks correct", ReportStatusEnum.Success);
                }
            }
            catch (Exception ex)
            {
                await AddLog(groupName, operatorName, $"Error: " + ex.ToString(), ReportStatusEnum.Success);
            }
        }


        public async Task<AnalyzeReportItem> ChangeLogStatus(AnalyzeReportItem item, ReportStatusEnum reportStatus)
        {
            var itemToModify = AnalyseReports.First(x => x.Id == item.Id);
            itemToModify.ReportStatus = reportStatus;
            StateHasChanged();
            _ = Task.Delay(1);
            return itemToModify;
        }

        public async Task<AnalyzeReportItem> AddLog(string groupName, string operatorName, string description, ReportStatusEnum reportStatus = ReportStatusEnum.InQueue, bool isExecuteError = false, string? tableHtml = null)
        {
            var id = AnalyseReports.Count() > 0 ? AnalyseReports.Max(x => x.Id) + 1 : 1;

            try
            {
                var item = new AnalyzeReportItem(id, groupName, operatorName, description, DateTime.Now, reportStatus, isExecuteError, tableHtml);
                AnalyseReports.Add(item);
                StateHasChanged();
                _ = Task.Delay(1);
                return item;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static List<(string Name, PropertyInfo PropertyInfo)> GetProperties(Object obj)
        {
            var properties = new List<(string, PropertyInfo)>();
            // Get the type of the object
            Type type = obj.GetType();



            // Get all public properties
            PropertyInfo[] publicProperties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Console.WriteLine("\nPublic Properties:");
            foreach (var property in publicProperties)
            {
                properties.Add(new(property.Name, property));
            }
            return properties;
        }

        public string GenerateHtmlTable<T>(List<T> dataList, List<int> columnWidths)
        {
            try
            {


                if (dataList == null || dataList.Count == 0)
                {
                    return "<p>No data to display.</p>";
                }

                var properties = GetProperties(dataList[0]);

                var tableHtml = new StringBuilder();

                tableHtml.Append("<div class='table-responsive'><table class='table table-bordered'><thead><tr>");
                int i = 0;
                foreach (var property in properties)
                {
                    tableHtml.Append($"<th style='width: {columnWidths[i]}px;'>{property.Name}</th>");
                    i++;
                }

                tableHtml.Append("</tr></thead><tbody>");
                foreach (var item in dataList)
                {
                    tableHtml.Append("<tr>");
                    i = 0;
                    foreach (var property in properties)
                    {
                        tableHtml.Append($"<td><div style='max-width: {columnWidths[i]}px; overflow: auto; max-height: 100px;' >{property.PropertyInfo.GetValue(item)}</div></td>");
                        i++;
                    }
                    tableHtml.Append("</tr>");
                }

                tableHtml.Append("</tbody></table></div>");

                return tableHtml.ToString();
            }
            catch (Exception ex)
            {

                return string.Empty;
            }
        }


        private List<T> ExecuteSQL<T>(string sql)
        {
            try
            {
                FormattableString sqlExec = FormattableStringFactory.Create(sql);
                return _smilerContext.Database.SqlQuery<T>(sqlExec).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
