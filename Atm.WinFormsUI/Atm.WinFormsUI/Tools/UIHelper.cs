using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atm.WinFormsUI.Tools
{
    public  class UIHelper
    {
        public GroupBox[] AuthorizedScreenGroupBoxes { get; set; }
        public GroupBox[] CustomerScreenGroupBoxes { get; set; }
        public GroupBox[] MainScreenGroupBoxes { get; set; }

        public void AlignMainScreenGroupBoxes(params GroupBox[] groupBoxes)
        {
            MainScreenGroupBoxes= groupBoxes;
            foreach (var gbx in groupBoxes)
            {
                gbx.Left = 0;
                gbx.Top = 0;
            }
        }
        public void HideAllMainGroupBoxes()
        {
            foreach (var gbx in MainScreenGroupBoxes)
            {
                gbx.Hide();
            }
        }

        public void AlignCustomerGroupBoxes(params GroupBox[] groupBoxes)
        {
            CustomerScreenGroupBoxes = groupBoxes;

            foreach (var gbx in groupBoxes)
            {
                gbx.Left = 182;
                gbx.Top = 85;
            }
        }
        public void HideAllCustomerGroupBoxes()
        {
            foreach (var gbx in CustomerScreenGroupBoxes)
            {
                gbx.Visible = false;
            }
        }

        public void AlignAuthorizedGroupBoxes(params GroupBox[] groupBoxes)
        {
            AuthorizedScreenGroupBoxes = groupBoxes;
            //152x 16 y
            foreach (var groupBox in groupBoxes)
            {

                groupBox.Left = 152;
                groupBox.Top = 16;
            }

        }

        public void HideAllAuthorizedGbx()
        {
            foreach (var gbx in AuthorizedScreenGroupBoxes)
            {
                gbx.Visible = false;
            }
        }

        public void NullOrEmptyValidation(params TextBox[] textBoxes)
        {
            foreach (var textBox in textBoxes)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    throw new Exception("Lütfen tüm alanları doldurunuz.");
                   
                }
            }
        }

        public void ClearTextBoxes(params TextBox[] textBoxes)
        {
            foreach (var tbx in textBoxes)
            {
                tbx.Text = "";
            }
        }

        public void HideGroupBoxes(params GroupBox[] groupBoxes)
        {
            foreach (var gbx in groupBoxes)
            {
                gbx.Hide();
            }
        }

        public void ClearLabels(params Label[] labels)
        {
            foreach (var lbl in labels)
            {
                lbl.Text = "";
            }
        }
    }
}
