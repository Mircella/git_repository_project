using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Singleton
{
    public class RepositoryClient
    {
        private RepositoryService repositoryService;

        public RepositoryClient()
        {
            this.repositoryService = RepositoryService.Instance;
        }

        public Repository findRepository(string repositoryName)
        {
            return repositoryService.findByName(repositoryName);
        }

        public Repository addNewRepository(RepositoryType repositoryType, string repositoryName, User user)
        {
            Repository repository = RepositoryFactory.CreateRepository(repositoryType, repositoryName, user);
            repository.Contributors.Add(user);
            Repository newRepository = repositoryService.addToStorage(repository);
            user.RepositoryAccesses.Add(new RepositoryAccess(newRepository, RepositoryAccessType.OWNER, user));
            user.RepositoryAccesses.Add(new RepositoryAccess(newRepository, RepositoryAccessType.CONTRIBUTOR, user));
            return newRepository;
        }

        public List<Repository> listRepositoriesByRating(int topCount, User user)
        {
            var accessibleRepositories = user.RepositoryAccesses.Select(r => r.Repository.Name);
            List<Repository> repositories = repositoryService.getAll()
                .FindAll(it =>
                    it.GetType() == typeof(PublicRepository)
                    || accessibleRepositories.Any(name => name.Equals(it.Name)))
                .Take(topCount)
                .ToList();
            repositories.Sort((first, second) =>
                - first.getRepositoryRating().CompareTo(second.getRepositoryRating()));
            return repositories;
        }

        public List<Repository> listRepositoriesByType(RepositoryType repositoryType)
        {
            switch (repositoryType)
            {
                case RepositoryType.PUBLIC:
                {
                    List<Repository> repositories = repositoryService.getAll()
                        .FindAll(it => it.GetType() == typeof(PublicRepository)).ToList();
                    return repositories;
                }
                case RepositoryType.PRIVATE:
                {
                    List<Repository> repositories = repositoryService.getAll()
                        .FindAll(it => it.GetType() == typeof(PrivateRepository)).ToList();
                    return repositories;
                }
                default:
                    throw new InvalidEnumArgumentException($"Invalid type of repository:{repositoryType}");
            }
        }

        public List<Repository> findOwnRepositories(List<RepositoryAccess> repositoryAccesses)
        {
            List<Repository> ownRepositories = repositoryAccesses
                .FindAll(it => it.RepositoryAccessType == RepositoryAccessType.OWNER)
                .Select(it => it.Repository).ToList();
            return repositoryService.getAll().FindAll(it => ownRepositories.Contains(it));
        }

        public List<Repository> findContributedRepositories(List<RepositoryAccess> repositoryAccesses)
        {
            List<Repository> ownRepositories = repositoryAccesses
                .FindAll(it => it.RepositoryAccessType == RepositoryAccessType.CONTRIBUTOR)
                .Select(it => it.Repository).ToList();
            return repositoryService.getAll().FindAll(it => ownRepositories.Contains(it));
        }
    }
}