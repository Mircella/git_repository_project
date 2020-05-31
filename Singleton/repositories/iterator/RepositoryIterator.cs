namespace Singleton.iterator
{
    public interface RepositoryIterator
    {
        Repository getNext();
        bool hasMore();
    }
}