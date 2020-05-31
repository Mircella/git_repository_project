using System;

namespace Singleton
{
    public class LoginAlreadyExists: SystemException
    {
        public  LoginAlreadyExists(string login) : base($"Login {login} already exists")
        {
        }
    }
}