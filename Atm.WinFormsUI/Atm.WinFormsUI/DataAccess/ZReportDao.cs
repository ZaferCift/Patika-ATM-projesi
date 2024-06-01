using Atm.WinFormsUI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atm.WinFormsUI.DataAccess
{
    public class ZReportDao
    {
        AtmContext _context;
        public ZReportDao()
        {
            _context = new AtmContext();
            var result=_context.CheckZReportTableExists();
            if (!result) _context.CreateZReportTable(new ZReport());

        }

        public bool Update(ZReport report)
        {
            
            _context.CreateZReportTable(report);
            return true;
        }

        public ZReport GetZReport() 
        {
            return _context.GetZReportTable();
        }

        
    }
}
