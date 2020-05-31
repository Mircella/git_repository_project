using System.Collections.Generic;

namespace Singleton.iterator
{
    public class PublicRepositoryCollection: RepositoryCollection
    {
        private RepositoryClient repositoryClient;

        public PublicRepositoryCollection()
        {
            this.repositoryClient = new RepositoryClient();
        }

        public RepositoryIterator RepositoryIterator()
        {
            return new PublicRepositoryIterator(this);
        }

        public List<Repository> getRepositories()
        {
            return repositoryClient.listRepositoriesByType(RepositoryType.PUBLIC);
        }
    }
}