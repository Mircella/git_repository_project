using System;

namespace Singleton
{
    public class OperationNotPermitted : SystemException
    {
        public OperationNotPermitted(string login, RepositoryOperationType repositoryOperationType) : base(
            $"Operation:{repositoryOperationType} is not permitted for {login}"
        )
        {
        }
    }
}