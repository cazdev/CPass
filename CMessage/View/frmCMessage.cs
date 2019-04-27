using BunifuAnimatorNS;
using System;
using System.Windows.Forms;

namespace CMessage
{
    public partial class frmCMessage : Form
    {
        public frmCMessage()
        {
            InitializeComponent();
        }

        private void tmrMainProgress_Tick(object sender, EventArgs e)
        {

            if (mainProgress.Value < 100)
            {
                mainProgress.Value += 2;
            }
            else
            {
                tmrMainProgress.Stop();

                BunifuTransition transition = new BunifuTransition();
                frmContent frm = new frmContent
                {
                    Location = Location,
                    TopMost = true
                };
                frm.Show();
                Hide();
                frm.TopMost = false;
            }

        }
    }
}
