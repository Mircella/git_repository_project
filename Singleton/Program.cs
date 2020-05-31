using System;
using System.Collections.Generic;

namespace Singleton
{
    class Program
    {
        static void Main(string[] args)
        {
            UserOperationsOrchestrator userOperationsOrchestrator = new UserOperationsOrchestrator();
            Console.WriteLine("Welcome to GangOfThree repository!");
            while (true)
            {
                UserRepositoryAccount userRepositoryAccount = userOperationsOrchestrator.anonymousUserRepositoryAccount();
                while (true)
                {
                    Console.WriteLine($"Choose one option:");
                    try
                    {
                        List<UserOperation> userOperations = userRepositoryAccount.UserOperations;
                        userOperations.ForEach(it =>
                            Console.WriteLine($"{it.Id} - {it.Name}")
                        );
                        int choice = ReadUserInputChoice();
                        UserOperation userOperation = userOperations.Find(it => it.Id == choice);
                        Tuple<string, UserRepositoryAccount> result =
                            userOperationsOrchestrator.handleUserOperation(userRepositoryAccount, userOperation.UserOperationType);
                        userRepositoryAccount = result.Item2;
                        Console.WriteLine(result.Item1);
                    
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }  
                }
            }
        }
        
        private static int ReadUserInputChoice()
        {
            return Int32.Parse(Console.ReadLine() ?? throw new ArgumentException("Invalid choice"));
        }
    }
}