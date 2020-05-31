using System;

namespace Singleton
{
    public class BranchNotFound: SystemException
    {
        public BranchNotFound(string branchName) : base($"Branch:{branchName} not found")
        {
        }
    }
}