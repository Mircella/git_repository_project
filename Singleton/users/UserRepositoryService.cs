using System;
using System.Collections.Generic;
using System.Linq;

namespace Singleton
{
    public class UserRepositoryService
    {
        private static UserRepositoryService instance;
        private UserRepositorySessionHandler userRepositorySessionHandler;
        private RepositoryClient repositoryClient;

        private UserRepositoryService()
        {
            this.repositoryClient = new RepositoryClient();
            this.userRepositorySessionHandler = new UserRepositorySessionHandler();
        }
        
        public static UserRepositoryService Instance
        {
            get { return instance ?? (instance = new UserRepositoryService()); }
        }

        public UserRepositoryAccount selectRepository(UserRepositoryAccount userRepositoryAccount)
        {
            Console.WriteLine("Enter name of repository to work with:");
            string repositoryName = Console.ReadLine();
            var user = userRepositoryAccount.User;
            var repository = repositoryClient.findRepository(repositoryName);
            
            if (repository == null)
            {
                throw new RepositoryNotFound(repositoryName);
            }
            
            var repositoryAccess = user.RepositoryAccesses.Find(it => it.Repository.Name.Equals(repositoryName));
            if (repositoryAccess == null)
            {
                if (!(repository.GetType() == typeof(PublicRepository)))
                {
                    throw new AccessDenied(repositoryName, user.Login);
                } 
                repositoryAccess = new RepositoryAccess(repository, RepositoryAccessType.CONTRIBUTOR, user);
            }
            
            var userRepositorySession = new UserRepositorySession(user, repositoryAccess);
            userRepositorySessionHandler.handle(userRepositorySession);
            return userRepositoryAccount;
        }
        
        public string createUserRepository(RepositoryType repositoryType, UserRepositoryAccount userRepositoryAccount)
        {
            Console.WriteLine("Enter name of repository:");
            string repositoryName = Console.ReadLine();
            Repository repository = repositoryClient.addNewRepository(repositoryType, repositoryName, userRepositoryAccount.User);
            userRepositoryAccount.User.ClonedRepositories.Add(repository);
            if (repository != null)
            {
                return $"Repository {repository.Name} was created";
            }
            else
            {
                return "Repository failed to be created, try again";
            }
        }
        
        public UserRepositoryAccount anonymous(List<UserOperation> userOperations)
        {
            return new UserRepositoryAccount(null, userOperations);
        }
        
        public UserRepositoryAccount identified(User user, List<UserOperation> userOperations)
        {
            var ownRepositories = repositoryClient.findOwnRepositories(user.RepositoryAccesses);
            var contributedRepositories = repositoryClient.findContributedRepositories(user.RepositoryAccesses);
            return new UserRepositoryAccount(user, userOperations, ownRepositories, contributedRepositories);
        }
        
        public List<string> listOwnRepositories(UserRepositoryAccount userRepositoryAccount)
        {
            return userRepositoryAccount.OwnRepositories.Select(it => it.Name).ToList();
        }
        
        public List<string> listClonedRepositories(UserRepositoryAccount userRepositoryAccount)
        {
            return userRepositoryAccount.User.ClonedRepositories.Select(it => it.Name).ToList();
        }

        public List<string> listContributedRepositories(UserRepositoryAccount userRepositoryAccount)
        {
            return userRepositoryAccount.ContributedRepositories.Select(it => it.Name).ToList();
        }

    }
}