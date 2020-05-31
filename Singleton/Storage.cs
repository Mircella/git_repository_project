using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Singleton
{
    public class Storage
    {
        private static Storage instance;
        private List<Repository> repositories;

        private Storage()
        {
            this.repositories = new List<Repository>();
        }

        public static Storage Instance
        {
            get { return instance ?? (instance = new Storage()); }
        }
        
        public Repository AddRepository(Repository repository)
        {
            if (repositories.Contains(repository))
            {
                throw new RepositoryWithSuchNameAlreadyExists();
            }
            this.repositories.Add(repository);
            return repository;
        }
        
        public List<Repository> Repositories => repositories;
    }
}

