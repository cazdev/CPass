using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.IO;
using BunifuAnimatorNS;
using Bunifu;
using System.Security;
using System.Net;

namespace CMessage
{
    public partial class frmContent : Form
    {
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        private SecureString SecString = new NetworkCredential("", "TljO3NBhu088VIE36VSAmzJqfo1qSKFg48D9VzHAAythPimmy/+KRCys1pqBptpqB4WEczUNbVG645WSWy77cCgcW7v05ur+qyWzF+ZlIA/fIhdwCJG8s41oEWkT58re").SecurePassword;
        
        public frmContent()
        {
            InitializeComponent();
        }

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

        private void frmContent_Load(object sender, EventArgs e)
        {
            HideAll();

            if (File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/pass.set")))
            {
                lblTitle.Text = "Enter your password";
                lblHint.Visible = false;
                lblHint1.Visible = false;

                txtPass.BorderColorFocused = Color.Teal;
                txtPass.BorderColorMouseHover = Color.Teal;
                txtPass.BorderColorIdle = Color.Gray;
            }
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

        private void txtPass_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                HideAll();

                e.Handled = true;

                if (!File.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/pass.set")))
                {
                    string plaintext = txtPass.Text;

                    string encryptedstring = "";
                    string decryptedstring = "";
                    
                        encryptedstring = StringCipher.Encrypt(plaintext, new NetworkCredential("", SecString).Password);
                        decryptedstring = StringCipher.Decrypt(encryptedstring, new NetworkCredential("", SecString).Password);

                    string pathPass = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/pass.key");
                    string pathInfo = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/info.enc");

                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass"));

                    var fileStream1 = File.Create(pathPass);
                    fileStream1.Close();
                    var fileStream2 = File.Create(pathInfo);
                    fileStream2.Close();

                    TextWriter tw = new StreamWriter(pathPass);
                    tw.WriteLine(encryptedstring);
                    tw.Close();

                    try
                    {
                        File.Encrypt(pathPass);
                        File.Encrypt(pathInfo);
                    }
                    catch { }

                    var fileStream3 = File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/pass.set"));
                    fileStream3.Close();

                    iconTick.Visible = true;
                    pnlContent.Visible = true;
                    pnlContent.Hide();

                    BunifuTransition transition = new BunifuTransition();
                    transition.ShowSync(pnlContent, true, BunifuAnimatorNS.Animation.Transparent);
                }
                else
                {
                    string encryptedstring = "";
                    string decryptedstring = "";
                    
                        encryptedstring = File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/pass.key"));
                        decryptedstring = StringCipher.Decrypt(encryptedstring, new NetworkCredential("", SecString).Password);

                    if (txtPass.Text == decryptedstring)
                    {
                        iconTick.Visible = true;
                        pnlContent.Visible = true;
                        pnlContent.Hide();
                        
                        using (StreamReader r = new StreamReader(File.Open(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/info.enc"), FileMode.Open)))
                        {
                            string line;
                            while ((line = r.ReadLine()) != null)
                            {
                                lstInfo.Items.Add(StringCipher.Decrypt(line, new NetworkCredential("", SecString).Password));
                            }
                        }

                        BunifuTransition transition = new BunifuTransition();
                        transition.ShowSync(pnlContent, true, BunifuAnimatorNS.Animation.Transparent);
                    }
                    else
                    {
                        flash = 0;
                        tmrFlash.Enabled = true;
                    }
                }
            }
        }

        private void lstInfo_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            //if the item state is selected them change the back color 
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e = new DrawItemEventArgs(e.Graphics,
                                          e.Font,
                                          e.Bounds,
                                          e.Index,
                                          e.State ^ DrawItemState.Selected,
                                          e.ForeColor,
                                          Color.Teal);//Choose the color

            // Draw the background of the ListBox control for each item.
            e.DrawBackground();
            // Draw the current item text
            e.Graphics.DrawString(lstInfo.Items[e.Index].ToString(), e.Font, Brushes.Silver, e.Bounds, StringFormat.GenericDefault);
            // If the ListBox has focus, draw a focus rectangle around the selected item.
            e.DrawFocusRectangle();
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

        private int flash = 0;
        private void tmrFlash_Tick(object sender, EventArgs e)
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
                tmrFlash.Enabled = false;

                txtPass.BorderColorFocused = Color.Teal;
                txtPass.BorderColorMouseHover = Color.Teal;
                txtPass.BorderColorIdle = Color.Gray;
            }
        }

        private void SaveAndEncrypt()
        {
            StringBuilder sb = new StringBuilder();

            foreach (Object o in lstInfo.Items)
            {
                sb.AppendLine(StringCipher.Encrypt(o.ToString(), new NetworkCredential("", SecString).Password));
            }
            
            System.IO.File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/info.enc"), sb.ToString());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlContent.Hide();
            pnlLoading.Show();

            SaveAndEncrypt();

            tmrClose.Enabled = true;
        }

        bool AddNew = false;

        private void btnAdd_Click(object sender, EventArgs e)
        {
            pnlAddNewEntry.BringToFront();

            if (!AddNew)
            {
                AddNew = true;
                BunifuTransition transition = new BunifuTransition();
                transition.ShowSync(pnlAddNewEntry, true, BunifuAnimatorNS.Animation.VertSlide);
            }
            else
            {
                AddNew = false;
                BunifuTransition transition = new BunifuTransition();
                transition.HideSync(pnlAddNewEntry, true, BunifuAnimatorNS.Animation.VertBlind);
            }
        }

        private void btnAddEntry_Click(object sender, EventArgs e)
        {
            if (txtNameEntry.Text != "" && txtUserEntry.Text != "" && txtPassEntry.Text != "")
            {
                AddNew = false;

                lstInfo.Items.Add(txtNameEntry.Text + " - " + "User: " + txtUserEntry.Text + ", " + "Pass: " + txtPassEntry.Text);

                BunifuTransition transition = new BunifuTransition();
                transition.HideSync(pnlAddNewEntry, true, BunifuAnimatorNS.Animation.VertBlind);

                txtNameEntry.ResetText();
                txtUserEntry.ResetText();
                txtPassEntry.ResetText();
            }

            SaveAndEncrypt();
        }

        private void pnlContent_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            AddNew = false;

            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlAddNewEntry, true, BunifuAnimatorNS.Animation.VertBlind);

            txtNameEntry.ResetText();
            txtUserEntry.ResetText();
            txtPassEntry.ResetText();
        }

