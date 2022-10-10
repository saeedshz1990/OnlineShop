using OnlineShop.Infrastructure;

namespace OnlineShop.Persistence.EF
{
    public class EFUnitOfWork : UnitOfWork
    {
        private readonly EFDataContext _context;

        public EFUnitOfWork(EFDataContext context)
        {
            _context = context;
        }

        public async Task Begin()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.SaveChangesAsync();
            await _context.Database.CommitTransactionAsync();
        }

        public async Task CommitPartial()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Complete()
        {
            await _context.SaveChangesAsync();
        }

        public async Task Rollback()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}
