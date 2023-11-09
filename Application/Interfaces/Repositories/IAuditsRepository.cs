namespace Application.Interfaces.Repositories
{
    public interface IAuditsRepository
    {
        Task AuditClaimAsync(string id, string httpRequestType);
        Task AuditCoverAsync(string id, string httpRequestType);
    }
}
