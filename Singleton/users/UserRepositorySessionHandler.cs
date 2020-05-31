using System;
using System.Collections.Generic;
using System.Linq;
using Singleton.branches;
using Singleton.files;

namespace Singleton
{
    public class UserRepositorySessionHandler
    {
        private UserService userService;

        public UserRepositorySessionHandler()
        {
            userService = UserService.Instance;
        }

        public void handle(UserRepositorySession userRepositorySession)
        {
            var repositoryAccess = userRepositorySession.Current;
            var user = userRepositorySession.User;
            var repositoryOperations = prepareRepositoryOperations(repositoryAccess, user);
            do
            {
                repositoryOperations.ForEach(it =>
                    Console.WriteLine($"{it.Id} - {it.Name}")
                );
                int choice = ReadUserInputChoice();
                var operation = repositoryOperations.Find(it => it.Id == choice);
                userRepositorySession = handle(userRepositorySession, operation);
                repositoryOperations = userRepositorySession.RepositoryOperations;
            } while (repositoryOperations.Any());
        }

        private UserRepositorySession handle(UserRepositorySession userRepositorySession,
            RepositoryOperation repositoryOperation)
        {
            User user = userRepositorySession.User;
            RepositoryAccess repositoryAccess = userRepositorySession.Current;
            switch (repositoryOperation.RepositoryOperationType)
            {
                case RepositoryOperationType.MAIN_MENU:
                {
                    return new UserRepositorySession(user, repositoryAccess, new List<RepositoryOperation>());
                }
                case RepositoryOperationType.INVITE_USER:
                {
                    var result = grantAccess(userRepositorySession);
                    Console.WriteLine($"{result.Item1.Login} got access to {result.Item2}");
                    return new UserRepositorySession(user, repositoryAccess, prepareRepositoryOperations(repositoryAccess, user));
                }
                case RepositoryOperationType.CREATE_BRANCH:
                {
                    var result = createBranch(userRepositorySession);
                    Console.WriteLine($"Branch: {result.Name} was created");
                    return new UserRepositorySession(user, repositoryAccess, prepareRepositoryOperations(repositoryAccess, user));
                }
                case RepositoryOperationType.JOIN_AS_CONTRIBUTOR:
                {
                    var result = getAccess(userRepositorySession);
                    Console.WriteLine($"{result.Item1.Login} got access to {result.Item2}");
                    return new UserRepositorySession(user, repositoryAccess, prepareRepositoryOperations(repositoryAccess, user));
                }
                case RepositoryOperationType.LIST_BRANCHES:
                {
                    var branches = listBranches(userRepositorySession);
                    Console.WriteLine($"Branches:{String.Join("\n", branches.Select(it=> it.Name ))}");
                    return new UserRepositorySession(user, repositoryAccess, prepareRepositoryOperations(repositoryAccess, user));
                }
                case RepositoryOperationType.PICK_BRANCH:
                {
                    var branch = getBranch(userRepositorySession);
                    Console.WriteLine($"Branch:{branch.Name}\nFiles:{String.Join("\n",branch.Files.Select(it=>it.Title))}");
                    return new UserRepositorySession(user, repositoryAccess, prepareRepositoryOperations(repositoryAccess, user));
                }
                case RepositoryOperationType.PUSH:
                {
                    var branch = pushToBranch(userRepositorySession);
                    Console.WriteLine($"Branch:{branch.Name}\nFiles:\n{String.Join("\n",branch.Files.Select(it=>it.Title))}");
                    return new UserRepositorySession(user, repositoryAccess, prepareRepositoryOperations(repositoryAccess, user));
                }
                case RepositoryOperationType.CLONE:
                {
                    var access = userRepositorySession.Current;
                    var repository = userRepositorySession.Current.Repository.cloneRepository(access);
                    userRepositorySession.User.ClonedRepositories.Add(repository);
                    var session = new UserRepositorySession(user, repositoryAccess, prepareRepositoryOperations(repositoryAccess, user));
                    Console.WriteLine($"Repository {repository.Name} was successfully cloned");
                    return session;
                }
            }

            return userRepositorySession;
        }
        
        private Branch getBranch(UserRepositorySession userRepositorySession)
        {
            Console.WriteLine("Enter name of branch:");
            string branchName = Console.ReadLine();
            var branch = userRepositorySession.Current.Repository.Branches.Find(it => it.Name.Equals(branchName));
            if (branch == null)
            {
                throw new BranchNotFound(branchName);
            }
            return branch;
        }
        
