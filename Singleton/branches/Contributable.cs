using System;
using System.Collections.Generic;
using Singleton.files;

namespace Singleton.branches
{
    public interface Contributable
    {
        Contributable add(List<File> files);
        
        List<Commit> log();

        Commit pick(Guid hash);

        List<File> getFiles();
    }
}