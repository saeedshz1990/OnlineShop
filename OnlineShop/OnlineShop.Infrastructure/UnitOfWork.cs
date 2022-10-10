namespace OnlineShop.Infrastructure
{
    public interface UnitOfWork
    {
        Task Begin();

        Task Commit();

        Task Rollback();

        Task CommitPartial();

        Task Complete();
    }
}
