using System;

namespace Singleton
{
    public class RepositoryWithSuchNameAlreadyExists: SystemException
    {
        public RepositoryWithSuchNameAlreadyExists() : base("Repository with such name already exists")
        {
        }
    }
}