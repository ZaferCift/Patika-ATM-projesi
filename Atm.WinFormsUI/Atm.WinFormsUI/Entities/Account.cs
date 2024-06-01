using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atm.WinFormsUI.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public int AccountNo { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }
    }
}
