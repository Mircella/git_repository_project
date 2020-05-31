using System;

namespace Singleton
{
    public class RepositoryNotFound: SystemException
    {
        public RepositoryNotFound(string repositoryName) : base($"Repository:{repositoryName} not found")
        {
        }
    }
}