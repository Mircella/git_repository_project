using System;
using System.Collections.Generic;
using System.Linq;
using Singleton.branches;
using Singleton.files;

namespace Singleton
{
    public class PrivateRepository : Repository
    {
        public PrivateRepository(string name, User owner) : base(name, owner)
        {
            createBranch(new RepositoryAccess(this, RepositoryAccessType.OWNER, owner), "master");
        }
        
        public PrivateRepository(string name, User owner, List<Branch> branches, List<User> contributors) : base(name, owner, branches, contributors)
        {
            createBranch(new RepositoryAccess(this, RepositoryAccessType.OWNER, owner), "master");
        }

        public override List<File> GetFilesForUser(RepositoryAccess repositoryAccess, string branchName)
        {
            validateContributor(repositoryAccess.Contributor);
            return base.GetFilesForUser(repositoryAccess, branchName);
        }

        public override List<Branch> GetBranchesForUser(RepositoryAccess repositoryAccess)
        {
            validateRepositoryAccess(repositoryAccess);
            validateContributor(repositoryAccess.Contributor);
            return base.GetBranchesForUser(repositoryAccess);
        }

        public override User addContributor(User contributor)
        {
            validateContributor(contributor);
            return base.addContributor(contributor);
        }

        private void validateContributor(User contributor)
        {
            Boolean hasAccess = this.hasAccess(contributor);
            if (!hasAccess)
            {
                throw new AccessDenied(this, contributor);
            }
        }

        private bool hasAccess(User contributor)
        {
            return contributor.RepositoryAccesses
                .Select(it => it.Repository)
                .Any(it => it.Equals(this));
        }

        public override Shareable Push(RepositoryAccess repositoryAccess, string branchName, File file)
        {
            validateRepositoryAccess(repositoryAccess);
            base.push(branchName, file);
            return this;
        }

        public override List<File> Pull(RepositoryAccess repositoryAccess, string branchName)
        {
            validateRepositoryAccess(repositoryAccess);

            List<Branch> parentBranches = base.Branches;
            Branch branchThatWeSearch = null;
            foreach (var branch in parentBranches)
            {
                if (branch.Name.Equals(branchName))
                {
                    branchThatWeSearch = branch;
                }
            }

            if (branchThatWeSearch != null)
            {
                return branchThatWeSearch.Files;
            }
            else
            {
                return new List<File>();
            }
        }

        public override void createBranch(RepositoryAccess repositoryAccess, string name)
        {
            validateRepositoryAccess(repositoryAccess);
            base.createBranch(repositoryAccess, name);
        }

        override public void validateRepositoryAccess(RepositoryAccess repositoryAccess)
        {
            if (!repositoryAccess.Repository.Equals(this))
            {
                throw new AccessDenied(this, repositoryAccess.Contributor);
            }
        }

        public override void addAsContributor(RepositoryAccess repositoryAccess)
        {
            validateRepositoryAccess(repositoryAccess);
            addContributor(repositoryAccess.Contributor);
        }

        public override Repository clone(List<Branch> branches, List<User> contributors)
        {
            return new PrivateRepository(this.Name, this.Owner, branches, contributors);
        }
    }
}