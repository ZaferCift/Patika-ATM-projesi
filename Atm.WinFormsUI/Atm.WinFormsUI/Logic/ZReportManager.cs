using Atm.WinFormsUI.DataAccess;
using Atm.WinFormsUI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Atm.WinFormsUI.Logic
{
    public class ZReportManager
    {
        ZReportDao _zReportDao;
        public ZReportManager()
        {
           _zReportDao=new ZReportDao();
        }

        public bool UpdateTotalDepositMoney(decimal depositAmount)
        {
            var zReport = _zReportDao.GetZReport();
            zReport.TotalDepositMoney += depositAmount;
            _zReportDao.Update(zReport);
            return true;
        }

        public bool UpdateTotalWithdrawMoney(decimal withdrawAmount)
        {
            var zReport=_zReportDao.GetZReport();
            zReport.TotalWithdrawMoney += withdrawAmount;
            _zReportDao.Update(zReport);
            return true;
        }

        public bool UpdateTotalEft(decimal eftAmount)
        {
            var zReport=_zReportDao.GetZReport();
            zReport.TotalEft += eftAmount;
            _zReportDao.Update(zReport);
             return true;
        }
        public bool UpdateTotalBankTransfer(decimal bankTransferAmount)
        {
            var zReport = _zReportDao.GetZReport();
            zReport.TotalBankTransfer += bankTransferAmount;
            _zReportDao.Update(zReport);
            return true;
        }

        public bool UpdateTotalAtmMoney(decimal amount)
        {
            var zReport = _zReportDao.GetZReport();
            zReport.TotalAtmMoney += amount;
            _zReportDao.Update(zReport);
            return true;
        }

        public ZReport GetZreport()
        {
            return _zReportDao.GetZReport();
        }

        public ZReport EndZReport()
        {
            var zReport = _zReportDao.GetZReport();
            _zReportDao.Update(new ZReport());
            zReport.ReportTime = DateTime.Now;
            return zReport;
        }

    }
}
