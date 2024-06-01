using Atm.WinFormsUI.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.XPath;

namespace Atm.WinFormsUI.DataAccess
{
    public class AccountDao
    {
        AtmContext _context;
        
        public AccountDao()
        {
            _context = new AtmContext();

            if (!_context.CheckAccountTableExists())
            {
                _context.CreateAccountTable();
            }


        }


        public Account Get(int accountNo)
        {
            var accounts = _context.GetAccountTable();

            if (accounts!=null)
            {
                var result = accounts.FirstOrDefault(p => p.AccountNo == accountNo);
                return result;
            }
            else
            {
                return null;
            }


        }
        public bool Add(Account account)
        {
            var accounts = _context.GetAccountTable();
            if (accounts == null)
            {
                accounts = new List<Account>();
            }
            accounts.Add(account);


            _context.CreateAccountTable(accounts);
            return true;

        }

        public bool Delete(Account account)
        {
            var accounts = _context.GetAccountTable();
            //var accountResult = accounts.FirstOrDefault(p => p.AccountNo == account.AccountNo);
            var accountIndex=accounts.FindIndex(p=>p.AccountNo==account.AccountNo);
            accounts.RemoveAt(accountIndex);
            
            _context.CreateAccountTable(accounts);

            return true;
        }

        public bool Update(Account account)
        {
            var accounts = _context.GetAccountTable();
            
            accounts.Remove(accounts.FirstOrDefault(p => p.AccountNo == account.AccountNo));
            accounts.Add(account);
            _context.CreateAccountTable(accounts);

            return true;
        }

        public List<Account> GetAll()
        {


            return _context.GetAccountTable();
        }




    }
}
