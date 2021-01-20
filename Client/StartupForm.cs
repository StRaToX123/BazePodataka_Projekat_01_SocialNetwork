using System;
using System.Collections.Concurrent;
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
    public partial class StartupForm : Form
    {
        private HubConnection _connection;
        private IHubProxy _hub;
        private FindPeopleForm _findPeopleForm;
        private string _selectedFriend;


        private FriendsFromFriendForm _friendsFromFriendForm = new FriendsFromFriendForm();
        private Boolean _showCalledOnFriendsFromFriendForm = false;
        // pamtim za svakog prijatelja chat history
        public ConcurrentDictionary<string, List<string>> _perFriendChatHistory = new ConcurrentDictionary<string, List<string>>();
        // pamtim za svakog prijatelja chat textbox sta je bilo uneto poslednji put
        public ConcurrentDictionary<string, string> _perFriendChatTextBox = new ConcurrentDictionary<string, string>();
        public StartupForm()
        {
            InitializeComponent();

            string url = @"http://localhost:8080/";
            _connection = new HubConnection(url);
            _hub = _connection.CreateHubProxy("TestHub");
            try
            {
                _connection.Start().Wait();
            }
            catch
            {
                MessageBox.Show("Server je offline :( \n pokusajte ponovo kasnije");
                System.Environment.Exit(1);
                return;
            }

            // Registrovanje callback funkcija za signalR poruke
            _hub.On("ReceiveLength", x => ReceiveLength(x));
            _hub.On("LogInSuccessful", x => LogInSuccessful(x));
            _hub.On("LogInFailed", x => LogInFailed(x));
            _hub.On("AccountCreatedSuccessfuly", x => MessageBox.Show("Uspedno ste napravili novi nalog!"));
            _hub.On("AccountCreationFailed", x => MessageBox.Show("Doslo je do greske pri pravljenju novog naloga!"));
            _hub.On("FriendBecameOnline", x => FriendBecameOnline(x));
            _hub.On("FriendBecameOffline", x => FriendBecameOffline(x));
            _hub.On("FriendRequestArrived", x => FriendRequestArrived(x));
            _hub.On("FriendRequestSentOut", x => FriendRequestSentOut(x));
            _hub.On("FailedFriendRequest", x => FailedFriendRequest(x));
            _hub.On("OutgoingFriendRequestAccepted", x => OutgoingFriendRequestAccepted(x));
            _hub.On("IncomingFriendRequestAccepted", x => IncomingFriendRequestAccepted(x));
            _hub.On("FriendRemoved", x => FriendRemoved(x));
            _hub.On("FindPeopleResult", x => FindPeopleResult(x));
            _hub.On("GetChatHistoryResult", x => GetChatHistoryResult(x));
            _hub.On("ReceivedMessage", x => ReceivedMessage(x));
            _hub.On("SendMessageSuccessful", x => SendMessageSuccessful(x));
            _hub.On("LogOutSuccessful", x => LogOutSuccessful(x));
            _hub.On("FriendsFromFriendResult", x => FriendsFromFriendResult(x));
            _selectedFriend = "";
        }

        private void ReceiveLength(string x)
        {
            MessageBox.Show(x);
        }

       

        private void StartupForm_Load(object sender, EventArgs e)
        {
            // Omogucimo custom iscrtavanje libox Item-a sa vise reodva teksta
            this.chatViewListBox.DrawMode = DrawMode.OwnerDrawVariable;
            this.chatViewListBox.MeasureItem += ListBox1MeasureItem;
            this.chatViewListBox.DrawItem += ListBox1DrawItem;
            

            // Podesiti listview da izgleda nalik listbox
            this.friendsListView.View = View.Details;
            this.friendsListView.HeaderStyle = ColumnHeaderStyle.None;
            this.friendsListView.FullRowSelect = true;
            this.friendsListView.Columns.Add("", -2);

            this.outgoingFriendRequestsListView.View = View.Details;
            this.outgoingFriendRequestsListView.HeaderStyle = ColumnHeaderStyle.None;
            this.outgoingFriendRequestsListView.FullRowSelect = true;
            this.outgoingFriendRequestsListView.Columns.Add("", -2);

            this.incomingFriendRequestsListView.View = View.Details;
            this.incomingFriendRequestsListView.HeaderStyle = ColumnHeaderStyle.None;
            this.incomingFriendRequestsListView.FullRowSelect = true;
            this.incomingFriendRequestsListView.Columns.Add("", -2);
        }

        internal int CountOccurrences(string haystack, string needle)
        {
            int n = 0, pos = 0;
            while ((pos = haystack.IndexOf(needle, pos)) != -1)
            {
                n++;
                pos += needle.Length;
            }
            return n;
        }

        public void ListBox1MeasureItem(object sender, MeasureItemEventArgs e)
        {
            e.ItemHeight = (int)((CountOccurrences(((ListBox)sender).Items[e.Index].ToString(), "\n") + 1) * ((ListBox)sender).Font.GetHeight() + 2);
        }

        public void ListBox1DrawItem(object sender, DrawItemEventArgs e)
        {
            if (((ListBox)sender).Items.Count == 0)
            {
                return;
            }

            string text = ((ListBox)sender).Items[e.Index].ToString();
            e.DrawBackground();
            using (Brush b = new SolidBrush(e.ForeColor)) e.Graphics.DrawString(text, e.Font, b, new RectangleF(e.Bounds.Left, e.Bounds.Top, e.Bounds.Width, e.Bounds.Height));
            e.DrawFocusRectangle();
        }

        private void StartupForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _connection.Stop();
        }


        private void LogInSuccessful(string friendsDataString)
        {
            Action actionChangeFormName = delegate () {
                this.Text = usernameTextBox.Text;
            };
            this.BeginInvoke(actionChangeFormName);
            Action action = delegate () {
                label1.Visible = false;
            };
            label1.BeginInvoke(action);
            Action action2 = delegate () {
                label2.Visible = false;
            };
            label2.BeginInvoke(action2);
            Action action3 = delegate () {
                label3.Visible = false;
            };
            label3.BeginInvoke(action3);
            Action action4 = delegate () {
                label4.Visible = false;
            };
            label4.BeginInvoke(action4);
            Action action5 = delegate () {
                label5.Visible = false;
            };
            label5.BeginInvoke(action5);
            Action action6 = delegate () {
                label6.Visible = false;
            };
            label6.BeginInvoke(action6);
            Action action7 = delegate () {
                label7.Visible = false;
            };
            label7.BeginInvoke(action7);
            Action action8 = delegate () {
                label8.Visible = false;
            };
            label8.BeginInvoke(action8);


            Action action9 = delegate () {
                usernameTextBox.Visible = false;
            };
            usernameTextBox.BeginInvoke(action9);
            Action action10 = delegate () {
                passwordTextBox.Visible = false;
            };
            passwordTextBox.BeginInvoke(action10);
            Action action11 = delegate () {
                loginButton.Visible = false;
            };
            loginButton.BeginInvoke(action11);

            Action action12 = delegate () {
                usernameCreateAccountTextBox.Visible = false;
            };
            Action action13 = delegate () {
                passwordCreateAccountTextBox.Visible = false;
            };
            Action action14 = delegate () {
                confirmPasswordCreateAccountTextBox.Visible = false;
            };
            Action action15 = delegate () {
                emailCreateAccountTextBox.Visible = false;
            };
            Action action16 = delegate () {
                createAccountButton.Visible = false;
            };

            usernameCreateAccountTextBox.BeginInvoke(action12);
            passwordCreateAccountTextBox.BeginInvoke(action13);
            confirmPasswordCreateAccountTextBox.BeginInvoke(action14);
            emailCreateAccountTextBox.BeginInvoke(action15);
            createAccountButton.BeginInvoke(action16);

            Action action17 = delegate () {
                friendsListView.Visible = true;
            };
            friendsListView.BeginInvoke(action17);

            Action action18 = delegate () {
                friendsLabel.Visible = true;
            };
            friendsLabel.BeginInvoke(action18);
            Action action19 = delegate () {
                findPeopleButton.Visible = true;
            };
            findPeopleButton.BeginInvoke(action19);
            Action action21 = delegate () {
                removeFriendButton.Visible = true;
            };
            removeFriendButton.BeginInvoke(action21);

            Action action22 = delegate () {
                label9.Visible = true;
            };
            label9.BeginInvoke(action22);
            Action action23 = delegate () {
                label10.Visible = true;
            };
            label10.BeginInvoke(action23);
            Action action24 = delegate () {
                incomingFriendRequestsListView.Visible = true;
            };
            incomingFriendRequestsListView.BeginInvoke(action24);
            Action action25 = delegate () {
                outgoingFriendRequestsListView.Visible = true;
            };
            outgoingFriendRequestsListView.BeginInvoke(action25);

            





            string[] dataSplit = friendsDataString.Split('#');
            // Popuniti listu incoming friend requests
            string[] incomingFriendRquestsTemp = dataSplit[0].Split('@');
            Action action26 = delegate () {
                foreach (string incomingFriendRequest in incomingFriendRquestsTemp)
                {
                    if (incomingFriendRequest != "")
                    {
                        incomingFriendRequestsListView.Items.Add(incomingFriendRequest);
                    }                  
                }
            };
            incomingFriendRequestsListView.BeginInvoke(action26);
            // Popuniti listu outgoing friendRequests
            string[] outgoingFriendRquestsTemp = dataSplit[1].Split('@');
            Action action27 = delegate () {
                foreach (string outgoingFriendRequest in outgoingFriendRquestsTemp)
                {
                    if (outgoingFriendRequest != "")
                    {
                        outgoingFriendRequestsListView.Items.Add(outgoingFriendRequest);
                    }
                }
            };
            outgoingFriendRequestsListView.BeginInvoke(action27);
            // Popuniti listu prijatelja
            string[] friendsTemp = dataSplit[2].Split('@');
            Action action28 = delegate () {
                foreach (string friend in friendsTemp)
                {
                    string[] friendData = friend.Split(' ');
                    // prvi element je ime, drugi element je boolean koji oznacava da li je taj 
                    // korisnik trenutno online
                    if (friendData[0] != "")
                    {
                        friendsListView.Items.Add(friendData[0]);
                        Boolean isOnline;
                        Boolean.TryParse(friendData[1], out isOnline);
                        if (isOnline)
                        {
                            friendsListView.Items[friendsListView.Items.Count - 1].ForeColor = Color.Green;
                        }
                        else
                        {
                            friendsListView.Items[friendsListView.Items.Count - 1].ForeColor = Color.Black;
                        }
                    }
                }
            };
            friendsListView.BeginInvoke(action28);

            Action action29 = delegate () {
                acceptFriendRequestButton.Visible = true;
            };
            acceptFriendRequestButton.BeginInvoke(action29);

            Action action30 = delegate () {
                chatViewListBox.Visible = true;
            };
            chatViewListBox.BeginInvoke(action30);

            Action action31 = delegate () {
                chatTextBox.Visible = true;
            };
            chatTextBox.BeginInvoke(action31);

            Action action32 = delegate () {
                chatViewSendButton.Visible = true;
            };
            chatViewSendButton.BeginInvoke(action32);

            Action action33 = delegate () {
                chatViewLoadOlderButton.Visible = true;
            };
            chatViewLoadOlderButton.BeginInvoke(action33);

            Action action34 = delegate () {
                logoutButton.Visible = true;
            };
            logoutButton.BeginInvoke(action34);
        }

        private void LogOutSuccessful(string nista)
        {
            // Ukljuciti sve kontrole od login screen
            Action actionChangeFormName = delegate () {
                this.Text = "Neo4j IM App";
            };
            this.BeginInvoke(actionChangeFormName);
            Action action = delegate () {
                label1.Visible = true;
            };
            label1.BeginInvoke(action);
            Action action2 = delegate () {
                label2.Visible = true;
            };
            label2.BeginInvoke(action2);
            Action action3 = delegate () {
                label3.Visible = true;
            };
            label3.BeginInvoke(action3);
            Action action4 = delegate () {
                label4.Visible = true;
            };
            label4.BeginInvoke(action4);
            Action action5 = delegate () {
                label5.Visible = true;
            };
            label5.BeginInvoke(action5);
            Action action6 = delegate () {
                label6.Visible = true;
            };
            label6.BeginInvoke(action6);
            Action action7 = delegate () {
                label7.Visible = true;
            };
            label7.BeginInvoke(action7);
            Action action8 = delegate () {
                label8.Visible = true;
            };
            label8.BeginInvoke(action8);


            Action action9 = delegate () {
                usernameTextBox.Visible = true;
            };
            usernameTextBox.BeginInvoke(action9);
            Action action10 = delegate () {
                passwordTextBox.Visible = true;
            };
            passwordTextBox.BeginInvoke(action10);
            Action action11 = delegate () {
                loginButton.Visible = true;
            };
            loginButton.BeginInvoke(action11);

            Action action12 = delegate () {
                usernameCreateAccountTextBox.Visible = true;
            };
            Action action13 = delegate () {
                passwordCreateAccountTextBox.Visible = true;
            };
            Action action14 = delegate () {
                confirmPasswordCreateAccountTextBox.Visible = true;
            };
            Action action15 = delegate () {
                emailCreateAccountTextBox.Visible = true;
            };
            Action action16 = delegate () {
                createAccountButton.Visible = true;
            };

            usernameCreateAccountTextBox.BeginInvoke(action12);
            passwordCreateAccountTextBox.BeginInvoke(action13);
            confirmPasswordCreateAccountTextBox.BeginInvoke(action14);
            emailCreateAccountTextBox.BeginInvoke(action15);
            createAccountButton.BeginInvoke(action16);

            // Iskljuciti sve kontrole od chat view screen
            Action action17 = delegate () {
                friendsListView.Visible = false;
            };
            friendsListView.BeginInvoke(action17);

            Action action18 = delegate () {
                friendsLabel.Visible = false;
            };
            friendsLabel.BeginInvoke(action18);
            Action action19 = delegate () {
                findPeopleButton.Visible = false;
            };
            findPeopleButton.BeginInvoke(action19);
            Action action21 = delegate () {
                removeFriendButton.Visible = false;
            };
            removeFriendButton.BeginInvoke(action21);

            Action action22 = delegate () {
                label9.Visible = false;
            };
            label9.BeginInvoke(action22);
            Action action23 = delegate () {
                label10.Visible = false;
            };
            label10.BeginInvoke(action23);
            Action action24 = delegate () {
                incomingFriendRequestsListView.Visible = false;
            };
            incomingFriendRequestsListView.BeginInvoke(action24);
            Action action25 = delegate () {
                outgoingFriendRequestsListView.Visible = false;
            };
            outgoingFriendRequestsListView.BeginInvoke(action25);
            Action action29 = delegate () {
                acceptFriendRequestButton.Visible = false;
            };
            acceptFriendRequestButton.BeginInvoke(action29);

            Action action30 = delegate () {
                chatViewListBox.Visible = false;
            };
            chatViewListBox.BeginInvoke(action30);

            Action action31 = delegate () {
                chatTextBox.Visible = false;
            };
            chatTextBox.BeginInvoke(action31);

            Action action32 = delegate () {
                chatViewSendButton.Visible = false;
            };
            chatViewSendButton.BeginInvoke(action32);

            Action action33 = delegate () {
                chatViewLoadOlderButton.Visible = false;
            };
            chatViewLoadOlderButton.BeginInvoke(action33);

            Action action34 = delegate () {
                logoutButton.Visible = false;
            };
            logoutButton.BeginInvoke(action34);

            // Obrisati sve chatview liste
            Action clearIncomingFriendRequestsAction = delegate () {
                incomingFriendRequestsListView.Items.Clear();
            };
            incomingFriendRequestsListView.BeginInvoke(clearIncomingFriendRequestsAction);
            Action clearOutgoingFriendRequestsAction = delegate () {
                outgoingFriendRequestsListView.Items.Clear();
            };
            outgoingFriendRequestsListView.BeginInvoke(clearOutgoingFriendRequestsAction);
            Action clearfriendsListViewAction = delegate () {
                friendsListView.Items.Clear();
            };
            friendsListView.BeginInvoke(clearfriendsListViewAction);
            Action clearChatViewAction = delegate () {
                chatViewListBox.Items.Clear();
            };
            chatViewListBox.BeginInvoke(clearChatViewAction);
            Action clearChatTextBox = delegate () {
                chatTextBox.Text = "";
            };
            chatTextBox.BeginInvoke(clearChatTextBox);

            if (_findPeopleForm != null)
            {
                _findPeopleForm.Close();
            }

            if (_showCalledOnFriendsFromFriendForm == true)
            {
                if (_friendsFromFriendForm.IsDisposed == false)
                {
                    Action closeForm = delegate () {
                        _friendsFromFriendForm.Close();
                    };
                    _friendsFromFriendForm.BeginInvoke(closeForm);
                }
            }

            _selectedFriend = "";
            
        }

        private void LogInFailed(string nista)
        {
            MessageBox.Show("FAILED LOGIN");
        }

        private void FriendBecameOnline(string friend)
        {
            Action action = delegate ()
            {
                for (int i = 0; i < friendsListView.Items.Count; i++)
                {
                    if (friendsListView.Items[i].Text == friend)
                    {
                        friendsListView.Items[i].ForeColor = Color.Green;
                        break;
                    }
                }
            };
            friendsListView.BeginInvoke(action);
        }
        private void FriendBecameOffline(string friend)
        {
            Action action = delegate ()
            {
                for (int i = 0; i < friendsListView.Items.Count; i++)
                {
                    if (friendsListView.Items[i].Text == friend)
                    {
                        friendsListView.Items[i].ForeColor = Color.Black;
                    }
                }
            };
            friendsListView.BeginInvoke(action);
        }

        private void loginButton_Click(object sender, EventArgs e)
        {
            _hub.Invoke("LogIn", usernameTextBox.Text, passwordTextBox.Text);
        }

        private void logoutButton_Click(object sender, EventArgs e)
        {
            _hub.Invoke("LogOut", "nista").Wait();
        }

        private void createAccountButton_Click(object sender, EventArgs e)
        {
            // Prvo proveriti da li su sva polja popunjena
            bool emailCheckResult = false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(emailCreateAccountTextBox.Text);
                emailCheckResult = addr.Address == emailCreateAccountTextBox.Text;
            }
            catch
            {
                emailCheckResult = false;
            }

            if (emailCheckResult == false)
            {
                MessageBox.Show("Niste lepo uneli Email adresu");
                return;
            }

            // Provera validno popunjen username
            if (usernameCreateAccountTextBox.Text.Contains(" "))
            {
                MessageBox.Show("Username nesme da ima razmake");
                return;
            }

            if (usernameCreateAccountTextBox.Text.Length < 3)
            {
                MessageBox.Show("Username nesme da ima manje od 3 karaktera");
                return;
            }

            char[] SpecialChars = "!@#$%^&*()".ToCharArray();
            int indexOf = usernameCreateAccountTextBox.Text.IndexOfAny(SpecialChars);
            if (indexOf != -1)
            {
                MessageBox.Show("Username nesme da sadrzi specijalne karaktere");
                return;
            }

            // Provera validno popunjen password
            if (passwordCreateAccountTextBox.Text.Contains(" "))
            {
                MessageBox.Show("Password nesme da ima razmake");
                return;
            }

            if (passwordCreateAccountTextBox.Text.Length < 3)
            {
                MessageBox.Show("Password nesme da ima manje od 3 karaktera");
                return;
            }

            if (passwordCreateAccountTextBox.Text != confirmPasswordCreateAccountTextBox.Text)
            {
                MessageBox.Show("Passwords se ne poklapaju");
                return;
            }

            _hub.Invoke("CreateAccount", usernameCreateAccountTextBox.Text,
                passwordCreateAccountTextBox.Text, 
                emailCreateAccountTextBox.Text).Wait();
        }


        private void FriendRequestArrived(string username)
        {
            Action action = delegate () {
                incomingFriendRequestsListView.Items.Add(username);
            };
            incomingFriendRequestsListView.BeginInvoke(action);
        }

        private void FriendRequestSentOut(string username)
        {
            Action action = delegate () {
                outgoingFriendRequestsListView.Items.Add(username);
            };
            outgoingFriendRequestsListView.BeginInvoke(action);
        }

        private void FailedFriendRequest(string nesto)
        {
            MessageBox.Show("Taj korisnik ne postoji, proverite username koji ste uneli");
        }

        // Treba da posalje serveru zahtev da nam pribavi listu prijatelja ovog naseg prijatelja
        private void friendsListView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (_friendsFromFriendForm.IsDisposed)
                {
                    _friendsFromFriendForm = new FriendsFromFriendForm();
                }
                _showCalledOnFriendsFromFriendForm = true;
                _friendsFromFriendForm.Show();
                _hub.Invoke("GetFriendsFromFriend", friendsListView.SelectedItems[0].Text);
            }
            catch
            {
                
            }
        }

        private void FriendsFromFriendResult(string friendsOfFriendList)
        {
            if (_friendsFromFriendForm != null)
            {
                _friendsFromFriendForm.UpdateForm(friendsOfFriendList);
            }
        }

        private void findPeopleButton_Click(object sender, EventArgs e)
        {
            // Otvori find people form
            Action action = delegate ()
            {
                findPeopleButton.Enabled = false;
            };
            findPeopleButton.BeginInvoke(action);

            _findPeopleForm = new FindPeopleForm(ref _connection, 
                ref _hub, 
                ref friendsListView, 
                ref incomingFriendRequestsListView, 
                ref outgoingFriendRequestsListView,
                ref findPeopleButton);
            _findPeopleForm.Show();
        }

        // Kada korisnik klikne na search u findPeople formi callback od servera se nalazi u ovoj klasi
        // znaci da mora da prosledimo poruku findPeople formi
        private void FindPeopleResult(string peopleUsernames)
        {
            try
            {
                if (_findPeopleForm != null)
                {
                    _findPeopleForm.UpdateFindPeopleListView(peopleUsernames);
                }
            }
            catch
            {
                
            }
        }

        private void acceptFriendRequestButton_Click(object sender, EventArgs e)
        {
            try
            {
                _hub.Invoke("AcceptFriendRequest", incomingFriendRequestsListView.SelectedItems[0].Text);
            }
            catch
            {
                
            }
        }

        // Ova funkcija se poziva kada neko iz nase friendRequest liste treba
        // da predje u friends listu
        private void OutgoingFriendRequestAccepted(string username)
        {
            Action action1 = delegate () {
                for (int i = 0; i < outgoingFriendRequestsListView.Items.Count; i++)
                {
                    if (outgoingFriendRequestsListView.Items[i].Text == username)
                    {
                        outgoingFriendRequestsListView.Items.RemoveAt(i);
                        break;
                    }
                }
            };
            outgoingFriendRequestsListView.BeginInvoke(action1);

            Action action2 = delegate () {
                friendsListView.Items.Add(username);
                // Posto nam je upravo stigla potvrda da nam je ovo novi prijatelj
                // mozemo podrazumevati da je taj korisnik online trenutno
                friendsListView.Items[friendsListView.Items.Count - 1].ForeColor = Color.Green;
            };
            friendsListView.BeginInvoke(action2);

            return;
        }

        private void IncomingFriendRequestAccepted(string usernameAndOnlineStatus)
        {
            // na nultom indeksu ce biti username a na prvom njegov online status
            string[] usernameAndOnlineStatusTemp = usernameAndOnlineStatus.Split(' ');
            Action action1 = delegate ()
            {
                for (int i = 0; i < incomingFriendRequestsListView.Items.Count; i++)
                {
                    if (incomingFriendRequestsListView.Items[i].Text == usernameAndOnlineStatusTemp[0])
                    {
                        incomingFriendRequestsListView.Items.RemoveAt(i);
                        break;
                    }
                }
            };
            incomingFriendRequestsListView.BeginInvoke(action1);

            Action action2 = delegate ()
            {
                friendsListView.Items.Add(usernameAndOnlineStatusTemp[0]);
                Boolean isOnline;
                Boolean.TryParse(usernameAndOnlineStatusTemp[1], out isOnline);
                if (isOnline)
                {
                    friendsListView.Items[friendsListView.Items.Count - 1].ForeColor = Color.Green;
                }
                else
                {
                    friendsListView.Items[friendsListView.Items.Count - 1].ForeColor = Color.Black;
                }
            };
            friendsListView.BeginInvoke(action2);   
          
            return;
        }

        private void removeFriendButton_Click(object sender, EventArgs e)
        {
            try
            {
                _hub.Invoke("RemoveFriend", friendsListView.SelectedItems[0].Text);
            }
            catch
            {
                
            }
        }

        // Poziva se kada neko izbrise nas sa njegove liste prijatelja
        // ili
        // kao potvrda da smo uspesno izbrisali korisnika sa nase liste prijatelja
        private void FriendRemoved(string friendUsername)
        {
            Action action = delegate ()
            {
                for (int i = 0; i < friendsListView.Items.Count; i++)
                {
                    if (friendsListView.Items[i].Text == friendUsername)
                    {
                        friendsListView.Items.RemoveAt(i);
                        break;
                    }
                }
            };
            friendsListView.BeginInvoke(action);

            List<string> garbage;
            string garbage2;
            _perFriendChatHistory.TryRemove(friendUsername, out garbage);
            _perFriendChatTextBox.TryRemove(friendUsername, out garbage2);
            if (_selectedFriend == friendUsername)
            {
                _selectedFriend = "";
                Action action2 = delegate () {
                    chatViewListBox.Items.Clear();
                };
                chatViewListBox.BeginInvoke(action2);
            }
        }

        // Na samo jedan levi klik friendsListe treba otvoriti chat selektovanog prijatelja
        private void friendsListView_Click(object sender, EventArgs e)
        {
            try
            {
                if (friendsListView.SelectedItems[0].Text != _selectedFriend)
                {
                    _perFriendChatTextBox[_selectedFriend] = chatTextBox.Text;
                    _selectedFriend = friendsListView.SelectedItems[0].Text;

                    Action action3 = delegate () {
                        chatViewListBox.Items.Clear();
                    };
                    chatViewListBox.BeginInvoke(action3);

                    // Da li vec imamo neki u memoriji chat history sa ovom osobom
                    List<string> result;
                    _perFriendChatHistory.TryGetValue(friendsListView.SelectedItems[0].Text, out result);
                    if (result != null)
                    {
                        // Znaci da mozemo da ucitamo iz memorije
                        Action action = delegate () {
                            for (int i = 0; i < result.Count; i++)
                            {
                                chatViewListBox.Items.Add(result[i]);
                            }
                        };
                        chatViewListBox.BeginInvoke(action);

                        Action action2 = delegate () {
                            for (int i = 0; i < friendsListView.Items.Count; i++)
                            {
                                if (friendsListView.Items[i].Text == _selectedFriend)
                                {
                                    friendsListView.Items[i].BackColor = Color.White;
                                    break;
                                }
                            }
                        };
                        friendsListView.BeginInvoke(action2);
                    }
                    else
                    {
                        _perFriendChatHistory.TryAdd(friendsListView.SelectedItems[0].Text, new List<string>());
                        _perFriendChatTextBox.TryAdd(friendsListView.SelectedItems[0].Text, "");
                        // saljemo -1 jer zelimo da nam server vrati najnovije sto ima 
                        _hub.Invoke("GetChatHistory", _selectedFriend, "-1");
                    }
                }    
            }
            catch
            {
                
            }
        }

        private void GetChatHistoryResult(string chatData)
        {
            // chatDataTemp[0] je ime prijatelja
            // sve ostalo je u descentid order poruke
            List<string> chatDataTemp = chatData.Split('#').ToList();
            // znaci da kada ubacujemo moramo da okrenemo chatDataTemp naopacke
            int counter = 0;
            for (int i = chatDataTemp.Count - 1; i >= 1; i--)
            {
                if (chatDataTemp[i] != "")
                {
                    _perFriendChatHistory[chatDataTemp[0]].Insert(counter, chatDataTemp[i]);
                    counter++;
                }
            }

            // Ako je ovo trenutno izabrani prijatelj treba popuniti chatViewListBox sa ovim novim informacijama
            if (_selectedFriend == chatDataTemp[0])
            {
                Action action = delegate () {
                    counter = 0;
                    for (int i = chatDataTemp.Count - 1; i >= 1; i--)
                    {
                        if (chatDataTemp[i] != "")
                        {
                            chatViewListBox.Items.Insert(counter, chatDataTemp[i]);
                            counter++;
                        }
                    }
                };
                chatViewListBox.BeginInvoke(action);

                Action action2 = delegate () {
                    for (int i = 0; i < friendsListView.Items.Count; i++)
                    {
                        if (friendsListView.Items[i].Text == chatDataTemp[0])
                        {
                            friendsListView.Items[i].BackColor = Color.White;
                            break;
                        }
                    }
                };
                friendsListView.BeginInvoke(action2);
            }
        }

        private void chatViewSendButton_Click(object sender, EventArgs e)
        {
            try
            {
                if ((_selectedFriend != "") && (!string.IsNullOrWhiteSpace(chatTextBox.Text)))
                {
                    _hub.Invoke("SendMessage", _selectedFriend, chatTextBox.Text);
                    Action action = delegate () {
                        chatTextBox.Text = "";
                    };
                    chatTextBox.BeginInvoke(action);
                    _perFriendChatTextBox[_selectedFriend] = "";
                }
            }
            catch
            {
                
            }
        }

        private void ReceivedMessage(string message)
        {
            string[] messageTemp = message.Split('#'); // na nultoj poziciji je ime posiljaoca
            List<string> chatCache;
            _perFriendChatHistory.TryGetValue(messageTemp[0], out chatCache);

            // Ako postoji u kesu chat history za ovog prijatelja onda ga mozemo updejtovati
            // u suprotnom nema razloga updejtovati jer ce klijent zatraziti od servera vec deo chat istorije
            // kada bude bio kliknuo na ovog prijatelja
            if (chatCache != null)
            {
                _perFriendChatHistory[messageTemp[0]].Add(messageTemp[1]);
                // Ako je trenutno aktivan chat view sa ovim prijateljem onda treba odma prikazati novu poruku
                if (_selectedFriend == messageTemp[0])
                {
                    Action updateChatViewListBox = delegate ()
                    {
                        chatViewListBox.Items.Add(messageTemp[1]);
                    };
                    chatViewListBox.BeginInvoke(updateChatViewListBox);
                }
                else
                {
                    // Promeniti boju prijatelja
                    Action action = delegate ()
                    {
                        for (int i = 0; i < friendsListView.Items.Count; i++)
                        {
                            if (friendsListView.Items[i].Text == messageTemp[0])
                            {
                                friendsListView.Items[i].BackColor = Color.LightBlue;
                                break;
                            }
                        }
                    };
                    friendsListView.BeginInvoke(action);
                }
            }
            else
            {
                // Promeniti boju prijatelja
                Action action = delegate ()
                {
                    for (int i = 0; i < friendsListView.Items.Count; i++)
                    {
                        if (friendsListView.Items[i].Text == messageTemp[0])
                        {
                            friendsListView.Items[i].BackColor = Color.LightBlue;
                            break;
                        }
                    }
                };
                friendsListView.BeginInvoke(action);
            }
        }

        private void SendMessageSuccessful(string message)
        {
            string[] messageTemp = message.Split('#'); // na nultoj poziciji je ime posiljaoca
            List<string> chatCache;
            _perFriendChatHistory.TryGetValue(messageTemp[0], out chatCache);

            // Ako postoji u kesu chat history za ovog prijatelja onda ga mozemo updejtovati
            // u suprotnom nema razloga updejtovati jer ce klijent zatraziti od servera vec deo chat istorije
            // kada bude bio kliknuo na ovog prijatelja
            if (chatCache != null)
            {
                _perFriendChatHistory[messageTemp[0]].Add(messageTemp[1]);
                // Ako je trenutno aktivan chat view sa ovim prijateljem onda treba odma prikazati novu poruku
                if (_selectedFriend == messageTemp[0])
                {
                    Action updateChatViewListBox = delegate ()
                    {
                        chatViewListBox.Items.Add(messageTemp[1]);
                    };
                    chatViewListBox.BeginInvoke(updateChatViewListBox);


                }
            }
        }

        private void chatViewLoadOlderButton_Click(object sender, EventArgs e)
        {
            _hub.Invoke("GetChatHistory", _selectedFriend, _perFriendChatHistory[_selectedFriend].Count);
        }

        private void chatTextBox_TextChanged(object sender, EventArgs e)
        {
            char[] SpecialChars = "@#$%^&".ToCharArray();
            int indexOf = chatTextBox.Text.IndexOfAny(SpecialChars);
            if (indexOf != -1)
            {
                chatTextBox.Text = chatTextBox.Text.Remove(chatTextBox.Text.Length - 1, 1);
                chatTextBox.SelectionStart = chatTextBox.Text.Length;
                chatTextBox.SelectionLength = 0;
            }
        }
    }

   
}
