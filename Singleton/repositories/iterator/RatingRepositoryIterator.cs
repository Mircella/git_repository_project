namespace Singleton.iterator
{
    public class RatingRepositoryIterator: RepositoryIterator

    {
        private RatingRepositoryCollection ratingRepositoryCollection;
        private User user;
        private int currentPosition;
        private Repository[] ratingRepositories;
        
        public RatingRepositoryIterator(RatingRepositoryCollection ratingRepositoryCollection, User user)
        {
            this.ratingRepositoryCollection = ratingRepositoryCollection;
            this.user = user;
        }

        private void loadRepositories()
        {
            if (ratingRepositories == null)
            {
                ratingRepositories = ratingRepositoryCollection.getRepositories(10, user).ToArray();
            }
        }
        public Repository getNext()
        {
            if (hasMore())
            {
                var repository = ratingRepositories[currentPosition];
                currentPosition++;
                return repository;
            }

            return null;
        }

        public bool hasMore()
        {
            loadRepositories();
            return currentPosition < ratingRepositories.Length;
        }
    }
}