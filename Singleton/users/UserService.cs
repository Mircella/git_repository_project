using System;
using System.Collections.Generic;

namespace Singleton
{
    public class UserService
    {
        private static UserService instance;
        private List<User> loggedUsers;
        private List<User> users;
        
        public List<User> LoggedUsers => loggedUsers;

        public List<User> Users => users;

        private UserService()
        {
            this.loggedUsers = new List<User>();
            this.users = new List<User>();
        }
        
        public static UserService Instance
        {
            get { return instance ?? (instance = new UserService()); }
        }

        public User createUser(string login, string password)
        {
            User user = users.Find(it => it.Login.Equals(login));
            if (user != null)
            {
                throw new LoginAlreadyExists(login);
            }

            User newUser = new User(login, password);
            users.Add(newUser);
            return newUser;
        }

        public User logIn(string login, string password)
        {
            var loggedInUser = loggedUsers.Find(it => it.Login.Equals(login));
            if (loggedInUser != null)
            {
                throw new AlreadyLoggedIn(login);
            }

            var existingUser = getUser(login);
            checkCredentials(existingUser, login, password);
            loggedUsers.Add(existingUser);
            return existingUser;
        }
        
        public void logOut(User user)
        {
            loggedUsers.Remove(user);
        }
        
        public User getUser(string login)
        {
            var existingUser = users.Find(it => it.Login.Equals(login));
            if (existingUser == null)
            {
                throw new UserNotFound(login);
            }

            return existingUser;
        }
        
        private void checkCredentials(User user, string login, string password)
        {
            if (String.IsNullOrWhiteSpace(login) || String.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Invalid login or password");
            }

            if (!(user.Login.Equals(login) && user.Password.Equals(password)))
            {
                throw new InvalidCredentials();
            }
        }
    }
}