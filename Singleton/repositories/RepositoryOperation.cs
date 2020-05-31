namespace Singleton
{
    public class RepositoryOperation
    {
        private string name;
        private int id;
        private RepositoryOperationType repositoryOperationType;

        public RepositoryOperation(string name, int id, RepositoryOperationType repositoryOperationType)
        {
            this.name = name;
            this.id = id;
            this.repositoryOperationType = repositoryOperationType;
        }

        public string Name => name;

        public int Id => id;

        public RepositoryOperationType RepositoryOperationType => repositoryOperationType;
    }
}