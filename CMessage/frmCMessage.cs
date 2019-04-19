using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bunifu;
using BunifuAnimatorNS;

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
                frmContent frm = new frmContent();
                frm.Location = this.Location;
                frm.TopMost = true;
                frm.Show();
                this.Hide();
                frm.TopMost = false;
            }
           
        }
    }
}
