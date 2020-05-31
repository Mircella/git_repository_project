using System;
using System.Collections.Generic;
using Singleton.files;

namespace Singleton.branches
{
    public class Commit
    {
        private Guid hash;
        private string message;
        private List<File> files;
        private DateTime createdAt;

        public Commit(string message, List<File> files)
        {
            this.hash = System.Guid.NewGuid();
            this.message = message;
            this.files = files;
            this.createdAt = DateTime.Now;
        }
        
        public Guid Hash => hash;

        public DateTime CreatedAt
        {
            get => createdAt;
        }

        public List<File> Files
        {
            get => files;
        }

        public string Massage
        {
            set => message = value;
        }
    }
}