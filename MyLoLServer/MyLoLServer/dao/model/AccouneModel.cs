using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLoLServer.dao.model
{
    public class AccountModel
    {
        private string id;
        private string account;
        private string password;

        public string Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Account
        {
            get
            {
                return account;
            }

            set
            {
                account = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public AccountModel()
        {

        }

        public AccountModel(string _id, string _account, string _password)
        {
            Id = _id;
            Account = _account;
            Password = _password;
        }
    }
}
