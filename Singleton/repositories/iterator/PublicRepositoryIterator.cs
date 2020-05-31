using System;

namespace Singleton.iterator
{
    public class PublicRepositoryIterator: RepositoryIterator
    {
        private PublicRepositoryCollection publicRepositoryCollection;

        private int currentPosition;
        private Repository[] publicRepositories;

        public PublicRepositoryIterator(PublicRepositoryCollection publicRepositoryCollection)
        {
            this.publicRepositoryCollection = publicRepositoryCollection;
        }

        private void loadPublicRepositories()
        {
            if (publicRepositories == null)
            {
                publicRepositories = publicRepositoryCollection.getRepositories().ToArray();
            }
        }
        public Repository getNext()
        {
            if (hasMore())
            {
                var repository = publicRepositories[currentPosition];
                currentPosition++;
                return repository;
            }

            return null;
        }

        public bool hasMore()
        {
            loadPublicRepositories();
            return currentPosition < publicRepositories.Length;
        }
    }
}