namespace Client
{
    partial class StartupForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.loginButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.usernameCreateAccountTextBox = new System.Windows.Forms.TextBox();
            this.passwordCreateAccountTextBox = new System.Windows.Forms.TextBox();
            this.confirmPasswordCreateAccountTextBox = new System.Windows.Forms.TextBox();
            this.emailCreateAccountTextBox = new System.Windows.Forms.TextBox();
            this.createAccountButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.friendsLabel = new System.Windows.Forms.Label();
            this.findPeopleButton = new System.Windows.Forms.Button();
            this.removeFriendButton = new System.Windows.Forms.Button();
            this.friendsListView = new System.Windows.Forms.ListView();
            this.incomingFriendRequestsListView = new System.Windows.Forms.ListView();
            this.label9 = new System.Windows.Forms.Label();
            this.outgoingFriendRequestsListView = new System.Windows.Forms.ListView();
            this.label10 = new System.Windows.Forms.Label();
            this.acceptFriendRequestButton = new System.Windows.Forms.Button();
            this.logoutButton = new System.Windows.Forms.Button();
            this.chatTextBox = new System.Windows.Forms.TextBox();
            this.chatViewListBox = new System.Windows.Forms.ListBox();
            this.chatViewLoadOlderButton = new System.Windows.Forms.Button();
            this.chatViewSendButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(145, 152);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(100, 20);
            this.usernameTextBox.TabIndex = 0;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(145, 191);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.PasswordChar = '*';
            this.passwordTextBox.Size = new System.Drawing.Size(100, 20);
            this.passwordTextBox.TabIndex = 1;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(157, 217);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "LogIn";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(145, 133);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 175);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Password";
            // 
            // usernameCreateAccountTextBox
            // 
            this.usernameCreateAccountTextBox.Location = new System.Drawing.Point(502, 132);
            this.usernameCreateAccountTextBox.Name = "usernameCreateAccountTextBox";
            this.usernameCreateAccountTextBox.Size = new System.Drawing.Size(100, 20);
            this.usernameCreateAccountTextBox.TabIndex = 5;
            // 
            // passwordCreateAccountTextBox
            // 
            this.passwordCreateAccountTextBox.Location = new System.Drawing.Point(502, 175);
            this.passwordCreateAccountTextBox.Name = "passwordCreateAccountTextBox";
            this.passwordCreateAccountTextBox.PasswordChar = '*';
            this.passwordCreateAccountTextBox.Size = new System.Drawing.Size(100, 20);
            this.passwordCreateAccountTextBox.TabIndex = 6;
            // 
            // confirmPasswordCreateAccountTextBox
            // 
            this.confirmPasswordCreateAccountTextBox.Location = new System.Drawing.Point(502, 218);
            this.confirmPasswordCreateAccountTextBox.Name = "confirmPasswordCreateAccountTextBox";
            this.confirmPasswordCreateAccountTextBox.PasswordChar = '*';
            this.confirmPasswordCreateAccountTextBox.Size = new System.Drawing.Size(100, 20);
            this.confirmPasswordCreateAccountTextBox.TabIndex = 7;
            // 
            // emailCreateAccountTextBox
            // 
            this.emailCreateAccountTextBox.Location = new System.Drawing.Point(502, 257);
            this.emailCreateAccountTextBox.Name = "emailCreateAccountTextBox";
            this.emailCreateAccountTextBox.Size = new System.Drawing.Size(100, 20);
            this.emailCreateAccountTextBox.TabIndex = 8;
            // 
            // createAccountButton
            // 
            this.createAccountButton.Location = new System.Drawing.Point(502, 283);
            this.createAccountButton.Name = "createAccountButton";
            this.createAccountButton.Size = new System.Drawing.Size(100, 23);
            this.createAccountButton.TabIndex = 9;
            this.createAccountButton.Text = "Create Account";
            this.createAccountButton.UseVisualStyleBackColor = true;
            this.createAccountButton.Click += new System.EventHandler(this.createAccountButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(502, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Username";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(502, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Password";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(502, 202);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Confirm Password";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(502, 241);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Email";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(124, 56);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(142, 55);
            this.label7.TabIndex = 14;
            this.label7.Text = "Login";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(363, 56);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(356, 55);
            this.label8.TabIndex = 15;
            this.label8.Text = "Create Account";
            // 
            // friendsLabel
            // 
            this.friendsLabel.AutoSize = true;
            this.friendsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.friendsLabel.Location = new System.Drawing.Point(12, 325);
            this.friendsLabel.Name = "friendsLabel";
            this.friendsLabel.Size = new System.Drawing.Size(62, 20);
            this.friendsLabel.TabIndex = 20;
            this.friendsLabel.Text = "Friends";
            this.friendsLabel.Visible = false;
            // 
            // findPeopleButton
            // 
            this.findPeopleButton.Location = new System.Drawing.Point(12, 471);
            this.findPeopleButton.Name = "findPeopleButton";
            this.findPeopleButton.Size = new System.Drawing.Size(225, 23);
            this.findPeopleButton.TabIndex = 21;
            this.findPeopleButton.Text = "Find People";
            this.findPeopleButton.UseVisualStyleBackColor = true;
            this.findPeopleButton.Visible = false;
            this.findPeopleButton.Click += new System.EventHandler(this.findPeopleButton_Click);
            // 
            // removeFriendButton
            // 
            this.removeFriendButton.Location = new System.Drawing.Point(12, 500);
            this.removeFriendButton.Name = "removeFriendButton";
            this.removeFriendButton.Size = new System.Drawing.Size(104, 23);
            this.removeFriendButton.TabIndex = 23;
            this.removeFriendButton.Text = "Remove Friend";
            this.removeFriendButton.UseVisualStyleBackColor = true;
            this.removeFriendButton.Visible = false;
            this.removeFriendButton.Click += new System.EventHandler(this.removeFriendButton_Click);
            // 
            // friendsListView
            // 
            this.friendsListView.HideSelection = false;
            this.friendsListView.Location = new System.Drawing.Point(12, 348);
            this.friendsListView.Name = "friendsListView";
            this.friendsListView.Size = new System.Drawing.Size(225, 119);
            this.friendsListView.TabIndex = 24;
            this.friendsListView.UseCompatibleStateImageBehavior = false;
            this.friendsListView.Visible = false;
            this.friendsListView.Click += new System.EventHandler(this.friendsListView_Click);
            this.friendsListView.DoubleClick += new System.EventHandler(this.friendsListView_DoubleClick);
            // 
            // incomingFriendRequestsListView
            // 
            this.incomingFriendRequestsListView.HideSelection = false;
            this.incomingFriendRequestsListView.Location = new System.Drawing.Point(12, 175);
            this.incomingFriendRequestsListView.Name = "incomingFriendRequestsListView";
            this.incomingFriendRequestsListView.Size = new System.Drawing.Size(225, 120);
            this.incomingFriendRequestsListView.TabIndex = 25;
            this.incomingFriendRequestsListView.UseCompatibleStateImageBehavior = false;
            this.incomingFriendRequestsListView.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(11, 155);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(187, 20);
            this.label9.TabIndex = 26;
            this.label9.Text = "Incoming Friend Rquests";
            this.label9.Visible = false;
            // 
            // outgoingFriendRequestsListView
            // 
            this.outgoingFriendRequestsListView.HideSelection = false;
            this.outgoingFriendRequestsListView.Location = new System.Drawing.Point(12, 32);
            this.outgoingFriendRequestsListView.Name = "outgoingFriendRequestsListView";
            this.outgoingFriendRequestsListView.Size = new System.Drawing.Size(225, 120);
            this.outgoingFriendRequestsListView.TabIndex = 27;
            this.outgoingFriendRequestsListView.UseCompatibleStateImageBehavior = false;
            this.outgoingFriendRequestsListView.Visible = false;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(11, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(187, 20);
            this.label10.TabIndex = 28;
            this.label10.Text = "Outgoing Friend Rquests";
            this.label10.Visible = false;
            // 
            // acceptFriendRequestButton
            // 
            this.acceptFriendRequestButton.Location = new System.Drawing.Point(12, 299);
            this.acceptFriendRequestButton.Name = "acceptFriendRequestButton";
            this.acceptFriendRequestButton.Size = new System.Drawing.Size(225, 23);
            this.acceptFriendRequestButton.TabIndex = 29;
            this.acceptFriendRequestButton.Text = "Accept Request";
            this.acceptFriendRequestButton.UseVisualStyleBackColor = true;
            this.acceptFriendRequestButton.Visible = false;
            this.acceptFriendRequestButton.Click += new System.EventHandler(this.acceptFriendRequestButton_Click);
            // 
            // logoutButton
            // 
            this.logoutButton.Location = new System.Drawing.Point(122, 501);
            this.logoutButton.Name = "logoutButton";
            this.logoutButton.Size = new System.Drawing.Size(115, 23);
            this.logoutButton.TabIndex = 30;
            this.logoutButton.Text = "Logout";
            this.logoutButton.UseVisualStyleBackColor = true;
            this.logoutButton.Visible = false;
            this.logoutButton.Click += new System.EventHandler(this.logoutButton_Click);
            // 
            // chatTextBox
            // 
            this.chatTextBox.Location = new System.Drawing.Point(243, 471);
            this.chatTextBox.Multiline = true;
            this.chatTextBox.Name = "chatTextBox";
            this.chatTextBox.Size = new System.Drawing.Size(476, 53);
            this.chatTextBox.TabIndex = 31;
            this.chatTextBox.Visible = false;
            // 
            // chatViewListBox
            // 
            this.chatViewListBox.FormattingEnabled = true;
            this.chatViewListBox.Location = new System.Drawing.Point(243, 45);
            this.chatViewListBox.Name = "chatViewListBox";
            this.chatViewListBox.Size = new System.Drawing.Size(569, 420);
            this.chatViewListBox.TabIndex = 33;
            this.chatViewListBox.Visible = false;
            // 
            // chatViewLoadOlderButton
            // 
            this.chatViewLoadOlderButton.Location = new System.Drawing.Point(480, 18);
            this.chatViewLoadOlderButton.Name = "chatViewLoadOlderButton";
            this.chatViewLoadOlderButton.Size = new System.Drawing.Size(75, 23);
            this.chatViewLoadOlderButton.TabIndex = 34;
            this.chatViewLoadOlderButton.Text = "Load Older";
            this.chatViewLoadOlderButton.UseVisualStyleBackColor = true;
            this.chatViewLoadOlderButton.Visible = false;
            this.chatViewLoadOlderButton.Click += new System.EventHandler(this.chatViewLoadOlderButton_Click);
            // 
            // chatViewSendButton
            // 
            this.chatViewSendButton.Location = new System.Drawing.Point(726, 471);
            this.chatViewSendButton.Name = "chatViewSendButton";
            this.chatViewSendButton.Size = new System.Drawing.Size(86, 53);
            this.chatViewSendButton.TabIndex = 36;
            this.chatViewSendButton.Text = "Send";
            this.chatViewSendButton.UseVisualStyleBackColor = true;
            this.chatViewSendButton.Visible = false;
            this.chatViewSendButton.Click += new System.EventHandler(this.chatViewSendButton_Click);
            // 
            // StartupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(824, 535);
            this.Controls.Add(this.chatViewSendButton);
            this.Controls.Add(this.chatViewLoadOlderButton);
            this.Controls.Add(this.chatViewListBox);
            this.Controls.Add(this.chatTextBox);
            this.Controls.Add(this.logoutButton);
            this.Controls.Add(this.acceptFriendRequestButton);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.outgoingFriendRequestsListView);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.incomingFriendRequestsListView);
            this.Controls.Add(this.friendsListView);
            this.Controls.Add(this.removeFriendButton);
            this.Controls.Add(this.findPeopleButton);
            this.Controls.Add(this.friendsLabel);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.createAccountButton);
            this.Controls.Add(this.emailCreateAccountTextBox);
            this.Controls.Add(this.confirmPasswordCreateAccountTextBox);
            this.Controls.Add(this.passwordCreateAccountTextBox);
            this.Controls.Add(this.usernameCreateAccountTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.usernameTextBox);
            this.Name = "StartupForm";
            this.Text = "Neo4j IM App";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StartupForm_FormClosed);
            this.Load += new System.EventHandler(this.StartupForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox usernameCreateAccountTextBox;
        private System.Windows.Forms.TextBox passwordCreateAccountTextBox;
        private System.Windows.Forms.TextBox confirmPasswordCreateAccountTextBox;
        private System.Windows.Forms.TextBox emailCreateAccountTextBox;
        private System.Windows.Forms.Button createAccountButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label friendsLabel;
        private System.Windows.Forms.Button findPeopleButton;
        private System.Windows.Forms.Button removeFriendButton;
        private System.Windows.Forms.ListView friendsListView;
        private System.Windows.Forms.ListView incomingFriendRequestsListView;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ListView outgoingFriendRequestsListView;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button acceptFriendRequestButton;
        private System.Windows.Forms.Button logoutButton;
        private System.Windows.Forms.TextBox chatTextBox;
        private System.Windows.Forms.ListBox chatViewListBox;
        private System.Windows.Forms.Button chatViewLoadOlderButton;
        private System.Windows.Forms.Button chatViewSendButton;
    }
}

