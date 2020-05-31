using System;

namespace Singleton
{
    public class AlreadyLoggedIn: SystemException
    {
        public AlreadyLoggedIn(string userLogin) : base(
            $"{userLogin} already logged in"
        )
        {
        }
    }
}