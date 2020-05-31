using System.ComponentModel;

namespace Singleton
{
    public class RepositoryFactory
    {

        public static Repository CreateRepository(RepositoryType repositoryType, string repositoryName, User user)
        {
            switch (repositoryType)
            {
                case RepositoryType.PUBLIC:
                {
                    return new PublicRepository(repositoryName, user);
                }
                case RepositoryType.PRIVATE:
                {
                    return new PrivateRepository(repositoryName, user);
                }
                default:
                    throw new InvalidEnumArgumentException($"Invalid type of repository:{repositoryType}");
            }
        }
       
    }

}