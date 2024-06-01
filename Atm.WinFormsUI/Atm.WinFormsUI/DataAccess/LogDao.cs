using Atm.WinFormsUI.Entities;
using Atm.WinFormsUI.Entities.LogTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atm.WinFormsUI.DataAccess
{
    public class LogDao
    {
        AtmContext _context;
        public LogDao() 
        {
            _context=new AtmContext();
            if (!_context.CheckLogTableExists())
            {
                _context.CreateLogTable();
            }
        }

        public Log Get(int logId)
        {
            var logs = _context.GetLogTable();

            if (logs != null)
            {
                var result = logs.FirstOrDefault(p => p.Id == logId);
                return result;
            }
            else
            {
                return null;
            }


        }

        public bool Add(Log logBase)
        {
            var logBases = _context.GetLogTable();

            if (logBases==null)
            {
                logBases=new List<Log>();
            }
            logBases.Add(logBase);

            _context.CreateLogTable(logBases);
            return true;

        }

        //public bool Delete(LogBase logBase)
        //{
        //    var logs = _context.GetAccountTable();
        //    //var accountResult = accounts.FirstOrDefault(p => p.AccountNo == account.AccountNo);
        //    var logIndex = logs.FindIndex(p => p.Id == logBase.Id);
        //    logs.RemoveAt(logIndex);

        //    _context.CreateAccountTable(logs);

        //    return true;
        //}

        public List<Log> GetAll()
        {


            return _context.GetLogTable();
        }
    }
}
