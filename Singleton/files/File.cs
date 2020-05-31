using System;

namespace Singleton.files
{
    public class File
    {
        private string title;
        private string content;
        private DateTime createdAt;

        public File(string title, string content)
        {
            this.title = title;
            this.content = content;
            this.createdAt = DateTime.Now;
        }

        public string Content => content;

        public string Title => title;
        
        public DateTime CreatedAt => createdAt;
        
        protected bool Equals(File other)
        {
            return title == other.title && content == other.content;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((File) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((title != null ? title.GetHashCode() : 0) * 397) ^ (content != null ? content.GetHashCode() : 0);
            }
        }
    }
}