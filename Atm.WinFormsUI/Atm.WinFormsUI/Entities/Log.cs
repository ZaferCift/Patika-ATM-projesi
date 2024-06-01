
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atm.WinFormsUI.Entities.LogTypes
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime LogTime { get; set; }
        public string LogType { get; set; }
        public string LogMessage { get; set; }

        

        public enum LogTypes
        {
            Info,
            Error,
            Warning,
            Danger,               
            BankTransfer,
            DepositMoney,
            Eft,
            WithdrawMoney,
        }

    }
}
