using Atm.WinFormsUI.DataAccess;
using Atm.WinFormsUI.Entities;
using Atm.WinFormsUI.Entities.LogTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atm.WinFormsUI.Logic
{
    public class LogManager
    {
        LogDao _logDao;
        public LogManager()
        {
               _logDao = new LogDao();
        }
        //public string Location { get; set; }

        public int GenerateId()
        {
            int nextId = 0;
            var allData = _logDao.GetAll();
            if (allData != null)
            {
                nextId = allData.Max(p => p.Id) + 1;
            }
            else
            {

                nextId = 1;
            }

            return nextId;

        }




        public Log AddLog(Log log)
        {
            log.Id = GenerateId();
            
            var result = _logDao.Add(log);
            if (result)
            {
                return log;
            }
            else
            {
                throw new Exception("Bir hata oluştu daha sonra tekrar deneyin.");
            }

        }

        public Log GetLogById(int id)
        {
            return _logDao.Get(id);
        }

        public List<Log> GetAllLogs()
        {
            return _logDao.GetAll();
        }

        
    }
}
