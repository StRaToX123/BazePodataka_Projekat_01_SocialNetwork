using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Server.DomainModel;
using Neo4jClient;
using Neo4jClient.Cypher;

namespace Server
{
    public partial class CreateAccount : Form
    {
        public GraphClient client;
        public CreateAccount()
        {
            InitializeComponent();
        }

        // Create Button
        private void button1_Click(object sender, EventArgs e)
        {

            Account account = new Account();
            account.username = usernameTextBox.Text;
            account.password = passwordTextBox.Text;
            account.email = emailTextBox.Text;
            string maxId = GetMaxId();
            if (maxId == null)
            {
                account.id = "0";
            }
            else
            {
                try
                {
                    int mId = Int32.Parse(maxId);
                    mId++;
                    account.id = mId.ToString();
                }
                catch (Exception exception)
                {
                    account.id = "";
                }
            }
            
            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("username", account.username);
            queryDict.Add("password", account.password);
            queryDict.Add("email", account.email);

            // Prvo proveriti da li takav account vec postoji

            Dictionary<string, object> queryDictCheckExisting = new Dictionary<string, object>();
            queryDictCheckExisting.Add("username", account.username);

            var queryCheckExisting = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Account) and exists(n.username) and n.username = '" + account.username + "' return n limit 1",
                                                            queryDictCheckExisting, CypherResultMode.Set);

            List<Account> existingAccounts = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryCheckExisting).ToList();

            if (existingAccounts.Count != 0)
            {
                MessageBox.Show("Vec postoji takav account");
                return;
            }

            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Account {id:'" + account.id + "', username:'" + account.username 
                                                            + "', password:'" + account.password + "', email:'" + account.email
                                                            + "'}) return n",
                                                            queryDict, CypherResultMode.Set);

            List<Account> accounts = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(query).ToList();

            foreach (Account a in accounts)
            {
                MessageBox.Show(a.username);
            }

            //NodeReference<Actor> newActor = client.Create(actor);

            this.Close();
        }

        


        private String GetMaxId()
        {
            var query = new Neo4jClient.Cypher.CypherQuery("match (n:Account) where exists(n.id) return max(n.id)",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);

            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(query).ToList().FirstOrDefault();

            return maxId;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
