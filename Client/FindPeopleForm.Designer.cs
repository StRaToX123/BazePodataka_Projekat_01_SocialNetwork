
namespace Client
{
    partial class FindPeopleForm
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
            this.findPeopleListView = new System.Windows.Forms.ListView();
            this.findPeopleTextBox = new System.Windows.Forms.TextBox();
            this.findPeopleButton = new System.Windows.Forms.Button();
            this.sendFriendRequestButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // findPeopleListView
            // 
            this.findPeopleListView.HideSelection = false;
            this.findPeopleListView.Location = new System.Drawing.Point(12, 38);
            this.findPeopleListView.Name = "findPeopleListView";
            this.findPeopleListView.Size = new System.Drawing.Size(204, 156);
            this.findPeopleListView.TabIndex = 0;
            this.findPeopleListView.UseCompatibleStateImageBehavior = false;
            // 
            // findPeopleTextBox
            // 
            this.findPeopleTextBox.Location = new System.Drawing.Point(12, 12);
            this.findPeopleTextBox.Name = "findPeopleTextBox";
            this.findPeopleTextBox.Size = new System.Drawing.Size(123, 20);
            this.findPeopleTextBox.TabIndex = 2;
            // 
            // findPeopleButton
            // 
            this.findPeopleButton.Location = new System.Drawing.Point(141, 9);
            this.findPeopleButton.Name = "findPeopleButton";
            this.findPeopleButton.Size = new System.Drawing.Size(75, 23);
            this.findPeopleButton.TabIndex = 3;
            this.findPeopleButton.Text = "Search";
            this.findPeopleButton.UseVisualStyleBackColor = true;
            this.findPeopleButton.Click += new System.EventHandler(this.findPeopleButton_Click);
            // 
            // sendFriendRequestButton
            // 
            this.sendFriendRequestButton.Location = new System.Drawing.Point(12, 201);
            this.sendFriendRequestButton.Name = "sendFriendRequestButton";
            this.sendFriendRequestButton.Size = new System.Drawing.Size(204, 23);
            this.sendFriendRequestButton.TabIndex = 4;
            this.sendFriendRequestButton.Text = "Send Friend Request";
            this.sendFriendRequestButton.UseVisualStyleBackColor = true;
            this.sendFriendRequestButton.Click += new System.EventHandler(this.sendFriendRequestButton_Click);
            // 
            // FindPeopleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(230, 244);
            this.Controls.Add(this.sendFriendRequestButton);
            this.Controls.Add(this.findPeopleButton);
            this.Controls.Add(this.findPeopleTextBox);
            this.Controls.Add(this.findPeopleListView);
            this.Name = "FindPeopleForm";
            this.Text = "FindPeopleForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FindPeopleForm_FormClosed);
            this.Load += new System.EventHandler(this.FindPeopleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView findPeopleListView;
        private System.Windows.Forms.TextBox findPeopleTextBox;
        private System.Windows.Forms.Button findPeopleButton;
        private System.Windows.Forms.Button sendFriendRequestButton;
    }
}