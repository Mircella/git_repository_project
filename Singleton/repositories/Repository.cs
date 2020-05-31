using System;
using System.Collections.Generic;
using System.Net;
using Singleton.branches;
using Singleton.files;

namespace Singleton
{
    public abstract class Repository: Shareable
    {
        private string name;
        private User owner;
        private int repositoryRating;
        private List<User> contributors;
        private List<Branch> branches;

        protected Repository(string name, User owner)
        {
            ValidateName(name);
            ValidateOwner(owner);
            this.name = name;
            this.owner = owner;
            this.branches = new List<Branch>();
            this.contributors = new List<User>();
            this.repositoryRating = getRepositoryRating();
        }

        protected Repository(string name, User owner, List<Branch> branches, List<User> contributors)
        {
            ValidateName(name);
            ValidateOwner(owner);
            this.name = name;
            this.owner = owner;
            this.branches = branches;
            this.contributors = contributors;
            this.repositoryRating = getRepositoryRating();
        }

        public int getRepositoryRating()
        {
            return contributors.Count;
        }

        public virtual List<File> GetFilesForUser(RepositoryAccess repositoryAccess, string branchName)
        {
            return this.GetBranchesForUser(repositoryAccess).Find(it => it.Title == branchName).Files;
        }

        public virtual List<Branch> GetBranchesForUser(RepositoryAccess repositoryAccess)
        {
            return this.branches;
        }
        
        public virtual List<User> GetContributorsForUser(RepositoryAccess repositoryAccess)
        {
            return this.contributors;
        }

        public virtual User addContributor(User contributor)
        {
            if (!contributors.Contains(contributor))
            {
                contributors.Add(contributor);
            }
            return contributor;
        }

        public string Name => name;

        public User Owner => owner;

        public List<User> Contributors => contributors;

        public List<Branch> Branches => branches;
        
        private void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name of repository must be present!");
            }
        }

        public void ValidateOwner(User owner)
        {
            if (owner == null)
            {
                throw new ArgumentException("Owner must be present!");
            }
            if (string.IsNullOrEmpty(owner.Login))
            {
                throw new ArgumentException("Login of Owner of repository is invalid!");
            }
        }
        
        protected bool Equals(Repository other)
        {
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Repository) obj);
        }

        public override int GetHashCode()
        {
            return (name != null ? name.GetHashCode() : 0);
        }

        public void push(string branchName, File file)
        {
            Branch branch = Branches.Find(it => it.Name.Equals(branchName));
            List<File> files = new List<File>();
            files.Add(file);
            branch.add(files);
        }

        public abstract Shareable Push(RepositoryAccess repositoryAccess, string branchName, File file);

        public abstract List<File> Pull(RepositoryAccess repositoryAccess, string branchName);

        public virtual void createBranch(RepositoryAccess repositoryAccess, string name)
        {
            var files = new List<File>();
            files.Add(new File("Readme.md", $"Project:'{repositoryAccess.Repository.Name}'"));
            this.Branches.Add(Branch.New(name, files));
        }

        public Repository cloneRepository(RepositoryAccess repositoryAccess)
        {
            validateRepositoryAccess(repositoryAccess);
            addAsContributor(repositoryAccess);
            List<Branch> branches = GetBranchesForUser(repositoryAccess);
            List<User> contributors = GetContributorsForUser(repositoryAccess);
            return clone(branches, contributors);
        }

        public abstract void validateRepositoryAccess(RepositoryAccess repositoryAccess);
        public abstract void addAsContributor(RepositoryAccess repositoryAccess);

        public abstract Repository clone(List<Branch> branches, List<User> contributors);
    }
}
