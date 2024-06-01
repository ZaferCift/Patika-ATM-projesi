using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atm.WinFormsUI.Entities
{
    public class ZReport
    {
        public DateTime ReportTime { get; set; }
        public decimal TotalDepositMoney { get; set; } = 0;
        public decimal TotalWithdrawMoney { get; set; } = 0;
        public decimal TotalEft { get; set; } = 0;
        public decimal TotalBankTransfer { get; set; } = 0;
        public decimal TotalAtmMoney { get; set; } = 0;
    }
}
