using System;
using System.Collections.Generic;
using Singleton.branches;
using Singleton.files;

namespace Singleton
{
    public class PublicRepository: Repository
    {
        public PublicRepository(string name, User owner) : base(name, owner)
        {
            createBranch(new RepositoryAccess(this, RepositoryAccessType.OWNER, owner), "master");
        }
        
        public PublicRepository(string name, User owner, List<Branch> branches, List<User> contributors) : base(name, owner, branches, contributors)
        {
        }

        public override Shareable Push(RepositoryAccess repositoryAccess, string branchName, File file)
        {
            base.push(branchName, file);
            return this;
        }

        public override List<File> Pull(RepositoryAccess repositoryAccess, string branchName)
        {
            List<Branch> parentBranches = base.Branches;
            Branch branchThatWeSearch = parentBranches.Find(it => it.Name.Equals(branchName));
            if (branchThatWeSearch != null)
            {
                return branchThatWeSearch.Files;
            }
            return new List<File>();
        }

        public override void createBranch(RepositoryAccess repositoryAccess, string name)
        {
            validateRepositoryAccess(repositoryAccess);
            base.createBranch(repositoryAccess, name);
        }

        public override void validateRepositoryAccess(RepositoryAccess repositoryAccess)
        {
            if (!repositoryAccess.Repository.Equals(this))
            {
                throw new InvalidOperationException($"Operation for {this.Name} with {repositoryAccess} not allowed");
            }
        }

        public override void addAsContributor(RepositoryAccess repositoryAccess)
        {
            validateRepositoryAccess(repositoryAccess);
            base.addContributor(repositoryAccess.Contributor);
        }

        public override Repository clone(List<Branch> branches, List<User> contributors)
        {
            return new PublicRepository(this.Name, this.Owner, branches, contributors);
        }
    }
}