        private Branch pushToBranch(UserRepositorySession userRepositorySession)
        {
            Console.WriteLine("Enter name of branch:");
            string branchName = Console.ReadLine();
            Console.WriteLine("Enter name of file:");
            string fileName = Console.ReadLine();
            Console.WriteLine("Enter content of file:");
            string fileContent = Console.ReadLine();
            var branch = userRepositorySession.Current.Repository.Branches.Find(it => it.Name.Equals(branchName));
            if (branch == null)
            {
                throw new BranchNotFound(branchName);
            }
            var contributableAdapter = new ContributableAdapter(branch);
            var result =(ContributableAdapter) contributableAdapter.Push(userRepositorySession.Current, branchName, new File(fileName, fileContent));
            return (Branch) result.Contributable;
        }

        private List<Branch> listBranches(UserRepositorySession userRepositorySession)
        {
            var branches = userRepositorySession.Current.Repository.Branches;
            return branches;
        }
        
        private Branch createBranch(UserRepositorySession userRepositorySession)
        {
            Console.WriteLine("Enter name of branch:");
            string branchName = Console.ReadLine();
            Console.WriteLine("Enter name of file:");
            string fileName = Console.ReadLine();
            Console.WriteLine("Enter content of file:");
            string fileContent = Console.ReadLine();
            var files = new List<File>();
            var file = new File(fileName, fileContent);
            files.Add(file);
            var branch = Branch.New(branchName, files);
            userRepositorySession.Current.Repository.Branches.Add(branch);
            return branch;
        }

        private Tuple<User, string> grantAccess(UserRepositorySession userRepositorySession)
        {
            var repositoryAccess = userRepositorySession.Current;
            var user = userRepositorySession.User;
            if (repositoryAccess.isOwner(user.Login))
            {
                Console.WriteLine("Enter login of user:");
                string login = Console.ReadLine();
                User invitee = userService.getUser(login);
                invitee.RepositoryAccesses.Add(new RepositoryAccess(repositoryAccess.Repository, RepositoryAccessType.CONTRIBUTOR, user));
                repositoryAccess.Repository.addContributor(invitee);
                return new Tuple<User, string>(invitee, repositoryAccess.Repository.Name); 
            }

            throw new OperationNotPermitted(user.Login, RepositoryOperationType.INVITE_USER);

        }
        
        private Tuple<User, string> getAccess(UserRepositorySession userRepositorySession)
        {
            var repositoryAccess = userRepositorySession.Current;
            var user = userRepositorySession.User;
            
            if (!user.getContributedRepositories().Contains(repositoryAccess.Repository))
            {
                user.RepositoryAccesses.Add(repositoryAccess);
                repositoryAccess.Repository.addContributor(user);
                return new Tuple<User, string>(user, repositoryAccess.Repository.Name); 
            }

            throw new OperationNotPermitted(user.Login, RepositoryOperationType.INVITE_USER);

        }
        private static int ReadUserInputChoice()
        {
            return Int32.Parse(Console.ReadLine() ?? throw new ArgumentException("Invalid choice"));
        }
        
        private List<RepositoryOperation> prepareRepositoryOperations(RepositoryAccess repositoryAccess, User user)
        {
            List<RepositoryOperation> repositoryOperations = new List<RepositoryOperation>();
            repositoryOperations.Add(new RepositoryOperation("Create branch", repositoryOperations.Count, RepositoryOperationType.CREATE_BRANCH));
            if (!user.ClonedRepositories.Contains(repositoryAccess.Repository))
            {
                repositoryOperations.Add(new RepositoryOperation("Clone repository", repositoryOperations.Count, RepositoryOperationType.CLONE)); 
            }
            if (repositoryAccess!=null && repositoryAccess.RepositoryAccessType == RepositoryAccessType.OWNER)
            {
                repositoryOperations.Add(new RepositoryOperation("Invite user", repositoryOperations.Count, RepositoryOperationType.INVITE_USER));
            }
            
            if (repositoryAccess.Repository.GetType() == typeof(PublicRepository) && !user.getContributedRepositories().Contains(repositoryAccess.Repository))
            {
                repositoryOperations.Add(new RepositoryOperation("Join as contibutor", repositoryOperations.Count, RepositoryOperationType.JOIN_AS_CONTRIBUTOR));
            }

            if (repositoryAccess.Repository.Branches.Any())
            {
                repositoryOperations.Add(new RepositoryOperation("List all branches", repositoryOperations.Count, RepositoryOperationType.LIST_BRANCHES));
                repositoryOperations.Add(new RepositoryOperation("Pick branch", repositoryOperations.Count, RepositoryOperationType.PICK_BRANCH));
                repositoryOperations.Add(new RepositoryOperation("Push to branch", repositoryOperations.Count, RepositoryOperationType.PUSH));
            }
            repositoryOperations.Add(new RepositoryOperation("Main menu", repositoryOperations.Count, RepositoryOperationType.MAIN_MENU));

            return repositoryOperations;
        }
    }
}