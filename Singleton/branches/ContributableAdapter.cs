using System.Collections.Generic;
using Singleton.files;

namespace Singleton.branches
{
    public class ContributableAdapter: Shareable
    {
        private Contributable contributable;

        public ContributableAdapter(Contributable contributable)
        {
            this.contributable = contributable;
        }

        public Contributable Contributable => contributable;

        public Shareable Push(RepositoryAccess repositoryAccess, string branchName, File file)
        {
            var files = new List<File>();
            files.Add(file);
            contributable.add(files);
            return this;
        }

        public List<File> Pull(RepositoryAccess repositoryAccess, string branchName)
        {
            return contributable.getFiles();
        }
    }
}