using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Singleton
{
    public class UserRepositoryAccount
    {
        private User user;
        private List<UserOperation> userOperations;
        private List<Repository> ownRepositories;
        private List<Repository> contributedRepositories;

        private Repository currentRepository;
        
        public UserRepositoryAccount(
            User user, 
            List<UserOperation> userOperations, 
            List<Repository> ownRepositories, 
            List<Repository> contributedRepositories
        )
        {
            this.user = user;
            this.userOperations = userOperations;
            this.ownRepositories = ownRepositories;
            this.contributedRepositories = contributedRepositories;
        }

        public UserRepositoryAccount(User user, List<UserOperation> userOperations)
        {
            this.user = user;
            this.userOperations = userOperations;
            this.contributedRepositories = new List<Repository>();
            this.ownRepositories = new List<Repository>();
        }

        public User User => user;
        public List<UserOperation> UserOperations
        {
            get => userOperations;
            set => userOperations = value;
        }

        public List<Repository> OwnRepositories => ownRepositories;

        public List<Repository> ContributedRepositories => contributedRepositories;

        public Repository CurrentRepository => currentRepository;
    }
}