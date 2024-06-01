using Atm.WinFormsUI.Entities;
using Atm.WinFormsUI.Entities.LogTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atm.WinFormsUI.Logic
{
    public class TransactionManager
    {
        Account _currentAccount;
        AccountManager _accountManager;
        LogManager _logManager;
        ZReportManager _zReportManager;

        private decimal _eftPrice=4;
        public decimal EftPrice { get { return _eftPrice; }  }
        public TransactionManager(Account currentAccount)
        {
            _currentAccount = currentAccount;
            _accountManager = new AccountManager();
            _logManager = new LogManager();
            _zReportManager = new ZReportManager();
        }
        public Account Eft(string ibanNo, decimal amount)
        {
            

            if (_currentAccount.Balance >= amount+_eftPrice)
            {
                _currentAccount.Balance -= amount+_eftPrice;
                var result=_accountManager.UpdateAccount(_currentAccount);
                if (result)
                {

                    return _currentAccount;
                }
                else
                {
                    throw new Exception("Bir hata oluştu.Tekrar deneyin.");
                }
                
            }
            else
            {
                throw new Exception("Bakiye yetersiz.");
            }
            
        }

        public Account BankTransfer(int targetAccountNo, decimal amount)
        {


            var targetAccount=_accountManager.GetByAccountNo(targetAccountNo);

            if (targetAccount!=null)
            {
                if (_currentAccount.Balance >= amount)
                {
                    _currentAccount.Balance -= amount;
                    targetAccount.Balance += amount;

                    var result = _accountManager.UpdateAccount(_currentAccount);
                    if (result)
                    {
                        _accountManager.UpdateAccount(targetAccount);
                        return _currentAccount;

                    }
                    else
                    {
                        throw new Exception("Bir hata oluştu.Tekrar deneyin");
                    }



                }
                else
                {
                    throw new Exception("Bakiye yetersiz.");
                }

            }
            else
            {
                throw new Exception("Böyle bir hesap mevcut değil");
            }
            
            
            
        }

        public Account Deposit(decimal amount)
        {

            _currentAccount.Balance += amount;
            var result = _accountManager.UpdateAccount(_currentAccount);
            
            if (result)
            {
                _zReportManager.UpdateTotalAtmMoney(_currentAccount.Balance);
                return _currentAccount;
            }
            else
            {
                throw new Exception("Bir hata oluştu.Tekrar deneyin.");
            }
            
                
            

            
        }
        public Account WithDraw(decimal amount)
        {

            var atmTotalAmount = _zReportManager.GetZreport().TotalAtmMoney;
            
            if (_currentAccount.Balance>=amount)
            {
                if (amount > atmTotalAmount) throw new Exception("Atm bakiyesi yetersiz. Para yüklenmesi bekleniyor.");
                _currentAccount.Balance -= amount;

                var result=_accountManager.UpdateAccount(_currentAccount);

                if (result)
                {
                    _zReportManager.UpdateTotalAtmMoney(-amount);
                    return _currentAccount;
                }
                else
                {
                    throw new Exception("Bir hata oluştu.Tekrar deneyin");
                }
                
            }
            else
            {
                throw new Exception("Bakiye yetersiz.");
            }
            
            
        }

        
    }
}
