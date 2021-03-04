﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TenmoServer.DAO;
using TenmoServer.Models;

namespace TenmoServer.Controllers
{
    [Route("/account")]
    [ApiController]
    //[Authorize]
    public class AccountController : ControllerBase
    {
        private IAccountDAO accountDAO;
        private IUserDAO userDAO;

        public AccountController(IAccountDAO accountDAO, IUserDAO userDAO)
        {
            this.accountDAO = accountDAO;
            this.userDAO = userDAO;
        }

        [HttpGet("users")]
        public ActionResult<Account> GetAccount(int userId)
        {
            Account account = accountDAO.GetAccount(userId);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                return account;
            }
        }

        [HttpGet]
        public ActionResult<List<User>> GetUsers()
        {
            List<User> users = userDAO.GetUsers();
            if (users == null)
            {
                return NotFound();
            }
            else
            {
                return users;
            }
        }

        [HttpGet("transfers")]
        public ActionResult<List<Transfer>> GetTransfers(int userId)
        {
            List<Transfer> transfers = accountDAO.GetTransfers(userId);
            if (transfers == null)
            {
                return NotFound();
            }
            else
            {
                return transfers;
            }
        }

        [HttpPut]
        public ActionResult Transfer(Transfer transfer)
        {
            int userId = transfer.Account_From;
            int accountToId = transfer.Account_To;
            decimal amount = transfer.Amount;

            accountDAO.TransferToRegUser(userId, accountToId, amount);
            return Ok();
        }
    }
}