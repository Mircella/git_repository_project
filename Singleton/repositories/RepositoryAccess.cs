namespace Singleton
{
    public class RepositoryAccess
    {
        private Repository repository;
        private User contributor;

        public User Contributor => contributor;

        private RepositoryAccessType repositoryAccessType;

        public RepositoryAccess(Repository repository, RepositoryAccessType repositoryAccessType, User contributor)
        {
            this.repository = repository;
            this.repositoryAccessType = repositoryAccessType;
            this.contributor = contributor;
        }

        public Repository Repository => repository;

        public RepositoryAccessType RepositoryAccessType => repositoryAccessType;

        public bool isOwner(string userName)
        {
            return repositoryAccessType == RepositoryAccessType.OWNER && this.contributor.Login.Equals(userName);
        }
    }
}