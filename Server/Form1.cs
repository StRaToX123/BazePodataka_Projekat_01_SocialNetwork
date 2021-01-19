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

using Microsoft.Owin.Hosting;
using Server.SignalRServer;

namespace Server
{
    public partial class Form1 : Form
    {
        private GraphClient client;
        private IDisposable signalRServer;
        //relationships:RATED, ACTS_IN, FRIEND, DIRECTED

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string url = @"http://localhost:8080/";
            signalRServer = WebApp.Start<Startup>(url);
        }
    }
}
