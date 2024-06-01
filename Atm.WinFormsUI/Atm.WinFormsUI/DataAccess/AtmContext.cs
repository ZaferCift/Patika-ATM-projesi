using Atm.WinFormsUI.Entities;
using Atm.WinFormsUI.Entities.LogTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atm.WinFormsUI.DataAccess
{
    public class AtmContext
    {
        //Accounts________________________________________________________________________
        public bool CheckAccountTableExists()
        {
            if (File.Exists(@"Accounts.json"))
            {

                return true;
            }
            else
            {

                return false;
            }
        }

        public void CreateAccountTable(List<Account> accounts = null)
        {
            if (accounts != null)
            {
                var serializedAccounts = JsonConvert.SerializeObject(accounts);
                StreamWriter file = File.CreateText(@"Accounts.json");
                file.WriteLine(serializedAccounts);
                file.Dispose();

            }
            else
            {

               var file=File.CreateText(@"Accounts.json");
                file.Dispose();
            }

        }

        public List<Account> GetAccountTable()
        {
            StreamReader table = File.OpenText(@"Accounts.json");
            var accounts = JsonConvert.DeserializeObject<List<Account>>(table.ReadToEnd());
            table.Dispose();
            return accounts;
        }



        //Logging____________________________________________________________________

        public bool CheckLogTableExists()
        {
            if (File.Exists(@"Logs.json"))
            {

                return true;
            }
            else
            {

                return false;
            }
        }

        public void CreateLogTable(List<Log> logBases = null)
        {
            if (logBases != null)
            {
                var serializedLogBases = JsonConvert.SerializeObject(logBases);
                StreamWriter file = File.CreateText(@"Logs.json");
                file.WriteLine(serializedLogBases);
                file.Dispose();

            }
            else
            {

                var file = File.CreateText(@"Logs.json");
                file.Dispose();
            }

        }

        public List<Log> GetLogTable()
        {
            StreamReader table = File.OpenText(@"Logs.json");
            var logBases = JsonConvert.DeserializeObject<List<Log>>(table.ReadToEnd());
            table.Dispose();
            return logBases ;
        }

        //ZReport_______________________________________________________________________

        public bool CheckZReportTableExists()
        {
            if (File.Exists(@"ZReport.json"))
            {

                return true;
            }
            else
            {

                return false;
            }
        }

        public void CreateZReportTable(ZReport zReport = null)
        {
            if (zReport != null)
            {
                var serializedZReport = JsonConvert.SerializeObject(zReport);
                StreamWriter file = File.CreateText(@"ZReport.json");
                file.WriteLine(serializedZReport);
                file.Dispose();

            }
            else
            {

                var file = File.CreateText(@"ZReport.json");
                file.Dispose();
            }

        }

        public ZReport GetZReportTable()
        {
            StreamReader table = File.OpenText(@"ZReport.json");
            var zReport = JsonConvert.DeserializeObject<ZReport>(table.ReadToEnd());
            table.Dispose();
            return zReport;
        }

    }
}
