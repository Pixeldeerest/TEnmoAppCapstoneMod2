﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.Models;

namespace TenmoServer.DAO
{
    public class AccountSqlDAO : IAccountDAO
    {
        private readonly string connectionString;

        public AccountSqlDAO(string dbConnectionString)
        {
            connectionString = dbConnectionString;
        }

        public Account GetAccount(int userId)
        {
            Account account = new Account();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(@"Select * from accounts where user_id = @userid", conn);
                cmd.Parameters.AddWithValue("@userid", userId);

                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    account = RowToAccount(rdr);
                }
            }
            return account;
        }


        public void TransferToRegUser(int userId, int accountToId, decimal amount)
        {
            Account account = GetAccount(userId);

            if (account.Balance >= amount)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionString))
                    {
                        conn.Open();

                        SqlCommand cmd = new SqlCommand(@"
                            BEGIN TRANSACTION
                            INSERT INTO transfers (transfer_type_id, transfer_status_id, account_from, account_to, amount) 
                                VALUES ((select transfer_type_id from transfer_types where transfer_type_desc = 'Send'), 
                                (Select transfer_status_id from transfer_statuses where transfer_status_desc = 'Approved'), 
                                @userId, @accountToId, @amount)
                            UPDATE accounts set balance = balance - @amount where account_id = @userId
                            UPDATE account set balance = balance + @amount where account_id = @accountToId
                            COMMIT TRANSACTION
                            ", conn);
                        cmd.Parameters.AddWithValue("@userId", userId);
                        cmd.Parameters.AddWithValue("@accountToId", accountToId);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException ex)
                {
                    throw;
                }
            }
            else
            {
                Console.WriteLine("YA BROKE!");
            }
        }

        public List<Transfer> GetTransfers(int transferId)
        {
            List<Transfer> transfers = new List<Transfer>();

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(@"SELECT * FROM transfers" /*WHERE transfer_Id = @transferId*/, conn);
                    cmd.Parameters.AddWithValue("@transferId", transferId);

                    SqlDataReader rdr = cmd.ExecuteReader();

                    while (rdr.Read())
                    {
                        Transfer transfer = RowToTransfer(rdr);
                        transfers.Add(transfer);

                    }
                }
            }
            catch (SqlException ex)
            {
                throw;
            }

            return transfers;
        }

        private Transfer RowToTransfer(SqlDataReader reader)
        {
            Transfer transfer = new Transfer();
            transfer.Transfer_Id = Convert.ToInt32(reader["transfer_id"]);
            transfer.Account_From = Convert.ToInt32(reader["account_from"]);
            transfer.Account_To = Convert.ToInt32(reader["account_to"]);
            transfer.Amount = Convert.ToDecimal(reader["amount"]);
            transfer.Transfer_Type_Id = Convert.ToInt32(reader["transfer_type_id"]);
            transfer.Transfer_Status_Id = Convert.ToInt32(reader["transfer_status_id"]);
            return transfer;
        }

        private Account RowToAccount(SqlDataReader rdr)
        {
            Account account = new Account();
            account.Account_Id = Convert.ToInt32(rdr["account_id"]);
            account.Balance = Convert.ToDecimal(rdr["balance"]);
            account.User_Id = Convert.ToInt32(rdr["user_id"]);
            return account;
        }
    }
}
