using System.Collections.Generic;

namespace Singleton
{
    public class RepositoryDetails
    {
        private string name;
        private string ownerName;
        private List<string> contributors;
        private List<string> branches;

        public RepositoryDetails(string name, string ownerName, List<string> contributors, List<string> branches)
        {
            this.name = name;
            this.ownerName = ownerName;
            this.contributors = contributors;
            this.branches = branches;
        }
        
        public string Name => name;

        public string OwnerName => ownerName;

        public List<string> Contributors => contributors;

        public List<string> Branches => branches;
    }
}