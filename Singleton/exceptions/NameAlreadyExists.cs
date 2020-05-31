using System;

namespace Singleton
{
    public class NameAlreadyExists : SystemException
    {
        public NameAlreadyExists(Repository repository) : base(
            $"Repository has already set name: {repository.Name}"
            )
        {
        }
    }
}