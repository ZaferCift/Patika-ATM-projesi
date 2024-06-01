using Atm.WinFormsUI.Entities;
using Atm.WinFormsUI.Entities.LogTypes;
using Atm.WinFormsUI.Exceptions;
using Atm.WinFormsUI.Logic;
using Atm.WinFormsUI.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atm.WinFormsUI
{
    public partial class Form1 : Form
    {
        AccountManager _accountManager;
        TransactionManager _transactionManager;
        Account _currentAccount;
        LogManager _logManager;
        ZReportManager _zreportManager;
        UIHelper _uiHelper;

        
        byte _timerCount = 0;
        public Form1()
        {

            InitializeComponent();
            gbxCloseAccount.Left = 372;
            gbxCloseAccount.Top = 25;

            Width = 667;
            Height = 435;
            

            _accountManager = new AccountManager();
            _logManager = new LogManager();
            _zreportManager=new ZReportManager();
            _uiHelper = new UIHelper();

            
            _uiHelper.AlignCustomerGroupBoxes(gbxBankTransfer, gbxDeposit, gbxEft, gbxWithdraw);
            _uiHelper.AlignAuthorizedGroupBoxes(gbxAllAccounts, gbxLogs, gbxZReport,gbxAtmBalance);
            _uiHelper.AlignMainScreenGroupBoxes(gbxLogIn, gbxCustomerScreen, gbxAuthorizedScreen, gbxOnlineServices);
        }

        private void UpdateScreen()
        {
            lblAccNo.Text = _currentAccount.AccountNo.ToString();
            lblCustomerName.Text = _currentAccount.Name + " " + _currentAccount.LastName;
            lblBalanceAmount.Text = _currentAccount.Balance.ToString();

        }

        private void ShowInfo(string info)
        {
            lblInfo.Text = info;
            gbxInfo.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Account account = new Account();



            _accountManager.AddAccount(account);

        }



        private void button1_Click_1(object sender, EventArgs e)
        {

            Account accountToLogin = new Account();

            try
            {
                _uiHelper.NullOrEmptyValidation(tbxAccountNo, tbxPassword);
                accountToLogin.AccountNo = Convert.ToInt32(tbxAccountNo.Text);
                accountToLogin.Password = tbxPassword.Text;

                _currentAccount = _accountManager.Login(accountToLogin);


                var logMessage = _currentAccount.AccountNo + " numaralı hesap oturum açtı.";
                Logger.Log(Log.LogTypes.Info, logMessage);

                UpdateScreen();
                _transactionManager = new TransactionManager(_currentAccount);
                lblEftPrice.Text = _transactionManager.EftPrice.ToString();
                _uiHelper.HideAllMainGroupBoxes();
                gbxCustomerScreen.Show();
                
                tbxAccountNo.Text = "";
                tbxPassword.Text = "";

            }
            catch (InvalidAccountException exception)
            {
                //log.LogType = Log.LogTypes.Danger.ToString();
                //log.LogMessage ="Geçersiz hesap numarası denemesi yapıldı. Girilen değer: "+"'"+tbxAccountNo.Text+"'";
                //_logManager.AddLog(log);

                var logMessage = "Geçersiz hesap numarası denemesi yapıldı. Girilen değer: " + "'" + tbxAccountNo.Text + "'";
                Logger.Log(Log.LogTypes.Danger, logMessage);

                MessageBox.Show(exception.Message);
            }
            catch (WrongPasswordException exception)
            {

                var logMessage = _currentAccount.AccountNo + " numaralı hesap için yanlış parola girişi yapıldı.";
                Logger.Log(Log.LogTypes.Danger, logMessage);


                MessageBox.Show(exception.Message);
            }
            catch (Exception exception)
            {

                var logMessage = "Oturum açma sırasında bir hata oluştu. Exception message: " + "'" + exception.Message + "'";
                Logger.Log(Log.LogTypes.Error, logMessage);

                MessageBox.Show(exception.Message);

            }



        }

        private void btnOnlineSupport_Click(object sender, EventArgs e)
        {
            //Form2 form2 = new Form2();
            //form2.Show();

            _uiHelper.HideAllMainGroupBoxes();
            lblDialog.Text = "Merhaba. Size nasıl yardımcı olabilirim?";
            gbxOnlineServices.Show();
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            //ShowOnlySelectedGbx("gbxWithdraw");
            _uiHelper.HideAllCustomerGroupBoxes();
            gbxWithdraw.Show();
            //var withDraw = _groupBoxes.FirstOrDefault(p => p.Name == "gbxWithdraw");
            //withDraw.Visible = true;


            //var gbxOthers = _groupBoxes.Where(p => p.Name != "gbxWithdraw");

            //foreach (var gbx in gbxOthers)
            //{
            //    gbx.Visible = false;
            //}


        }

        private void btnDeposit_Click(object sender, EventArgs e)
        {
            //ShowOnlySelectedGbx("gbxDeposit");
            _uiHelper.HideAllCustomerGroupBoxes();
            gbxDeposit.Show();
            //var deposit = _groupBoxes.FirstOrDefault(p => p.Name == "gbxDeposit");
            //deposit.Visible = true;


            //var gbxOthers = _groupBoxes.Where(p => p.Name != "gbxDeposit");

            //foreach (var gbx in gbxOthers)
            //{
            //    gbx.Visible = false;
            //}
        }

        private void btnBankTransfer_Click(object sender, EventArgs e)
        {
            //ShowOnlySelectedGbx("gbxBankTransfer");
            _uiHelper.HideAllCustomerGroupBoxes();
            gbxBankTransfer.Show();
            //var bankTransfer = _groupBoxes.FirstOrDefault(p => p.Name == "gbxBankTransfer");
            //bankTransfer.Visible = true;


            //var gbxOthers = _groupBoxes.Where(p => p.Name != "gbxBankTransfer");

            //foreach (var gbx in gbxOthers)
            //{
            //    gbx.Visible = false;
            //}
        }

        private void btnEft_Click(object sender, EventArgs e)
        {
            //ShowOnlySelectedGbx("gbxEft");
            _uiHelper.HideAllCustomerGroupBoxes();
            gbxEft.Show();


        }

        private void btnDepositAccept_Click(object sender, EventArgs e)
        {

            try
            {
                _uiHelper.NullOrEmptyValidation(tbxDeposit);

                var depositAmount = Convert.ToDecimal(tbxDeposit.Text);

                _currentAccount = _transactionManager.Deposit(depositAmount);
                UpdateScreen();
                ShowInfo("Para yatırma işlemi başarılı. Hesabınıza " + depositAmount + " TL yatırdınız.");

                var logMessage = _currentAccount.AccountNo + " numaralı hesaba " + tbxDeposit.Text + "TL yatırıldı.";
                _zreportManager.UpdateTotalDepositMoney(depositAmount);
                Logger.Log(Log.LogTypes.DepositMoney, logMessage);

                tbxDeposit.Text = "";
                gbxDeposit.Hide();

            }
            catch (Exception exception)
            {

                var logMessage = _currentAccount.AccountNo + " numaralı hesaba para yatırılırken bir hata oluştu. Exception Message: " + "'" + exception.Message + "'";
                Logger.Log(Log.LogTypes.Error, logMessage);

                ShowInfo(exception.Message);
            }



        }

        private void btnWithdrawAccept_Click(object sender, EventArgs e)
        {

            try
            {
                _uiHelper.NullOrEmptyValidation(tbxWithdraw);
                var withdrawAmount = Convert.ToDecimal(tbxWithdraw.Text);

                _currentAccount = _transactionManager.WithDraw(withdrawAmount);
                UpdateScreen();
                ShowInfo("Para çekme işlemi başarılı. Hesabınızdan " + withdrawAmount + " TL çektiniz.");

                var logMessage = _currentAccount.AccountNo + " numaralı hesaptan " + withdrawAmount.ToString() + " TL çekildi.";
                _zreportManager.UpdateTotalWithdrawMoney(withdrawAmount);
                Logger.Log(Log.LogTypes.WithdrawMoney, logMessage);
                

                tbxWithdraw.Text = "";
                gbxWithdraw.Hide();
            }
            catch (Exception exception)
            {


                var logMessage = _currentAccount.AccountNo + " numaralı hesaptan para çekilirken hata oluştu. Exception Message: " + "'" + exception.Message + "'";
                Logger.Log(Log.LogTypes.Error, logMessage);

                ShowInfo(exception.Message);
            }

        }

        private void btnEftAccept_Click(object sender, EventArgs e)
        {

            try
            {
                _uiHelper.NullOrEmptyValidation(tbxIbanNo, tbxEftAmount);
                var eftAmount = Convert.ToDecimal(tbxEftAmount.Text);
                var ibanNo = tbxIbanNo.Text;

                _currentAccount = _transactionManager.Eft(ibanNo, eftAmount);
                UpdateScreen();
                ShowInfo(eftAmount + " TL EFT işlemi başarılı. Bu işlem için bankamız hesabınızdan " + _transactionManager.EftPrice + " TL EFT ücreti tahsil etmiştir.");



                var logMessage = _currentAccount.AccountNo + " numaralı hesaptan " + ibanNo.ToString() + " numaralı ibana " + eftAmount + " TL gönderildi.";
                _zreportManager.UpdateTotalEft(eftAmount);
                Logger.Log(Log.LogTypes.Eft, logMessage);

                tbxIbanNo.Text = "";
                tbxEftAmount.Text = "";
                gbxEft.Hide();
            }
            catch (Exception exception)
            {

                var logMessage = _currentAccount.AccountNo + " numaralı hesaptan EFT işlemi yapılırken bir hata oluştu. Exception message: " + "'" + exception.Message + "'";
                Logger.Log(Log.LogTypes.Error, logMessage);

                ShowInfo(exception.Message);
            }

        }

        private void btnBankTransferAccept_Click(object sender, EventArgs e)
        {


            try
            {
                _uiHelper.NullOrEmptyValidation(tbxBankTransferAccount, tbxBankTransferAmount);
                var bankTransferAccount = Convert.ToInt32(tbxBankTransferAccount.Text);
                var bankTransferAmount = Convert.ToDecimal(tbxBankTransferAmount.Text);


                _currentAccount = _transactionManager.BankTransfer(bankTransferAccount, bankTransferAmount);
                var targetAccount = _accountManager.GetByAccountNo(bankTransferAccount);
                UpdateScreen();

                var logMessage = _currentAccount.AccountNo + " numaralı hesaptan " + bankTransferAccount + " numaralı hesaba " + bankTransferAmount + " TL havale yapıldı.";
                _zreportManager.UpdateTotalBankTransfer(bankTransferAmount);
                Logger.Log(Log.LogTypes.BankTransfer, logMessage);

                ShowInfo("Havale işlemi başarılı. Hesabınızdan " + targetAccount.Name + " " + targetAccount.LastName + " hesabına " + bankTransferAmount + " TL havale yaptınız.");
                tbxBankTransferAccount.Text = "";
                tbxBankTransferAmount.Text = "";
                gbxBankTransfer.Hide();
            }
            catch (Exception exception)
            {

                var logMessage = _currentAccount.AccountNo + " numaralı hesaptan havale yapılırken bir hata oluştu. Exception Message: " + "'" + exception.Message + "'";
                Logger.Log(Log.LogTypes.Error, logMessage);

                ShowInfo(exception.Message);
            }
        }

        private void btnInfoOk_Click(object sender, EventArgs e)
        {
            lblInfo.Text = "";
            gbxInfo.Hide();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            
            _uiHelper.ClearLabels(lblCustomerName,lblBalanceAmount,lblAccNo);
            _uiHelper.HideGroupBoxes(gbxDeposit, gbxWithdraw, gbxEft, gbxBankTransfer);

            var logMessage = _currentAccount.AccountNo + " numaralı hesap oturumu kapattı.";
            Logger.Log(Log.LogTypes.Info, logMessage);

            _currentAccount = null;
            _transactionManager = null;
            gbxCustomerScreen.Hide();
            gbxLogIn.Show();

        }

        private void gbxEft_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void tbxNewAccountAccept_Click(object sender, EventArgs e)
        {

        }

        //private void tbxAccNo_TextChanged(object sender, EventArgs e)
        //{

        //}

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxName_TextChanged(object sender, EventArgs e)
        {

        }

        private void tbxNewAccountAccept_Click_1(object sender, EventArgs e)
        {

            try
            {
                
                _uiHelper.NullOrEmptyValidation(tbxName,tbxLastName,tbxNewAccPassword,tbxNewAccPass2);
                Account account = new Account();
                if (tbxNewAccPassword.Text == tbxNewAccPass2.Text)
                {
                    //account.AccountNo = Convert.ToInt32(tbxAccNo.Text);
                    account.Name = tbxName.Text;
                    account.LastName = tbxLastName.Text;
                    account.Password = tbxNewAccPassword.Text;

                    var returnedAccount = _accountManager.AddAccount(account);

                    var logMessage = returnedAccount.AccountNo + " numaralı yeni hesap oluşturuldu.";
                    Logger.Log(Log.LogTypes.Info, logMessage);

                    lblDialog.Text = "  Hesabınız oluşturuldu. Hesap numaranız: " + "'" + returnedAccount.AccountNo.ToString() + "'" + " Hesap numaranızı not etmeyi unutmayınız. Bankamızı tercih ettiğiniz için teşekkür ederiz.";
                    _uiHelper.ClearTextBoxes(tbxName, tbxLastName, tbxNewAccPassword, tbxNewAccPass2);

                }
                else
                {
                    lblDialog.Text = "Girdiğiniz parolalar uyuşmuyor.";
                }

            }
            catch (Exception exception)
            {
                var logMessage = "Hesap oluşturma aşamasında bir hata oluştu. Exception message: " + "'" + exception.Message + "'";
                Logger.Log(Log.LogTypes.Error, logMessage);

                lblDialog.Text = exception.Message;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnNewAccount_Click(object sender, EventArgs e)
        {
            lblDialog.Text = "Tabiki, Yeni hesap bölümünden hesabınızı oluşturabilirsiniz.";
            gbxNewAccount.Show();
            tbxName.Focus();
            gbxCloseAccount.Hide();
        }

        private void tbxAccountCloseAccept_Click(object sender, EventArgs e)
        {

            try
            {
                _uiHelper.NullOrEmptyValidation(tbxCloseAccount, tbxCloseAccPassword);
                Account account = new Account();
                account.AccountNo = Convert.ToInt32(tbxCloseAccount.Text);
                account.Password = tbxCloseAccPassword.Text;

                var currentAccount = _accountManager.GetByAccountNo(account.AccountNo);
                if (currentAccount.Balance > 0) throw new Exception("Şu an hesabınızda " + currentAccount.Balance + " TL bulunmaktadır. Hesabınızı kapatmak için " +
                    "bu miktarı çekmeniz yada transfer etmeniz gerekmektedir. ");

                var result = _accountManager.RemoveAccount(account);

                if (result)
                {
                    var logMessage = tbxCloseAccount.Text + " numaralı hesap kapatıldı.";
                    Logger.Log(Log.LogTypes.Info, logMessage);

                    lblDialog.Text = "Hesabınız başarı ile kapatıldı.";
                    _uiHelper.ClearTextBoxes(tbxCloseAccount, tbxCloseAccPassword);
                }
                
            }
            catch (Exception exception)
            {
                lblDialog.Text = exception.Message;

            }

        }

        private void btnCloseAccount_Click(object sender, EventArgs e)
        {
            lblDialog.Text = "Tabiki, Hesap kapatma bölümünden hesabınızı kapatabilirsiniz.";
            gbxNewAccount.Hide();
            gbxCloseAccount.Show();
            tbxCloseAccount.Focus();
        }

        private void lblDialog_TextChanged(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _timerCount += 1;

            if (panel1.BackColor == Color.Turquoise)
            {
                panel1.BackColor = Color.SteelBlue;
                if (_timerCount == 6)
                {
                    timer1.Enabled = false;
                    _timerCount = 0;

                }
            }
            else
            {
                panel1.BackColor = Color.Turquoise;
            }
        }

        private void gbxAuthorizedScreen_VisibleChanged(object sender, EventArgs e)
        {
            if (gbxAuthorizedScreen.Visible)
            {
                var allAccounts = _accountManager.GetAllAccounts();
                dgvAllAccounts.DataSource = allAccounts;
            }

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var allAccounts = _accountManager.GetAllAccounts();
            dgvAllAccounts.DataSource = allAccounts;
        }

        private void btnAllAccounts_Click(object sender, EventArgs e)
        {
            _uiHelper.HideAllAuthorizedGbx();
            gbxAllAccounts.Show();
        }

        private void btnLogs_Click(object sender, EventArgs e)
        {
            
            var allLogs = _logManager.GetAllLogs();
            dgvLogs.DataSource = allLogs;
            _uiHelper.HideAllAuthorizedGbx();
            gbxLogs.Show();
        }

        private void btnLogDetail_Click(object sender, EventArgs e)
        {
            dgvLogs.Enabled = false;
            gbxLogText.Show();
            btnExport.Enabled = false;
            var logMessage = dgvLogs.SelectedRows[0].Cells["LogMessage"].Value.ToString();
            lblLogText.Text = logMessage;

        }

        private void btnFilter_Click(object sender, EventArgs e)
        {

        }

        private void chkbxDate_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void chkbxLogType_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbxLogType_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cbxLogType.Text))
            {
                var allLogs = _logManager.GetAllLogs();
                var filteredLogs = allLogs.Where(p => p.LogType == cbxLogType.Text).ToList();
                dgvLogs.DataSource = filteredLogs;

            }

        }

        private void dtpFilter_ValueChanged(object sender, EventArgs e)
        {
            var allLogs = _logManager.GetAllLogs();
            var filteredLogs = allLogs.Where(p => p.LogTime.Date == dtpFilter.Value.Date).ToList();
            dgvLogs.DataSource = filteredLogs;
        }

        private void btnLogRefresh_Click(object sender, EventArgs e)
        {
            dgvLogs.DataSource = _logManager.GetAllLogs();
        }

        private void btnLogTextOk_Click(object sender, EventArgs e)
        {
            lblLogText.Text = "";
            gbxLogText.Hide();
            btnExport.Enabled = true;
            dgvLogs.Enabled = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            dgvLogs.Enabled = false;
            if ((List<Log>)dgvLogs.DataSource!=null)
            {
                gbxExportLogs.Show();
                var result = folderBrowserDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    tbxDirectory.Text = folderBrowserDialog.SelectedPath;
                }
            }
            else
            {
                MessageBox.Show("Veri bulunamadı.");
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbxFileName.Text))
                {
                    StreamWriter writer = File.CreateText(tbxDirectory.Text + "\\" + tbxFileName.Text + ".txt");
                    var filteredLogs = (List<Log>)dgvLogs.DataSource;
                    string log = "";
                    foreach (var filteredLog in filteredLogs)
                    {
                        log += filteredLog.Id + " " + filteredLog.LogTime.ToString() + " " + filteredLog.LogType + " " + filteredLog.LogMessage;
                        writer.WriteLine(log);
                        log = "";
                    }
                    writer.Dispose();
                    MessageBox.Show("Dosya başarı ile oluşturuldu.");
                    gbxExportLogs.Hide();
                }
                else
                {
                    MessageBox.Show("Dosya adını giriniz.");
                }

                
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
            


        }

        private void btnZReport_Click(object sender, EventArgs e)
        {
            var zReport = _zreportManager.GetZreport();
            lblTotalDeposit.Text = zReport.TotalDepositMoney.ToString()+"  TL";
            lblTotalWithdraw.Text = zReport.TotalWithdrawMoney.ToString() + "  TL";
            lblTotalEft.Text = zReport.TotalEft.ToString() + "  TL";
            lblTotalBankTransfer.Text = zReport.TotalBankTransfer.ToString() + "  TL";
            lblTotalAtmAmount.Text = zReport.TotalAtmMoney.ToString() + " TL";
            _uiHelper.HideAllAuthorizedGbx();
            gbxZReport.Visible = true;
        }

        private void btnEndZReport_Click(object sender, EventArgs e)
        {
            gbxEndZReport.Show();
            btnEndZReport.Enabled = false;
            var result=folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK) 
            {
                tbxReportExport.Text=folderBrowserDialog.SelectedPath;
            }
        }

        private void btnEndZReportAccept_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(tbxReportName.Text) )
                {
                    var fileFullName = tbxReportExport.Text + "\\" + tbxReportName.Text + ".txt";
                    StreamWriter writer = File.CreateText(fileFullName);
                    var endedZReport = _zreportManager.EndZReport();
                    writer.WriteLine(DateTime.Now+"  Z Raporu");
                    writer.WriteLine(" ");
                    writer.WriteLine("---Mali Rapor---");
                    writer.WriteLine(" ");
                    writer.WriteLine("Toplam Yatırılan Miktar" + "      " + endedZReport.TotalDepositMoney + " TL");
                    writer.WriteLine("Toplam Çekilen Miktar  " + "      " + endedZReport.TotalWithdrawMoney + " TL");
                    writer.WriteLine("Toplam EFT Miktarı     " + "      " + endedZReport.TotalEft + " TL");
                    writer.WriteLine("Toplam Havale Miktarı  " + "      " + endedZReport.TotalBankTransfer + " TL");
                    writer.WriteLine("_____________________________________");
                    writer.WriteLine(" ");
                    writer.WriteLine("Toplam Atm Miktarı     " + "      " + endedZReport.TotalAtmMoney + " TL");
                    writer.Dispose();
                    lblTotalDeposit.Text = "0 TL";
                    lblTotalWithdraw.Text = "0 TL";
                    lblTotalEft.Text = "0 TL";
                    lblTotalBankTransfer.Text = "0 TL";
                    lblTotalAtmAmount.Text = "0 TL";
                    gbxEndZReport.Hide();
                }
                else
                {
                    MessageBox.Show("Dosya adını giriniz.");
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            gbxEndZReport.Hide();
            btnEndZReport.Enabled = true;
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            gbxExportLogs.Hide();
            
            dgvLogs.Enabled = true;
        }

        private void btnAuthorizedAccess_Click(object sender, EventArgs e)
        {
           MessageBox.Show("Yetkili girişi onaylandı.");
           
            _uiHelper.HideAllAuthorizedGbx();
            gbxAuthorizedScreen.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            _uiHelper.HideAllMainGroupBoxes();
            gbxLogIn.Show();
        }

        private void btnOnlineServiceExit_Click(object sender, EventArgs e)
        {
            _uiHelper.HideAllMainGroupBoxes();
            gbxLogIn.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var result= folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK) 
            {
                tbxDirectory.Text=folderBrowserDialog.SelectedPath;
            
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                tbxReportExport.Text = folderBrowserDialog.SelectedPath;

            }
        }

        private void gbxEndZReport_VisibleChanged(object sender, EventArgs e)
        {
            
        }

        private void gbxLogIn_VisibleChanged(object sender, EventArgs e)
        {
            if (gbxLogIn.Visible)
            {
                tbxAccountNo.Focus();
            }
        }

        private void btnAtmBalance_Click(object sender, EventArgs e)
        {
            var zReport=_zreportManager.GetZreport();
            lblAtmAmount.Text = zReport.TotalAtmMoney.ToString();
            _uiHelper.HideAllAuthorizedGbx();
            gbxAtmBalance.Show();


        }

        private void btnMoneyLoad_Click(object sender, EventArgs e)
        {
            try
            {              
                var amount = Convert.ToDecimal(tbxAtmBalance.Text);
                _zreportManager.UpdateTotalAtmMoney(amount);
                Logger.Log(Log.LogTypes.Info, "Atm'ye " + amount + " TL yüklendi.");
                var zReport = _zreportManager.GetZreport();
                lblTotalAtmAmount.Text = zReport.TotalAtmMoney.ToString()+" TL";
                lblAtmAmount.Text = lblTotalAtmAmount.Text;
                
                MessageBox.Show(amount + " TL başarı ile yüklendi. Atm bakiyesi Toplam: " + zReport.TotalAtmMoney + " TL.");
                gbxAtmBalance.Hide();
            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.Message);
            }
            

        }
    }
}
