namespace Egnyte.Api.Audit
{
    public class AuditReport
    {
        internal AuditReport(AuditReportEnum report, string id)
        {
            Report = report;
            Id = id;
        }

        public AuditReportEnum Report { get; }

        public string Id { get; }
    }
}