using System.Collections.Generic;
using Singleton.files;
using Singleton.branches;

namespace Singleton
{
    public interface Shareable
    {
        Shareable Push(RepositoryAccess repositoryAccess, string branchName, File file);

        List<File> Pull(RepositoryAccess repositoryAccess, string branchName);
    }
}