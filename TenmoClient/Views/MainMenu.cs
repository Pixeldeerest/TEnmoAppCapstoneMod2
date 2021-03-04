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
        private UserSqlDAO userDAO;

        public MainMenu()
        { 
            AddOption("View your current balance", ViewBalance)
                .AddOption("View your past transfers", ViewTransfers)
                //.AddOption("View your pending requests", ViewRequests)
                .AddOption("Send TE bucks", SendTEBucks)
                //.AddOption("Request TE bucks", RequestTEBucks)
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
            decimal balance = authService.GetBalance();
            Console.WriteLine($"Your current account balance is: {balance:c}");
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        //$"acccounts/transfers/{transferId}
        // "acccounts/transfers"
        private MenuOptionResult ViewTransfers()
        {
            Transfer transfer = new Transfer();
            AuthService authService = new AuthService();
            int userId = UserService.GetUserId();
            List<Transfer> transfers = authService.ViewAllTransfers();

            Console.WriteLine("-------------------------------------------");
            Console.WriteLine("Transfer");
            Console.WriteLine("ID\tFrom/To\t\t\tAmount");
            Console.WriteLine("-------------------------------------------");
            foreach(Transfer transfer1 in transfers)
            {
                string displayName;
                displayName = (userId == transfer1.Account_To) ? $"From:\t{userDAO.GetUserName(transfer1.Account_From)}" : $"To:\t{userDAO.GetUserName(transfer1.Account_To)}";
                Console.WriteLine($"{transfer1.Transfer_Id}\t{displayName}\t\t{transfer1.Amount}");
            }
            Console.WriteLine("-------------------------------------------");
            Console.WriteLine();
            Console.Write("Please enter transfer ID to view details (0 to cancel): ");
            string input = Console.ReadLine();
            int transferId = Convert.ToInt32(input);
            if (transferId == 0)
            {
                return MenuOptionResult.DoNotWaitAfterMenuSelection;
            }
            
            bool isTransferListed = false;
            foreach (Transfer transfer1 in transfers)
            {
                if (transfer1.Transfer_Id == transferId)
                {
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("Transfer Details");
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine();
                    Console.WriteLine($"Id: {transfer1.Transfer_Id}");
                    Console.WriteLine($"From: {transfer1.Account_From}");
                    Console.WriteLine($"To: {transfer1.Account_To}");
                    string type;
                    type = (userId == transfer1.Account_To) ? "Receive" : "Send";
                    Console.WriteLine($"Type: {type}");
                    Console.WriteLine($"Status: Approved");
                    Console.WriteLine($"Amount: ${transfer1.Amount}");
                    isTransferListed = true;
                }
            }
            if (!isTransferListed)
            {
                Console.WriteLine("The ID you entered was not valid!");
            }

            return MenuOptionResult.WaitAfterMenuSelection;
        }

        //private MenuOptionResult ViewRequests()
        //{
        //    Console.WriteLine("Not yet implemented!");
        //    return MenuOptionResult.WaitAfterMenuSelection;
        //}

        private MenuOptionResult SendTEBucks()
        {
            //pull in Transfer
            Transfer transfer = new Transfer();
            // pull in authservice
            AuthService authService = new AuthService();
            // get list of users ids
            List<API_User> list = authService.ListofAvailableUsers();

            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("\tUsers");
            Console.WriteLine("\tID \t\t Name");
            Console.WriteLine("--------------------------------------------");

            foreach (API_User userList in list)
            {

                Console.WriteLine($"\t{userList.UserId} \t\t {userList.Username}");
            }
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine();

            //pull transfer id info and convert user input to an int

            //put in user id logic. 

            //user input logic 

            // enter the amount you need  to send 

            //check against balance. 

            //transactions successful
            return MenuOptionResult.WaitAfterMenuSelection;
        }

        //private MenuOptionResult RequestTEBucks()
        //{
        //    Console.WriteLine("Not yet implemented!");
        //    return MenuOptionResult.WaitAfterMenuSelection;
        //}

        private MenuOptionResult Logout()
        {
            UserService.SetLogin(new API_User()); //wipe out previous login info
            return MenuOptionResult.CloseMenuAfterSelection;
        }

    }
}
