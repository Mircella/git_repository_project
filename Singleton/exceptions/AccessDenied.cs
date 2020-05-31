using System;
using System.Security;

namespace Singleton
{
    public class AccessDenied : SystemException
    {
        public AccessDenied(Repository repository, User user) : base(
            $"Access to {repository.Name} is denied for {user.Login}"
        )
        {
        }
        
        public AccessDenied(string repositoryName, string user) : base(
            $"Access to {repositoryName} is denied for {user}"
        )
        {
        }
    }
}