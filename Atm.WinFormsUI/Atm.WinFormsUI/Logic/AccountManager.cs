using Atm.WinFormsUI.DataAccess;
using Atm.WinFormsUI.Entities;
using Atm.WinFormsUI.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atm.WinFormsUI.Logic
{
    public class AccountManager
    {
        AccountDao _accountDao;
        public AccountManager()
        {
                _accountDao = new AccountDao();
        }

        public int GenerateUniqueAccountNo(out int nextIdResult)
        {
            int nextId = 0;
            var allData = _accountDao.GetAll();
            if (allData!=null)
            {
                nextId = allData.Max(p => p.Id) + 1;
            }
            else
            {

                nextId = 1;
            }
            

            var accountNoStr = DateTime.Now.Day.ToString()+DateTime.Now.Month.ToString()+DateTime.Now.Year.ToString()+nextId.ToString();
            var accountNo = Convert.ToInt32(accountNoStr);
            nextIdResult = nextId;

            return accountNo;

        }

        public Account AddAccount(Account account)
        {
            account.AccountNo=GenerateUniqueAccountNo(out int nextIdResult);
            account.Id=nextIdResult;
            var result=_accountDao.Add(account);
            if (result)
            {
                return account;
            }
            else
            {
                throw new Exception("Bir hata oluştu daha sonra tekrar deneyin.");
            }
            
        }

        public bool RemoveAccount(Account account)
        {
            var accountResult = _accountDao.Get(account.AccountNo);
            if (accountResult!=null)
            {
                if (account.Password != accountResult.Password) throw new Exception("Parolanızı yanlış girdiniz.");

                var deleteResult=_accountDao.Delete(accountResult);
                if (!deleteResult) throw new Exception("Bir hata oluştu daha sonra tekrar deneyiniz.");
                return true;
            }
            else
            {
                throw new Exception("Böyle bir hesap mecvut değil. Hesap numaranızı yanlış girmiş olabilirsiniz.");
            }
            
            
        }

        public bool UpdateAccount(Account account)
        {
            _accountDao.Update(account);
            return true;
        }

        public List<Account> GetAllAccounts()
        {
            return _accountDao.GetAll();
        }

        public Account GetByAccountNo(int accountNo)
        {
            return _accountDao.Get(accountNo);
        }

        public bool DeleteAccount(Account account)
        {
            _accountDao.Delete(account);
            return true;
        }

        public Account Login(Account account)
        {
            var accountToLogin= _accountDao.Get(account.AccountNo);

            if (accountToLogin != null  )
            {
                if (accountToLogin.Password == null || account.Password != accountToLogin.Password) throw new WrongPasswordException("Hesap numarası veya parola geçersiz.");
                return accountToLogin;
            }
            else
            {
                throw new InvalidAccountException("Hesap numarası veya parola geçersiz.");
            }

        }
    }
}
