using System;
using System.Collections.Generic;
using System.Linq;
using Singleton.files;

namespace Singleton.branches
{
    public class Branch: Contributable
    {
        private string name;
        private List<Commit> commits;
        private List<File> files = new List<File>();

        public string Name => name;

        public List<Commit> Commits => commits;

        public List<File> Files => files;

        public static Branch New(string title, List<File> files)
        {
            var commits = new List<Commit>();
            commits.Add(new Commit("Initial commit", files));
            return new Branch(title, commits);
        }

        private Branch(string name, List<Commit> commits)
        {
            this.name = name;
            this.commits = commits;
            this.files = addFiles(commits.SelectMany(it => it.Files).ToList());
        }

        public string Title
        {
            get => name;
            set => name = value;
        }

        public List<Commit> log()
        {
            return commits;
        }
        
        public Commit pick(Guid hash)
        {
            return commits.Find(it => it.Hash.Equals(hash));
        }

        public List<File> getFiles()
        {
            return files;
        }

        protected bool Equals(Branch other)
        {
            return name == other.name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Branch) obj);
        }

        public override int GetHashCode()
        {
            return (name != null ? name.GetHashCode() : 0);
        }
        
        public Contributable add(List<File> files)
        {
            this.files = addFiles(files);
            return this;
        }

        private List<File> addFiles(List<File> files)
        {
            List<File> result = new List<File>();
            List<File> notChangedFiles = this.Files.FindAll(it => !files.Any(t => t.Title.Equals(it.Title)));
            result.AddRange(notChangedFiles);
            result.AddRange(files);
            return result;
        }
    }
}