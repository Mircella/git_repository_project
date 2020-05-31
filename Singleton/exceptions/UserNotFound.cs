using System;

namespace Singleton
{
    public class UserNotFound: SystemException
    {
        public UserNotFound(string login) : base($"User with login:{login} not found")
        {
        }
    }
}