        bool removeBool = false;

        private void btnRemove_Click(object sender, EventArgs e)
        {
            pnlAreYouSure.BringToFront();

            if (!removeBool)
            {
                removeBool = true;
                BunifuTransition transition = new BunifuTransition();
                transition.ShowSync(pnlAreYouSure, true, BunifuAnimatorNS.Animation.VertBlind);
            }
            else
            {
                removeBool = false;
                BunifuTransition transition = new BunifuTransition();
                transition.HideSync(pnlAreYouSure, true, BunifuAnimatorNS.Animation.VertBlind);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            removeBool = false;

            lstInfo.Items.Remove(lstInfo.SelectedItem);

            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlAreYouSure, true, BunifuAnimatorNS.Animation.VertBlind);

            SaveAndEncrypt();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            removeBool = false;

            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlAreYouSure, true, BunifuAnimatorNS.Animation.VertBlind);
        }

        bool copyBool = false;

        private void btnCopy_Click(object sender, EventArgs e)
        {
            pnlCopy.BringToFront();

            if (!copyBool)
            {
                copyBool = true;
                BunifuTransition transition = new BunifuTransition();
                transition.ShowSync(pnlCopy, true, BunifuAnimatorNS.Animation.VertBlind);
            }
            else
            {
                copyBool = false;
                BunifuTransition transition = new BunifuTransition();
                transition.HideSync(pnlCopy, true, BunifuAnimatorNS.Animation.VertBlind);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            copyBool = false;
            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlCopy, true, BunifuAnimatorNS.Animation.VertBlind);
        }

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

        private void btnBackSettings_Click(object sender, EventArgs e)
        {
            settingsBool = false;

            pnlSettings.BringToFront();

            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlSettings, true, BunifuAnimatorNS.Animation.VertBlind);
        }

        bool settingsBool = false;

        private void btnSettings_Click(object sender, EventArgs e)
        {
            pnlSettings.BringToFront();

            if (!settingsBool)
            {
                settingsBool = true;
                BunifuTransition transition = new BunifuTransition();
                transition.ShowSync(pnlSettings, true, BunifuAnimatorNS.Animation.VertBlind);
            }
            else
            {
                settingsBool = false;
                BunifuTransition transition = new BunifuTransition();
                transition.HideSync(pnlSettings, true, BunifuAnimatorNS.Animation.VertBlind);
            }
        }

        bool wipeBool = false;

        private void btnWipeAll1_Click(object sender, EventArgs e)
        {
            pnlWipe.BringToFront();

            if (!wipeBool)
            {
                wipeBool = true;
                BunifuTransition transition = new BunifuTransition();
                transition.ShowSync(pnlWipe, true, BunifuAnimatorNS.Animation.VertBlind);
            }
            else
            {
                wipeBool = false;
                BunifuTransition transition = new BunifuTransition();
                transition.HideSync(pnlWipe, true, BunifuAnimatorNS.Animation.VertBlind);
            }
        }

        private void btnBackWipe_Click(object sender, EventArgs e)
        {
            wipeBool = false;
            BunifuTransition transition = new BunifuTransition();
            transition.HideSync(pnlWipe, true, BunifuAnimatorNS.Animation.VertBlind);
        }

        private void btnWipeAll_Click(object sender, EventArgs e)
        {
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/info.enc"));
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/pass.key"));
                File.Delete(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CPass/pass.set"));

            Application.Restart();
        }

        bool switchLoading = false;

        private void tmrLoading_Tick(object sender, EventArgs e)
        {
            if (progLoading.Value < 100 && !switchLoading)
            {
                progLoading.Value += 2;
            }
            else
            {
                switchLoading = true;
            }

            if (progLoading.Value > 10 && switchLoading)
            {
                progLoading.Value -= 2;
            }
            else
            {
                switchLoading = false;
            }
        }

        private void tmrClose_Tick(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnClose2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

