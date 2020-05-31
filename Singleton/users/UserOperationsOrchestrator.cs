using System;
using System.Collections.Generic;
using System.Linq;
using Singleton.branches;
using Singleton.iterator;

namespace Singleton
{
    public class UserOperationsOrchestrator
    {
        private UserService userService;
        private UserRepositoryService userRepositoryService;
        private RepositoryService repositoryService;

        public UserOperationsOrchestrator()
        {
            userService = UserService.Instance;
            userRepositoryService = UserRepositoryService.Instance;
            repositoryService = RepositoryService.Instance;
        }

        public Tuple<string, UserRepositoryAccount> handleUserOperation(UserRepositoryAccount userRepositoryAccount,
            UserOperationType type)
        {
            string result = "";
            UserRepositoryAccount resultUserRepositoryAccount = userRepositoryAccount;
            switch (type)
            {
                case UserOperationType.CREATE_PUBLIC_REPOSITORY:
                {
                    result = userRepositoryService.createUserRepository(RepositoryType.PUBLIC, userRepositoryAccount);
                    resultUserRepositoryAccount = identifiedUserRepositoryAccount(userRepositoryAccount.User);
                    break;
                }
                case UserOperationType.CREATE_PRIVATE_REPOSITORY:
                {
                    result = userRepositoryService.createUserRepository(RepositoryType.PRIVATE, userRepositoryAccount);
                    resultUserRepositoryAccount = identifiedUserRepositoryAccount(userRepositoryAccount.User);
                    break;
                }
                case UserOperationType.LIST_OWN_REPOSITORIES:
                {
                    result =
                        $"Your repositories:\n{String.Join("\n", userRepositoryService.listOwnRepositories(userRepositoryAccount).ToArray())}";
                    resultUserRepositoryAccount = identifiedUserRepositoryAccount(userRepositoryAccount.User);
                    break;
                }
                case UserOperationType.LIST_CLONED_REPOSITORIES:
                {
                    result =
                        $"Your cloned repositories:\n{String.Join("\n", userRepositoryService.listClonedRepositories(userRepositoryAccount).ToArray())}";
                    resultUserRepositoryAccount = identifiedUserRepositoryAccount(userRepositoryAccount.User);
                    break;
                }
                case UserOperationType.LIST_CONTRIBUTED_REPOSITORIES:
                {
                    var user = userService.getUser(userRepositoryAccount.User.Login);
                    userRepositoryAccount = identifiedUserRepositoryAccount(user);
                    result =
                        $"Repositories where you made contributions:\n{String.Join("\n", userRepositoryService.listContributedRepositories(userRepositoryAccount).ToArray())}";
                    resultUserRepositoryAccount = identifiedUserRepositoryAccount(userRepositoryAccount.User);
                    break;
                }
                case UserOperationType.LOG_OUT:
                {
                    result = "Thank you for your work! See you later!";
                    resultUserRepositoryAccount = logOut(userRepositoryAccount.User);
                    break;
                }
                case UserOperationType.SIGN_IN:
                {
                    resultUserRepositoryAccount = signIn();
                    result = $"Welcome back {resultUserRepositoryAccount.User.Login}! You successfully signed in.";
                    break;
                }
                case UserOperationType.SIGN_UP:
                {
                    resultUserRepositoryAccount = signUp();
                    result = "You successfully signed up";
                    break;
                }
                case UserOperationType.LOOK_FOR_USER:
                {
                    User existingUser = lookForUser();
                    var userOwnRepositories = existingUser.getOwnRepositories().Select(it => it.Name);
                    var userContributedRepositories = existingUser.getContributedRepositories().Select(it => it.Name);
                    result = $"Found user:{existingUser.Login}" +
                             $"\nUser own repositories:\n{String.Join("\n", userOwnRepositories)}" +
                             $"\nUser contributed repositories:\n{String.Join("\n", userContributedRepositories)}";
                    resultUserRepositoryAccount = identifiedUserRepositoryAccount(userRepositoryAccount.User);
                    break;
                }
                case UserOperationType.LOOK_FOR_REPOSITORY:
                {
                    RepositoryDetails repositoryDetails = repositoryService.getRepositoryDetails();
                    result = $"Repository:{repositoryDetails.Name}\n" +
                             $"Owner:{repositoryDetails.OwnerName}\n" +
                             $"Branches:\n{String.Join("\n", repositoryDetails.Branches)}\n" +
                             $"Contributors:\n{String.Join("\n", repositoryDetails.Contributors)}";
                    break;
                }
                case UserOperationType.SELECT_REPOSITORY:
                {
                    var account = userRepositoryService.selectRepository(userRepositoryAccount);
                    resultUserRepositoryAccount = identifiedUserRepositoryAccount(account.User);
                    break;
                }
                case UserOperationType.LIST_ALL_PUBLIC_REPOSITORIES:
                {
                    RepositoryCollection collection = new PublicRepositoryCollection();
                    RepositoryIterator repositoryIterator = collection.RepositoryIterator();
                    while (repositoryIterator.hasMore())
                    {
                        Repository repository = repositoryIterator.getNext();
                        Console.WriteLine($"Repository:{repository.Name}, owner:{repository.Owner.Login}");
                    }
                    break;
                }
                case UserOperationType.LIST_RATING_REPOSITORIES:
                {
                    RepositoryCollection collection = new RatingRepositoryCollection(resultUserRepositoryAccount.User);
                    RepositoryIterator repositoryIterator = collection.RepositoryIterator();
                    while (repositoryIterator.hasMore())
                    {
                        Repository repository = repositoryIterator.getNext();
                        Console.WriteLine($"Repository:{repository.Name}, owner:{repository.Owner.Login}, rating:{repository.getRepositoryRating()}");
                    }
                    break;
                }
                default:
                {
                    result = "Invalid user operation type";
                    break;
                }
            }

            return new Tuple<string, UserRepositoryAccount>(result, resultUserRepositoryAccount);
        }

