using System.Collections.Generic;

namespace Singleton
{
    public class UserRepositorySession
    {
        private User user;
        private RepositoryAccess current;
        private List<RepositoryOperation> repositoryOperations;

        public UserRepositorySession(User user, RepositoryAccess current, List<RepositoryOperation> repositoryOperations)
        {
            this.user = user;
            this.current = current;
            this.repositoryOperations = repositoryOperations;
        }

        
        public UserRepositorySession(User user, RepositoryAccess current)
        {
            this.user = user;
            this.current = current;
            this.repositoryOperations = new List<RepositoryOperation>();
        }

        public List<RepositoryOperation> RepositoryOperations => repositoryOperations;

        public RepositoryAccess Current => current;

        public User User => user;
    }
}