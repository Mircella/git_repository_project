using System;

namespace Singleton
{
    public class InvalidCredentials: SystemException
    {
        public InvalidCredentials() : base("Invalid credentials!")
        {
        }
    }
}