using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO
    {
        private readonly string connectionString;
        private const string SQL_GETBALANCEFROMUSERID = "Select balance from accounts where user_id = @userid";

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public decimal GetAccountBalance(int userId)
        {//Want to use userid because if someone knew your username, doesn't mean they should get their accountbalance

            Account account = new Account();
            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(SQL_GETBALANCEFROMUSERID, conn);
                cmd.Parameters.AddWithValue("@userid", userId);

                SqlDataReader rdr = cmd.ExecuteReader();

                while(rdr.Read())
                {
                     account = (RowToObject(rdr));
                }
            }

            return account.Balance;
        }

        private Account RowToObject(SqlDataReader rdr)
        {
            Account account = new Account();
            account.Account_Id = Convert.ToInt32(rdr["account_id"]);
            account.Balance = Convert.ToDecimal(rdr["balance"]);
            account.User_Id = Convert.ToInt32(rdr["user_id"]);
            return account;
        }
    }
}
