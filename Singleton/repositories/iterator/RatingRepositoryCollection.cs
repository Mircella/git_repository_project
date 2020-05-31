using System.Collections.Generic;

namespace Singleton.iterator
{
    public class RatingRepositoryCollection: RepositoryCollection
    {
        
        private RepositoryClient repositoryClient;
        private User user;
        
        public RatingRepositoryCollection(User user)
        {
            this.user = user;
            this.repositoryClient = new RepositoryClient();
        }

        public RepositoryIterator RepositoryIterator()
        {
            return new RatingRepositoryIterator(this, user);
        }

        public List<Repository> getRepositories(int topCount, User user)
        {
            return repositoryClient.listRepositoriesByRating(topCount, user);
        }
    }
}