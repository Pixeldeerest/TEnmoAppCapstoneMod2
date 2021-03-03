using MenuFramework;
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using TenmoServer.Models;
using TenmoServer.DAO;
using TenmoClient.Data;

namespace TenmoClient.Views
{
    public class MainMenu : ConsoleMenu
    {
        private AccountSqlDAO accountDAO;

        public MainMenu()
        { 
            AddOption("View your current balance", ViewBalance)
                .AddOption("View your past transfers", ViewTransfers)
                .AddOption("View your pending requests", ViewRequests)
                .AddOption("Send TE bucks", SendTEBucks)
                .AddOption("Request TE bucks", RequestTEBucks)
                .AddOption("Log in as different user", Logout)
                .AddOption("Exit", Exit);
        }

        protected override void OnBeforeShow()
        {
            Console.WriteLine($"TE Account Menu for User: {UserService.GetUserName()}");
        }

        private MenuOptionResult ViewBalance()
        {
            AuthService authService = new AuthService();
            
            //Console.WriteLine($"Please tell me your UserID!");
            //int userId = Convert.ToInt32(Console.ReadLine());
            decimal balance = accountDAO.GetAccountBalance(UserService.GetUserId());
            Console.WriteLine($"{balance:c2}");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewTransfers()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult ViewRequests()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult SendTEBucks()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult RequestTEBucks()
        {
            Console.WriteLine("Not yet implemented!");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        private MenuOptionResult Logout()
        {
            UserService.SetLogin(new API_User()); //wipe out previous login info
            return MenuOptionResult.CloseMenuAfterSelection;
        }

    }
}
