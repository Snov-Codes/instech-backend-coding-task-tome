using Domain.Entities;
using Persistance.DataAccess;
using Persistance.Interfaces;

namespace Persistance.Repositories
{
    public class AuditsRepository : IAuditsRepository
    {
        private readonly EfDataContext _dbContext;

        public AuditsRepository(EfDataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AuditClaimAsync(string id, string httpRequestType)
        {
            var claimAudit = new ClaimAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                ClaimId = id
            };

            _dbContext.Add(claimAudit);
            _dbContext.SaveChangesAsync();
        }

        public async Task AuditCoverAsync(string id, string httpRequestType)
        {
            var coverAudit = new CoverAudit()
            {
                Created = DateTime.Now,
                HttpRequestType = httpRequestType,
                CoverId = id
            };

            _dbContext.Add(coverAudit);
            _dbContext.SaveChangesAsync();
        }
    }
}