        private UserRepositoryAccount logOut(User user)
        {
            userService.logOut(user);
            return anonymousUserRepositoryAccount();
        }

        private User lookForUser()
        {
            Console.WriteLine("Enter login of user:");
            string login = Console.ReadLine();
            var foundUser = userService.getUser(login);
            return foundUser;
        }

        private UserRepositoryAccount signIn()
        {
            Tuple<string, string> loginAndPassword = ReadLoginAndPassword();
            var loggedInUser = userService.logIn(loginAndPassword.Item1, loginAndPassword.Item2);
            return identifiedUserRepositoryAccount(loggedInUser);
        }

        private UserRepositoryAccount identifiedUserRepositoryAccount(User loggedInUser)
        {
            List<UserOperation> userOperations = initAdvancedUserOperations(loggedInUser);
            return userRepositoryService.identified(loggedInUser, userOperations);
        }

        private UserRepositoryAccount signUp()
        {
            Tuple<string, string> loginAndPassword = ReadLoginAndPassword();
            userService.createUser(loginAndPassword.Item1, loginAndPassword.Item2);
            UserRepositoryAccount userRepositoryAccount = anonymousUserRepositoryAccount();
            return userRepositoryAccount;
        }

        public UserRepositoryAccount anonymousUserRepositoryAccount()
        {
            List<UserOperation> userOperations = initAnonymousUserOperations();
            return userRepositoryService.anonymous(userOperations);
        }

        private List<UserOperation> initAdvancedUserOperations(User user)
        {
            List<UserOperation> userOperations = initIdentifiedUserOperations();
            List<RepositoryAccess> repositoryAccesses = user.RepositoryAccesses;

            if (repositoryAccesses.Any(it => it.RepositoryAccessType == RepositoryAccessType.OWNER))
            {
                userOperations.Add(new UserOperation(
                    "List own repositories", userOperations.Count, UserOperationType.LIST_OWN_REPOSITORIES)
                );
            }
            
            if (repositoryAccesses.Any(it => it.Contributor.ClonedRepositories.Count > 0))
            {
                userOperations.Add(new UserOperation(
                    "List cloned repositories", userOperations.Count, UserOperationType.LIST_CLONED_REPOSITORIES)
                );
            }

            if (repositoryAccesses.Any(it => it.RepositoryAccessType == RepositoryAccessType.CONTRIBUTOR))
            {
                userOperations.Add(new UserOperation(
                    "List contributed repositories", userOperations.Count,
                    UserOperationType.LIST_CONTRIBUTED_REPOSITORIES)
                );
            }
            userOperations.Add(new UserOperation(
                "Select repository to work", userOperations.Count, UserOperationType.SELECT_REPOSITORY)
            );


            return userOperations;
        }

        private List<UserOperation> initIdentifiedUserOperations()
        {
            List<UserOperation> userOperations = new List<UserOperation>();
            userOperations.Add(new UserOperation("Create public repository", userOperations.Count,
                UserOperationType.CREATE_PUBLIC_REPOSITORY));
            userOperations.Add(new UserOperation("Create private repository", userOperations.Count,
                UserOperationType.CREATE_PRIVATE_REPOSITORY));
            userOperations.Add(new UserOperation("List public repositories", userOperations.Count, UserOperationType.LIST_ALL_PUBLIC_REPOSITORIES));
            userOperations.Add(new UserOperation("List repositories by rating", userOperations.Count, UserOperationType.LIST_RATING_REPOSITORIES));
            userOperations.Add(new UserOperation("Log out", userOperations.Count, UserOperationType.LOG_OUT));
            userOperations.Add(new UserOperation("Look for repository", userOperations.Count, UserOperationType.LOOK_FOR_REPOSITORY));
            userOperations.Add(new UserOperation("Look for user", userOperations.Count, UserOperationType.LOOK_FOR_USER));
            return userOperations;
        }

        private List<UserOperation> initAnonymousUserOperations()
        {
            List<UserOperation> userOperations = new List<UserOperation>();
            userOperations.Add(new UserOperation("Sign up", 0,
                UserOperationType.SIGN_UP));
            userOperations.Add(new UserOperation("Sign in", 1,
                UserOperationType.SIGN_IN));
            return userOperations;
        }

        private Tuple<string, string> ReadLoginAndPassword()
        {
            Console.WriteLine();
            Console.WriteLine("Enter your login:");
            string login = Console.ReadLine();
            Console.WriteLine("Enter your password:");
            string password = Console.ReadLine();
            return new Tuple<string, string>(login, password);
        }
    }
}