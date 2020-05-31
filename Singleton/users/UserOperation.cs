namespace Singleton
{
    public class UserOperation
    {
        private string name;
        private int id;
        private UserOperationType userOperationType;

        public UserOperation(string name, int id, UserOperationType userOperationType)
        {
            this.name = name;
            this.id = id;
            this.userOperationType = userOperationType;
        }

        public string Name => name;

        public int Id => id;

        public UserOperationType UserOperationType => userOperationType;
    }
}