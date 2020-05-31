using System;
using System.Collections.Generic;
using System.Linq;
using Singleton.branches;

namespace Singleton
{
    public class RepositoryService
    
    {
        private static RepositoryService instance;
        private Storage storage;

        private RepositoryService()
        {
            this.storage = Storage.Instance;
        }
        
        public static RepositoryService Instance
        {
            get { return instance ?? (instance = new RepositoryService()); }
        }

        public List<Repository> getAll()
        {
            return storage.Repositories;
        }
        
        public Repository addToStorage(Repository repository)
        {
            return storage.AddRepository(repository);
        }
        
        public Repository findByName(string repositoryName)
        {
            Repository found = storage.Repositories.Find(it => it.Name.Equals(repositoryName));
            
            if (found == null)
            {
                throw new RepositoryNotFound(repositoryName);
            }

            return found;
        }
        
        public RepositoryDetails getRepositoryDetails()
        {
            Console.WriteLine("Enter name of repository:");
            string repositoryName = Console.ReadLine();
            Repository repository = findByName(repositoryName);
            List<string> branches = listBranches(repository).Select(it=> it.Name).ToList();
            List<string> contributers = listContributors(repository).Select(it => it.Login).ToList();
            string ownerName = getOwner(repository).Login;
            return new RepositoryDetails(repositoryName, ownerName, contributers, branches);
        }

        private List<Branch> listBranches(Repository repository)
        {
            return repository.Branches;
        }

        private List<User> listContributors(Repository repository)
        {
            return repository.Contributors;
        }

        private User getOwner(Repository repository)
        {
            return repository.Owner;
        }
    }
}