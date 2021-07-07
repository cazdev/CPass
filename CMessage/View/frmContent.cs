using BunifuAnimatorNS;
using CPass.Model;
using CPass.ViewModel;
using System;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CMessage
{
    public partial class frmContent : Form
    {
        #region win api
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion

        private ViewModel viewModel = new ViewModel();
        private readonly string password = ViewModel.pass;

        public frmContent()
        {
            InitializeComponent();
        }

        #region frmContent buttons events
        private void btnAdd_Click(object sender, EventArgs e)
        {
            pnlAddNewEntry.BringToFront();
            BunifuTransition transition = new BunifuTransition();

            if (!viewModel.AddNew)
            {
                viewModel.AddNew = true;
                transition.ShowSync(pnlAddNewEntry, true, BunifuAnimatorNS.Animation.VertSlide);
            }
            else
            {
                viewModel.AddNew = false;
                transition.HideSync(pnlAddNewEntry, true, BunifuAnimatorNS.Animation.VertBlind);
            }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstInfo.Items.Count == 0)
            {
                return;
            }

            pnlAreYouSure.BringToFront();
            BunifuTransition transition = new BunifuTransition();

            if (!viewModel.removeBool)
            {
                viewModel.removeBool = true;
                transition.ShowSync(pnlAreYouSure, true, BunifuAnimatorNS.Animation.VertBlind);
            }
            else
            {
                viewModel.removeBool = false;
                transition.HideSync(pnlAreYouSure, true, BunifuAnimatorNS.Animation.VertBlind);
            }
        }
        private void btnCopy_Click(object sender, EventArgs e)
        {
            pnlCopy.BringToFront();
            BunifuTransition transition = new BunifuTransition();

            if (!viewModel.copyBool)
            {
                viewModel.copyBool = true;
                transition.ShowSync(pnlCopy, true, BunifuAnimatorNS.Animation.VertBlind);
            }
            else
            {
                viewModel.copyBool = false;
                transition.HideSync(pnlCopy, true, BunifuAnimatorNS.Animation.VertBlind);
            }
        }
        private void btnSettings_Click(object sender, EventArgs e)
        {
            pnlSettings.BringToFront();
            BunifuTransition transition = new BunifuTransition();

            if (!viewModel.settingsBool)
            {
                viewModel.settingsBool = true;
                transition.ShowSync(pnlSettings, true, BunifuAnimatorNS.Animation.VertBlind);
            }
            else
            {
                viewModel.settingsBool = false;
                transition.HideSync(pnlSettings, true, BunifuAnimatorNS.Animation.VertBlind);
            }
        }
        #endregion
        #region addNewEntry panel buttons events
        private void btnAddEntry_Click(object sender, EventArgs e)
        {
            if (txtNameEntry.Text != "" && txtUserEntry.Text != "" && txtPassEntry.Text != "")
            {
                viewModel.AddNew = false;

                viewModel.AddEntry(txtNameEntry.Text, txtUserEntry.Text, txtPassEntry.Text);
                AddNewEntryToLstInfo(viewModel.Entries.Last());

                BunifuTransition transition = new BunifuTransition();
                transition.HideSync(pnlAddNewEntry, true, BunifuAnimatorNS.Animation.VertBlind);
                PassEntryResetText();
            }

            SaveAndEncrypt();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            viewModel.AddNew = false;

            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlAddNewEntry, true, BunifuAnimatorNS.Animation.VertBlind);

            PassEntryResetText();
        }
        #endregion
        #region Reset all data panel buttons events
        private void btnWipeAll_Click(object sender, EventArgs e)
        {
            //TODO: May be ask pass before deleting?

            ViewModel.DeleteSerialized();

            Application.Restart();
        }
        private void btnBackWipe_Click(object sender, EventArgs e)
        {
            viewModel.wipeBool = false;
            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlWipe, true, BunifuAnimatorNS.Animation.VertBlind);
        }

        #endregion
        #region settings panel events
        private void btnWipeAll1_Click(object sender, EventArgs e)
        {
            pnlWipe.BringToFront();
            BunifuTransition transition = new BunifuTransition();

            if (!viewModel.wipeBool)
            {
                viewModel.wipeBool = true;
                transition.ShowSync(pnlWipe, true, BunifuAnimatorNS.Animation.VertBlind);
            }
            else
            {
                viewModel.wipeBool = false;
                transition.HideSync(pnlWipe, true, BunifuAnimatorNS.Animation.VertBlind);
            }
        }
        private void btnBackSettings_Click(object sender, EventArgs e)
        {
            viewModel.settingsBool = false;

            pnlSettings.BringToFront();

            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlSettings, true, BunifuAnimatorNS.Animation.VertBlind);
        }
        private void chkSort_OnChange(object sender, EventArgs e)
        {
            if (chkSort.Checked)
            {
                lstInfo.Sorted = true;
            }
            else
            {
                lstInfo.Sorted = false;
            }
        }

        #endregion
        #region copy entry panel events
        private void btnCopyAll_Click(object sender, EventArgs e)
        {
            try
            {
                tmrCopy.Enabled = true;

                Clipboard.SetText(lstInfo.SelectedItem.ToString());

                BunifuTransition transition = new BunifuTransition();
                transition.ShowSync(pnlCopied, true, BunifuAnimatorNS.Animation.VertSlide);
            }
            catch { }
        }
        private void btnCopyUser_Click(object sender, EventArgs e)
        {
            try
            {
                tmrCopy.Enabled = true;

                Clipboard.SetText(lstInfo.SelectedItem.ToString().Substring(lstInfo.SelectedItem.ToString().LastIndexOf("User: ") + 6, lstInfo.SelectedItem.ToString().LastIndexOf(", Pass: ") - lstInfo.SelectedItem.ToString().LastIndexOf("User: ") - 6));

                BunifuTransition transition = new BunifuTransition();
                transition.ShowSync(pnlCopied, true, BunifuAnimatorNS.Animation.VertSlide);
            }
            catch { }
        }
        private void btnCopyPass_Click(object sender, EventArgs e)
        {
            try
            {
                tmrCopy.Enabled = true;

                Clipboard.SetText(lstInfo.SelectedItem.ToString().Substring(lstInfo.SelectedItem.ToString().LastIndexOf("Pass: ") + 6));

                BunifuTransition transition = new BunifuTransition();
                transition.ShowSync(pnlCopied, true, BunifuAnimatorNS.Animation.VertSlide);
            }
            catch { }
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            viewModel.copyBool = false;
            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlCopy, true, BunifuAnimatorNS.Animation.VertBlind);
        }
        #endregion
        #region Deleting confirm panel
        private void btnDelete_Click(object sender, EventArgs e)
        {
            viewModel.removeBool = false;

            for (int i = 0; i < viewModel.Entries.Count; i++)
            {
                if (viewModel.Entries[i].ToString() == lstInfo.SelectedItem.ToString())
                {
                    viewModel.Entries.RemoveAt(i);
                    break;
                }
            }

            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlAreYouSure, true, BunifuAnimatorNS.Animation.VertBlind);

            SaveAndEncrypt();
            UpdateLstInfo();
        }
        private void btnNo_Click(object sender, EventArgs e)
        {
            viewModel.removeBool = false;

            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlAreYouSure, true, BunifuAnimatorNS.Animation.VertBlind);
        }

        #endregion
        #region password input events
        private void bunifuMetroTextbox1_OnValueChanged(object sender, EventArgs e)
        {
            if (!System.IO.File.Exists(System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/pass.set")))
            {
                if (txtPass.Text.Any(char.IsDigit))
                {
                    lblHint1.Visible = false;
                }
                else
                {
                    lblHint1.Visible = true;
                }

                if (txtPass.Text.Length < 5)
                {
                    lblHint.Visible = true;
                }
                else
                {
                    lblHint.Visible = false;
                }

                if (!lblHint.Visible && !lblHint1.Visible)
                {
                    txtPass.BorderColorFocused = Color.LimeGreen;
                    txtPass.BorderColorMouseHover = Color.LimeGreen;
                    txtPass.BorderColorIdle = Color.LimeGreen;
                }
                else
                {
                    txtPass.BorderColorFocused = Color.Crimson;
                    txtPass.BorderColorMouseHover = Color.Crimson;
                    txtPass.BorderColorIdle = Color.Crimson;
                }
            }
        }
        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {// event on pressing enter on password input form
                HideAll();
                e.Handled = true;

                viewModel = ViewModel.Deserialize();

                if (viewModel.UserPassword == "")
                {//first start
                    viewModel.UserPassword = txtPass.Text;
                }
                else
                {
                    if (viewModel.UserPassword != txtPass.Text)
                    {
                        flash = 0;
                        Thread flashing = new Thread(Flash);
                        flashing.Start();
                        return;
                    }
                }
                //hide pass input pnl
                iconTick.Visible = true;
                pnlContent.Visible = true;
                pnlContent.Hide();
                BunifuTransition transition = new BunifuTransition();
                transition.ShowSync(pnlContent, true, BunifuAnimatorNS.Animation.Transparent);
                //

                UpdateLstInfo();
            }
        }

        #endregion
        #region Graphics events
        private void lstInfo_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
            {
                return;
            }
            //if the item state is selected them change the back color 
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          Color.Teal);//Choose the color
            }

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Draw the current item text
            e.Graphics.DrawString(lstInfo.Items[e.Index].ToString(), e.Font, Brushes.Silver, e.Bounds, StringFormat.GenericDefault);
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
        }
        private int flash = 0;
        private void Flash()
        {//flashing when password is wrong
            bool callAgain = true;

            Task task = new Task(() =>
            {
                txtPass.Invoke(new MethodInvoker(delegate //to exept thead exeption
                {
                    if (flash % 2 == 0)
                    {
                        txtPass.BorderColorFocused = Color.Crimson;
                        txtPass.BorderColorMouseHover = Color.Crimson;
                        txtPass.BorderColorIdle = Color.Crimson;
                    }
                    else
                    {
                        txtPass.BorderColorFocused = Color.Gray;
                        txtPass.BorderColorMouseHover = Color.Gray;
                        txtPass.BorderColorIdle = Color.Gray;
                    }

                    flash++;

                    if (flash > 4)
                    {
                        callAgain = false;

                        txtPass.BorderColorFocused = Color.Teal;
                        txtPass.BorderColorMouseHover = Color.Teal;
                        txtPass.BorderColorIdle = Color.Gray;
                    }
                }));

                if (callAgain)
                {
                    Thread.Sleep(200);//interval
                    Flash();
                }
            });
            task.Start();
        }
        private void HideAll()
        {
            pnlAddNewEntry.Hide();
            pnlAreYouSure.Hide();
            pnlCopy.Hide();
            pnlCopied.Hide();
            pnlSettings.Hide();
            pnlWipe.Hide();
            pnlLoading.Hide();
        }
        private void txtSearch_OnTextChange(object sender, EventArgs e)
        {
            if (txtSearch.text != "")
            {
                lstInfo.SelectionMode = SelectionMode.MultiSimple;
                for (int i = 0; i < lstInfo.Items.Count; i++)
                {
                    if (lstInfo.Items[i].ToString().IndexOf(txtSearch.text, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        lstInfo.SetSelected(i, true);
                    }
                    else
                    {
                        // Do this if you want to select in the ListBox only the results of the latest search.
                        lstInfo.SetSelected(i, false);
                    }
                }
            }
            else
            {
                lstInfo.SelectionMode = SelectionMode.One;
                lstInfo.ClearSelected();
            }
        }
        private void pnlContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private void tmrLoading_Tick(object sender, EventArgs e)
        {
            if (progLoading.Value < 100 && !viewModel.switchLoading)
            {
                progLoading.Value += 2;
            }
            else
            {
                viewModel.switchLoading = true;
            }

            if (progLoading.Value > 10 && viewModel.switchLoading)
            {
                progLoading.Value -= 2;
            }
            else
            {
                viewModel.switchLoading = false;
            }
        }
        private void tmrCopy_Tick(object sender, EventArgs e)
        {
            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlCopied, true, BunifuAnimatorNS.Animation.VertSlide);

            tmrCopy.Enabled = false;
        }
        private void frmContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        #endregion
        #region methods
        private void SaveAndEncrypt()
        {
            #region
            //old style: (can be removed)
            //StringBuilder sb = new StringBuilder();
            //
            //foreach (object o in lstInfo.Items)
            //{
            //    sb.AppendLine(StringCipher.Encrypt(o.ToString(), password));
            //}
            //
            //System.IO.File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/info.enc"), sb.ToString());
            #endregion
            ViewModel.Serialize(viewModel);
        }
        public void UpdateLstInfo()
        {
            lstInfo.Items.Clear();
            for (int i = 0; i < viewModel.Entries.Count; i++)
            {
                lstInfo.Items.Add(viewModel.Entries[i].ToString());
            }
        }
        private void PassEntryResetText()
        {
            txtNameEntry.ResetText();
            txtUserEntry.ResetText();
            txtPassEntry.ResetText();
        }
        public void AddNewEntryToLstInfo(Entry entry)
        {
            lstInfo.Items.Add(entry.ToString());
        }

        #endregion
        #region load / exit
        private void frmContent_Load(object sender, EventArgs e)
        {
            HideAll();

            if (ViewModel.Deserialize().UserPassword != "")
            {
                lblTitle.Text = "Enter your password";

                lblHint.Visible = false;
                lblHint1.Visible = false;

                txtPass.BorderColorFocused = Color.Teal;
                txtPass.BorderColorMouseHover = Color.Teal;
                txtPass.BorderColorIdle = Color.Gray;
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlContent.Hide();
            pnlLoading.Show();

            SaveAndEncrypt();

            tmrClose.Enabled = true;
        }

        private void tmrClose_Tick(object sender, EventArgs e)
        {
            ViewModel.Serialize(viewModel);
            Environment.Exit(0);
        }
        private void btnClose2_Click(object sender, EventArgs e)
        {
            ViewModel.Serialize(viewModel);
            Environment.Exit(0);
        }
        #endregion
    }
}