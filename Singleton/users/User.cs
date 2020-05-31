using System.Collections.Generic;
using System.Linq;

namespace Singleton
{
    public class User
    {
        
        private string login;
        private string password;
        private List<RepositoryAccess> repositoryAccesses;
        private List<Repository> clonedRepositories;

        public User(string login, string password)
        {
            this.login = login;
            this.password = password;
            this.repositoryAccesses = new List<RepositoryAccess>();
            this.clonedRepositories = new List<Repository>();
        }

        public string Password => password;

        public string Login => login;
        
        public List<RepositoryAccess> RepositoryAccesses => repositoryAccesses;

        public List<Repository> ClonedRepositories => clonedRepositories;

        public List<Repository> getOwnRepositories()
        {
            return RepositoryAccesses.FindAll( it => it.RepositoryAccessType == RepositoryAccessType.OWNER).Select(it => it.Repository).ToList();
        }
        
        public List<Repository> getContributedRepositories()
        {
            return RepositoryAccesses.FindAll( it => it.RepositoryAccessType == RepositoryAccessType.CONTRIBUTOR).Select(it => it.Repository).ToList();
        }
        
        protected bool Equals(User other)
        {
            return login == other.login && password == other.password;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((login != null ? login.GetHashCode() : 0) * 397) ^ (password != null ? password.GetHashCode() : 0);
            }
        }
    }
}