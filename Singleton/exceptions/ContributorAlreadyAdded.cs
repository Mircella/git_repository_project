using System;

namespace Singleton
{
    public class ContributorAlreadyAdded: SystemException
    {
        public ContributorAlreadyAdded(User contributor, Repository repository) : base(
            $"{contributor.Login} is already contributor in repository:{repository.Name}"
        )
        {
        }
    }
}