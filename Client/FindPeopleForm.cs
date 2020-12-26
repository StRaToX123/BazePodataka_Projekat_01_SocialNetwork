using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.AspNet.SignalR.Client;

namespace Client
{
    public partial class FindPeopleForm : Form
    {
        private HubConnection _connection;
        private IHubProxy _hub;
        ListView _friendsListView;
        ListView _incomingFriendRequestsListView;
        ListView _outgoingFriendRequestsListView;
        Button _findPeopleButton; // da je postavimo na enabled pri zatvaranju ove forme

        public FindPeopleForm(ref HubConnection connection, 
            ref IHubProxy hub,
            ref ListView friendsListView, 
            ref ListView incomingFriendRequestsListView, 
            ref ListView outgoingFriendRequestsListiew,
            ref Button findPeopleButton)
        {
            _connection = connection;
            _hub = hub;
            _friendsListView = friendsListView;
            _incomingFriendRequestsListView = incomingFriendRequestsListView;
            _outgoingFriendRequestsListView = outgoingFriendRequestsListiew;
            _findPeopleButton = findPeopleButton;
            InitializeComponent();
        }

        private void FindPeopleForm_Load(object sender, EventArgs e)
        {
            this.findPeopleListView.View = View.Details;
            this.findPeopleListView.HeaderStyle = ColumnHeaderStyle.None;
            this.findPeopleListView.FullRowSelect = true;
            this.findPeopleListView.Columns.Add("", -2);
        }

        private void FindPeopleForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Action action = delegate () {
                _findPeopleButton.Enabled = true;
            };
            _findPeopleButton.BeginInvoke(action);

        }

        public void UpdateFindPeopleListView(string peopleUsernames)
        {
            Action action = delegate () {
                findPeopleListView.Items.Clear();
                string[] peopleUsernamesTemp = peopleUsernames.Split(' ');
                foreach (string username in peopleUsernamesTemp)
                {
                    findPeopleListView.Items.Add(username);
                }
            };
            findPeopleListView.BeginInvoke(action);
        }

        // Ovo je search button
        private void findPeopleButton_Click(object sender, EventArgs e)
        {
            if (findPeopleTextBox.Text.Contains(" "))
            {
                MessageBox.Show("Username nesme da ima razmake");
                return;
            }

            if (findPeopleTextBox.Text.Length < 3)
            {
                MessageBox.Show("Username nesme da ima manje od 3 karaktera");
                return;
            }

            char[] SpecialChars = "!@#$%^&*()".ToCharArray();
            int indexOf = findPeopleTextBox.Text.IndexOfAny(SpecialChars);
            if (indexOf != -1)
            {
                MessageBox.Show("Username nesme da sadrzi specijalne karaktere");
                return;
            }
            _hub.Invoke("FindPeople", findPeopleTextBox.Text);
        }

        private void sendFriendRequestButton_Click(object sender, EventArgs e)
        {
            // Ovde cemo proveriti da li je osoba kojoj zelim da posaljemo zahtev za prijatelja
            // vec u nekoj od nasis listi, kako bi sprecili ponovno slanje zahteva za prijatelje
            // Ali idalje je moguc scenario u kome dva klijenta posalju zahtev za prijatelja jedan drugome,
            // odprilike u isto vreme. U tom slucaju ova client side logika ih nece spreciti da naprave dupli zahtev
            // Zato ce server da proveri za svaki slucaj postojanje vec nekog zahteva izmedju ova dva korisnika

            // Ako nam je ta osoba vec prijatelj nema razloga praviti ponovo zahtev
            try
            {
                for (int i = 0; i < _friendsListView.Items.Count; i++)
                {
                    if (_friendsListView.Items[i].Text == findPeopleListView.SelectedItems[0].Text)
                    {
                        MessageBox.Show("Taj korisnik vam je vec prijatelj");
                        return;
                    }
                }
                // Ako nam je osoba vec poslala friend request nema razloga slati nas
                for (int i = 0; i < _incomingFriendRequestsListView.Items.Count; i++)
                {
                    if (_incomingFriendRequestsListView.Items[i].Text == findPeopleListView.SelectedItems[0].Text)
                    {
                        MessageBox.Show("Taj korisnik nam je vec poslao zahtev za prijatelja");
                        return;
                    }
                }
                // Ako smo vec poslali friend request toj osobi nema razlofa ponovo slati
                for (int i = 0; i < _outgoingFriendRequestsListView.Items.Count; i++)
                {
                    if (_outgoingFriendRequestsListView.Items[i].Text == findPeopleListView.SelectedItems[0].Text)
                    {
                        MessageBox.Show("Toj osobi ste vec poslali zahtev za prijatelja");
                        return;
                    }
                }

                _hub.Invoke("SendFriendRequest", findPeopleListView.SelectedItems[0].Text);
            }
            catch
            {
                
            }
        }

       
    }
}
