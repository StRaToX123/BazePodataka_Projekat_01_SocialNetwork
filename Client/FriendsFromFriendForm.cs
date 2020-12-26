using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class FriendsFromFriendForm : Form
    {
        public FriendsFromFriendForm()
        {
            InitializeComponent();
        }

        private void FriendsFromFriendForm_Load(object sender, EventArgs e)
        {

        }

        public void UpdateForm(string data)
        {
            List<string> list = data.Split('@').ToList();
            Action action1 = delegate () {
                this.Name = list[0] + "'s friends";
            };
            this.BeginInvoke(action1);

            Action action2 = delegate () {
            for (int i = 1; i < list.Count; i++)
            {
                if (list[i] != "")
                {
                    listBox1.Items.Add(list[i]);
                }
            }
            };
            listBox1.BeginInvoke(action2);
        }
    }
}
