using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Concurrent;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

using Server.DomainModel;

using Neo4jClient;
using Neo4jClient.Cypher;

namespace Server.SignalRServer
{
    [HubName("TestHub")]
    public class TestHub : Hub
    {
        private GraphClient client;
        // imamo dva recnika jedan u kome je connectionID ismedju servera i klijenta kljuc koji vodi ka account username
        // i reverse recnik od toga, gde account username vodi ka connectionID.
        // Potrebno za dostavljanje informacije o tome ko je online a ko nije
        public static ConcurrentDictionary<string, string> connectedUsersConnectionIDToUsername = new ConcurrentDictionary<string, string>();
        public static ConcurrentDictionary<string, string> connectedUsersUsernameToConnectionID = new ConcurrentDictionary<string, string>();

        public TestHub()
        {
            client = new GraphClient(new Uri("http://localhost:7474/db/data"), "neo4j", "edukacija");
            try
            {
                client.Connect();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }
  
        public override Task OnDisconnected(bool stopCalled)
        {
            string username;
            connectedUsersConnectionIDToUsername.TryGetValue(Context.ConnectionId, out username);
            if (username != null)
            {
                Dictionary<string, object> queryDictGetFriends = new Dictionary<string, object>();
                queryDictGetFriends.Add("username", username);

                var queryGetFriends = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:FRIEND]-(b:Account) where a.username = '" + username + "' return b.username",
                                                                queryDictGetFriends, CypherResultMode.Set);

                List<string> listOfFriends = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(queryGetFriends).ToList();
                for (int i = 0; i < listOfFriends.Count; i++)
                {
                    string friendConnectionID;
                    connectedUsersUsernameToConnectionID.TryGetValue(listOfFriends[i], out friendConnectionID);
                    if (friendConnectionID != null)
                    {
                        Clients.Client(friendConnectionID).FriendBecameOffline(username);
                    }
                }

                // Sada mozemo da obrisemo klijenta koji se diskonektovao is nasih recnika
                string garbage;
                connectedUsersUsernameToConnectionID.TryRemove(username, out garbage);
                connectedUsersConnectionIDToUsername.TryRemove(Context.ConnectionId, out garbage);
            }
            
            return base.OnDisconnected(stopCalled);
        }
        

        public void LogIn(string username, string password)
        {
            // Proveriti da li se ovaj client vec login-ovao
            string existingConnectionID;
            connectedUsersConnectionIDToUsername.TryGetValue(Context.ConnectionId, out existingConnectionID);
            if (existingConnectionID != null)
            {
                Clients.Caller.LogInFailed("nista");
                return;
            }

            // Proveriti da li ovaj nalog postoji
            // I da li je pogodjena lozinka
            Dictionary<string, object> queryDictCheckExisting = new Dictionary<string, object>();
            queryDictCheckExisting.Add("username", username);
            queryDictCheckExisting.Add("password", password);

            var queryCheckExisting = new Neo4jClient.Cypher.CypherQuery("match (n:Account) where n.username = '" + username + "' and n.password = '" + password + "' return n",
                                                            queryDictCheckExisting, CypherResultMode.Set);

            List<Account> existingAccounts = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryCheckExisting).ToList();

            if (existingAccounts.Count == 0)
            {
                //MessageBox.Show("Vec postoji takav account");
                Clients.Caller.LogInFailed("nesto");
                return;
            }
            else
            {
                connectedUsersConnectionIDToUsername.TryAdd(Context.ConnectionId, username);
                connectedUsersUsernameToConnectionID.TryAdd(username, Context.ConnectionId);
                // Skupi imena svih prijatelja ovog naloga, svih outgoing i incoming friend requests
                // Onda cemo vratiti te informacije korisniku kao jedan string koji ce on da raspakuje
                string friendDataString = "";
                // Prvo pribavljanje incoming friendRequests
                Dictionary<string, object> queryDictGetIncomingFriendRequests = new Dictionary<string, object>();
                queryDictGetIncomingFriendRequests.Add("username", username);
                var queryGetIncomingFriendRequests = new Neo4jClient.Cypher.CypherQuery("match (a:Account)<-[r:FRIEND_REQUEST]-(b:Account) where a.username = '" + username + "' return b.username",
                                                                queryDictGetIncomingFriendRequests, CypherResultMode.Set);

                List<string> listOfIncomingFriendRequests = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(queryGetIncomingFriendRequests).ToList();
                for (int i = 0; i < listOfIncomingFriendRequests.Count; i++)
                {
                    friendDataString += listOfIncomingFriendRequests[i];
                    if (i != (listOfIncomingFriendRequests.Count - 1))
                    {
                        friendDataString += "@";
                    }
                }

                friendDataString += "#";

                // Sada pribaviti outoging friendRequests
                Dictionary<string, object> queryDictGetOutgoingFriendRequests = new Dictionary<string, object>();
                queryDictGetOutgoingFriendRequests.Add("username", username);
                var queryGetOutgoingFriendRequests = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:FRIEND_REQUEST]->(b:Account) where a.username = '" + username + "' return b.username",
                                                                queryDictGetOutgoingFriendRequests, CypherResultMode.Set);

                List<string> listOfOutgoingFriendRequests = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(queryGetOutgoingFriendRequests).ToList();
                for (int i = 0; i < listOfOutgoingFriendRequests.Count; i++)
                {
                    friendDataString += listOfOutgoingFriendRequests[i];
                    if (i != (listOfOutgoingFriendRequests.Count - 1))
                    {
                        friendDataString += "@";
                    }
                }

                friendDataString += "#"; // da razdvojimo outgoing friend requests od friends

                // Sada pribaviti lisu prijatelja
                Dictionary<string, object> queryDictGetFriends= new Dictionary<string, object>();
                queryDictGetFriends.Add("username", username);

                var queryGetFriends = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:FRIEND]-(b:Account) where a.username = '" + username + "' return b.username",
                                                                queryDictGetFriends, CypherResultMode.Set);

                List<string> listOfFriends = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(queryGetFriends).ToList();
                for (int i = 0; i < listOfFriends.Count; i++)
                {
                    friendDataString += listOfFriends[i];
                    friendDataString += " ";
                    // Provera da li je taj prijatelj online
                    connectedUsersUsernameToConnectionID.TryGetValue(listOfFriends[i], out existingConnectionID);
                    if (existingConnectionID != null)
                    {
                        friendDataString += "true";
                        // Javi ovom korisniku da smo mi sada online
                        Clients.Client(existingConnectionID).FriendBecameOnline(username);
                    }
                    else
                    {
                        friendDataString += "false";
                    }

                    if (i != (listOfFriends.Count - 1))
                    {
                        friendDataString += "@";
                    }
                }
                
                Clients.Caller.LogInSuccessful(friendDataString);
            }
        }

        // Radi isto sto i Disconnect samo na zahtev korisnika
        public void LogOut(string nista)
        {
            // Prvo moramo javiti 
            string callerUsername;
            connectedUsersConnectionIDToUsername.TryGetValue(Context.ConnectionId, out callerUsername);
            if (callerUsername != null)
            {
                Dictionary<string, object> queryDictGetFriends = new Dictionary<string, object>();
                queryDictGetFriends.Add("username", callerUsername);

                var queryGetFriends = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:FRIEND]-(b:Account) where a.username = '" + callerUsername + "' return b.username",
                                                                queryDictGetFriends, CypherResultMode.Set);

                List<string> listOfFriends = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(queryGetFriends).ToList();
                for (int i = 0; i < listOfFriends.Count; i++)
                {
                    string friendConnectionID;
                    connectedUsersUsernameToConnectionID.TryGetValue(listOfFriends[i], out friendConnectionID);
                    if (friendConnectionID != null)
                    {
                        Clients.Client(friendConnectionID).FriendBecameOffline(callerUsername);
                    }
                }

                // Sada mozemo da obrisemo klijenta koji se diskonektovao is nasih recnika
                string garbage;
                connectedUsersUsernameToConnectionID.TryRemove(callerUsername, out garbage);
                connectedUsersConnectionIDToUsername.TryRemove(Context.ConnectionId, out garbage);

                Clients.Caller.LogOutSuccessful("nista");
            }      
        }

        public void GetFriendsFromFriend(string friendUsername)
        {
            Dictionary<string, object> queryDictGetFriends = new Dictionary<string, object>();
            queryDictGetFriends.Add("username", friendUsername);

            var queryGetFriends = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:FRIEND]-(b:Account) where a.username = '" + friendUsername + "' return b.username",
                                                            queryDictGetFriends, CypherResultMode.Set);

            List<string> listOfFriends = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(queryGetFriends).ToList();

            string resultString = friendUsername;
            resultString += "@";
            for (int i = 0; i < listOfFriends.Count; i++)
            {
                resultString += listOfFriends[i];
                if (i != (listOfFriends.Count - 1))
                {
                    resultString += "@";
                }
            }

            Clients.Caller.FriendsFromFriendResult(resultString);
        }

        public void DetermineLength(string message)
        {
            Console.WriteLine(message);

            string newMessage = string.Format(@"{0} has a length of: {1}", message, message.Length);
            Clients.Caller.ReceiveLength(newMessage);
        }

        public void CreateAccount(string username, string password, string email)
        {
            Account account = new Account();
            account.username = username;
            account.password = password;
            account.email = email;
            account.isOnline = "false";
            string maxId = GetAccountMaxId();
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

            

            // Prvo proveriti da li takav account vec postoji

            Dictionary<string, object> queryDictCheckExisting = new Dictionary<string, object>();
            queryDictCheckExisting.Add("username", account.username);
            var queryCheckExisting = new Neo4jClient.Cypher.CypherQuery("start n=node(*) where (n:Account) and exists(n.username) and n.username = '" + account.username + "' return n limit 1",
                                                            queryDictCheckExisting, CypherResultMode.Set);

            List<Account> existingAccounts = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryCheckExisting).ToList();

            if (existingAccounts.Count != 0)
            {
                //MessageBox.Show("Vec postoji takav account");
                Clients.Caller.AccountCreationFailed("nesto");
                return;
            }

            Dictionary<string, object> queryDict = new Dictionary<string, object>();
            queryDict.Add("username", account.username);
            queryDict.Add("password", account.password);
            queryDict.Add("email", account.email);
            queryDict.Add("isOnline", "false");
            var query = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Account {id:'" + account.id + "', username:'" + account.username
                                                            + "', password:'" + account.password + "', email:'" + account.email
                                                            + "'}) return n",
                                                            queryDict, CypherResultMode.Set);

            List<Account> accounts = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(query).ToList();

            Clients.Caller.AccountCreatedSuccessfuly("nista");
        }


        private String GetAccountMaxId()
        {
            var query = new Neo4jClient.Cypher.CypherQuery("match (n:Account) where exists(n.id) return max(n.id)",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);

            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(query).ToList().FirstOrDefault();

            return maxId;
        }

        public void SendFriendRequest(string friendRequestUsername)
        {
            // Klijent pre slanja zahteva za prijateljstvo prvo proverava kod sebe da li je taj korisnik u nekim od svojih lista
            // i ako jeste nece slati zahtev.
            // Posto je moguca situacija da dva klijenta u odprilike isto vreme posalju zahtev za prijateljstvo jedni drugome,
            // njihova logika za proveravanje duplikata na klijentskoj strani ih nece zaustaviti od slanja zahteva.
            // U tom slucaju server ce dobiti dva zahteva koji bi stvorili jednu kruznu relaziju, posto nam je cilj da izbegnemo 
            // kruzne relacije, moracemo da uradimo sledece
            // 1 - proveriti da li korisnik kome se salje zahtev uopste postoji, ako ne odgovoriti klijentu
            // 2 - proveriti da li neka relacija tipa FRIEND_REQUEST vec postoji izmedju ova dva naloga, ako da nista ne moramo preuzeti nadalje
            // 3 - ako su 1 i 2 netacni onda mozemo napraviti relaciju FRIEND_REQUEST

            string callerUsername;
            connectedUsersConnectionIDToUsername.TryGetValue(Context.ConnectionId, out callerUsername);

            // Proveri da li nalog sa imenom friendRequestUsername postoji
            Dictionary<string, object> queryDictExistAccount = new Dictionary<string, object>();
            queryDictExistAccount.Add("username", friendRequestUsername);
            var queryExistAccount = new Neo4jClient.Cypher.CypherQuery("match (a:Account) where a.username = '" + friendRequestUsername + 
                                                                                                                                   "' return a",
                                                            queryDictExistAccount, CypherResultMode.Set);

            List<Account> existingAccounts = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryExistAccount).ToList();
            if (existingAccounts.Count == 0)
            {
                // ovo je call back da stavimo do znanja korisniku da account kome zeli da posalje friend request ne postoji
                Clients.Caller.FailedFriendRequest("nesto");
                return;
            }

            // Proveri da li vec postoji neka veza izmedju ova dva korisnika kada je upitanju zahtev za prijatelja
            Dictionary<string, object> queryDictExistingFriendRequest = new Dictionary<string, object>();
            queryDictExistingFriendRequest.Add("username", friendRequestUsername);
            var queryExistingFriendRequest = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:FRIEND_REQUEST]-(b:Account) where a.username = '" + callerUsername
                                                                            + "' and b.username = '" + friendRequestUsername +
                                                                            "' return b",
                                                            queryDictExistingFriendRequest, CypherResultMode.Set);

            List<Account> existingFriendRequest = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryExistingFriendRequest).ToList();
            if(existingFriendRequest.Count != 0)
            {
                // Nema potrebe nista da javljamo korisniku
                return;
            }

            // Ako ne postoji zahtev za prijatelja izmedju ova dva korisnika
            // Mi cemo ga ovde postaviti
            Dictionary<string, object> queryDictCreateFriendRequest = new Dictionary<string, object>();
            queryDictCreateFriendRequest.Add("username", friendRequestUsername);
            var queryCreateFriendRequest = new Neo4jClient.Cypher.CypherQuery("match (a:Account), (b:Account) where a.username = '" + callerUsername
                                                                            + "' and b.username = '" + friendRequestUsername +
                                                                            "' create (a)-[r:FRIEND_REQUEST]->(b) return b",
                                                            queryDictCreateFriendRequest, CypherResultMode.Set);
            List<Account> createdFriendRequest = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryCreateFriendRequest).ToList();


            // Ako je friendRequestUsername online, poslati mu update
            string friendRequestConnectionID;
            connectedUsersUsernameToConnectionID.TryGetValue(friendRequestUsername, out friendRequestConnectionID);
            if (friendRequestConnectionID != null)
            {
                Clients.Client(friendRequestConnectionID).FriendRequestArrived(callerUsername);
            }

            // ovo je call back da stavimo do znanja korisniku da je friendRequest uspesno dostavljen
            Clients.Caller.FriendRequestSentOut(friendRequestUsername);
        }

        public void AcceptFriendRequest(string friendRequestUsername)
        {
            string callerUsername;
            connectedUsersConnectionIDToUsername.TryGetValue(Context.ConnectionId, out callerUsername);

            // Prvo treba da sklonimo FRIEND_REQUEST relaciju izmedju ova dva korisnika
            Dictionary<string, object> queryDictRemoveFriendRequest = new Dictionary<string, object>();
            queryDictRemoveFriendRequest.Add("username", friendRequestUsername);
            var queryRemoveFriendRequest = new Neo4jClient.Cypher.CypherQuery("match (a:Account)<-[r:FRIEND_REQUEST]-(b:Account) where a.username = '" + callerUsername
                                                                            + "' and b.username = '" + friendRequestUsername +
                                                                            "' delete r return b",
                                                            queryDictRemoveFriendRequest, CypherResultMode.Set);
            List<Account> deletedFriendRequest = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryRemoveFriendRequest).ToList();

            // Sada napraviti FRIEND relaciju izmedju ova dva korisnika
            Dictionary<string, object> queryDictCreateFriendRelation = new Dictionary<string, object>();
            queryDictCreateFriendRelation.Add("username", friendRequestUsername);
            var queryCreateFriendRelation = new Neo4jClient.Cypher.CypherQuery("match (a:Account), (b:Account) where a.username = '" + callerUsername
                                                                            + "' and b.username = '" + friendRequestUsername +
                                                                            "' create (a)-[r:FRIEND]->(b) return b",
                                                            queryDictCreateFriendRelation, CypherResultMode.Set);
            List<Account> createdFriendRelation= ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryCreateFriendRelation).ToList();

            // Sada kada su ova dva korisnika prijatelji
            // potrebno je napraviti i nod u kome ce se pamtiti njihov chat history
            // Oba naloga treba da budu u relaciji sa ovim nodom
            // Chat node se identifikuje sa njegovim id-om
            // Zato cemo prvo nabaviti novi id za novi chat node
            string maxId = GetChatMaxId();
            if (maxId == null)
            {
                maxId = "0";
            }
            else
            {
                try
                {
                    int mId = Int32.Parse(maxId);
                    mId++;
                    maxId = mId.ToString();
                }
                catch (Exception exception)
                {
                    maxId = "";
                }
            }

            Dictionary<string, object> queryDictCreateChatNote = new Dictionary<string, object>();
            queryDictCreateChatNote.Add("id", maxId);
            var queryCreateChatNode = new Neo4jClient.Cypher.CypherQuery("CREATE (n:Chat { id: " + maxId + " }) return n",
                                                            queryDictCreateChatNote, CypherResultMode.Set);
            List<Chat> createdChatNode = ((IRawGraphClient)client).ExecuteGetCypherResults<Chat>(queryCreateChatNode).ToList();

            // Sada kada imamo chat node, napravicemo relacije izmedju korisnika koji su deo ovog chat noda
            // Svaka relacija ce imati property sa kim je u chat-u, tako da cemo moci na zahtev klijenta da nadjemo chat node 
            // samo uz pomoc imena sa kim klijent pokusava da chat-uje
            Dictionary<string, object> queryDictCreateChatRelations = new Dictionary<string, object>();
            queryDictCreateChatRelations.Add("id", maxId);
            var queryCreateChatRelations = new Neo4jClient.Cypher.CypherQuery("match (a:Account), (b:Account), (c:Chat) where a.username = '" + callerUsername +
                "' and b.username = '" + friendRequestUsername + "' and c.id = " + maxId + " create (a)-[r:CHAT {with: '" + 
                friendRequestUsername + "'}]->(c), (b)-[g:CHAT {with: '" + callerUsername + "'}]->(c) return c", 
                                                            queryDictCreateChatRelations, CypherResultMode.Set);
            List<Chat> createdChatRelations = ((IRawGraphClient)client).ExecuteGetCypherResults<Chat>(queryCreateChatRelations).ToList();

            // Ako je online korisnik cije prijateljstvo smo prihvatili, obavestiti ga
            string friendRequestUsernameConnectionID;
            connectedUsersUsernameToConnectionID.TryGetValue(friendRequestUsername, out friendRequestUsernameConnectionID);
            if (friendRequestUsernameConnectionID != null)
            {
                Clients.Client(friendRequestUsernameConnectionID).OutgoingFriendRequestAccepted(callerUsername);
                // Klijentu koji je pozvao ovu funkciju moramo vratiti i informaciju da li je osoba cije prijateljstvo je upravo prihvatio trenutno online
                // Ovu informaciju ce klijent raspakovati na njegovoj strani
                friendRequestUsername += " true";
                Clients.Caller.IncomingFriendRequestAccepted(friendRequestUsername);
            }
            else
            {
                friendRequestUsername += " false";
                Clients.Caller.IncomingFriendRequestAccepted(friendRequestUsername);
            }
        }

        private string GetChatMaxId()
        {
            var query = new Neo4jClient.Cypher.CypherQuery("match (n:Chat) where exists(n.id) return max(n.id)",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);

            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(query).ToList().FirstOrDefault();

            return maxId;
        }

        public void RemoveFriend(string friendUsername)
        {
            string callerUsername;
            connectedUsersConnectionIDToUsername.TryGetValue(Context.ConnectionId, out callerUsername);

            // Prvo cemo izbrisati chat node izmedju ova dva korisnika
            Dictionary<string, object> queryDictChatNode = new Dictionary<string, object>();
            queryDictChatNode.Add("id", "0");
            var queryChatNode= new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:CHAT]->(b:Chat) where a.username = '" + callerUsername
                                                                            + "' and r.with = '" + friendUsername +
                                                                            "' return b",
                                                            queryDictChatNode, CypherResultMode.Set);
            List<Chat> foundChatNode = ((IRawGraphClient)client).ExecuteGetCypherResults<Chat>(queryChatNode).ToList();

            // Sada izbrisati sve ChatEntry
            Dictionary<string, object> queryDictDeleteChatEntries = new Dictionary<string, object>();
            queryDictDeleteChatEntries.Add("id", "0");
            var queryDeleteChatEntries = new Neo4jClient.Cypher.CypherQuery("match (a:Chat)-[r:CHAT_ENTRY]->(b:ChatEntry) where a.id = " + foundChatNode[0].id +
                                                                            " delete r,b return a",
                                                            queryDictDeleteChatEntries, CypherResultMode.Set);
            List<Chat> deleteChatEntriesOneResult = ((IRawGraphClient)client).ExecuteGetCypherResults<Chat>(queryDeleteChatEntries).ToList();

            // Sada mozemo da izbrisemo i chat node
            // Ovo mora da se uradi u dva koraka, prvo da sklonimo relacije pa onda node, 
            // ovde cu skloniti jednu relaciju a u drugom koraku cu skloniti preostalu relaciju zajedno za chat nodom
            Dictionary<string, object> queryDictDeleteChatNodeOne = new Dictionary<string, object>();
            queryDictDeleteChatNodeOne.Add("username", callerUsername);
            var queryDeleteChatNodeOne = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:CHAT]->(b:Chat) where a.username = '" + callerUsername + "' delete r return a",
                                                            queryDictDeleteChatNodeOne, CypherResultMode.Set);
            List<Account> deleteChatNodeOneResult = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryDeleteChatNodeOne).ToList();

            Dictionary<string, object> queryDictDeleteChatNodeTwo = new Dictionary<string, object>();
            queryDictDeleteChatNodeTwo.Add("username", callerUsername);
            var queryDeleteChatNodeTwo = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:CHAT]->(b:Chat) where a.username = '" + friendUsername + "' and r.with = '" + callerUsername + "' delete r,b return a",
                                                            queryDictDeleteChatNodeTwo, CypherResultMode.Set);
            List<Account> deleteChatNodeTwoResult = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryDeleteChatNodeTwo).ToList();

            // Sada mozemo izbrisati friend relaziju izmedju ova dva korisnika
            Dictionary<string, object> queryDictRemoveFriend = new Dictionary<string, object>();
            queryDictRemoveFriend.Add("username", friendUsername);
            var queryRemoveFriend = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:FRIEND]-(b:Account) where a.username = '" + callerUsername
                                                                            + "' and b.username = '" + friendUsername +
                                                                            "' delete r return b",
                                                            queryDictRemoveFriend, CypherResultMode.Set);
            List<Account> deletedFriend = ((IRawGraphClient)client).ExecuteGetCypherResults<Account>(queryRemoveFriend).ToList();

            // Ako je korisnik koga smo izbrisali online, obavestiti ga da mu vise nismo prjatelji
            string friendUsernameConnectionID;
            connectedUsersUsernameToConnectionID.TryGetValue(friendUsername, out friendUsernameConnectionID);
            if (friendUsernameConnectionID != null)
            {
                Clients.Client(friendUsernameConnectionID).FriendRemoved(callerUsername);
            }

            Clients.Caller.FriendRemoved(friendUsername);
        }

        public void FindPeople(string findUsername)
        {
            // Pogledati u data bazi koji sve korisnici imaju username slican ovome
            Dictionary<string, object> queryDictFindPeople= new Dictionary<string, object>();
            queryDictFindPeople.Add("username", findUsername);
            var query = new Neo4jClient.Cypher.CypherQuery("match (n:Account) where n.username=~'.*" +
                                                             findUsername + ".*' return n.username",
                                                            queryDictFindPeople, CypherResultMode.Set);

            List<string> foundUsernames = ((IRawGraphClient)client).ExecuteGetCypherResults<string>(query).ToList();
            string result = "";
            for (int i = 0; i < foundUsernames.Count; i++)
            {
                result += foundUsernames[i];
                if (i != (foundUsernames.Count - 1))
                {
                    result += " ";
                }
            }

            Clients.Caller.FindPeopleResult(result);
        }


        public void GetChatHistory(string friendUsername, string clientsChatHistoryCacheSize)
        {
            string callerUsername;
            connectedUsersConnectionIDToUsername.TryGetValue(Context.ConnectionId, out callerUsername);

            // pribaviti poslednjih 20 poruka
            // Prvo pribaviti chat node namenjen za ovog klijenta i njegovom prijatelju
            Dictionary<string, object> queryDictGetChatNode = new Dictionary<string, object>();
            queryDictGetChatNode.Add("id", "0");
            var queryGetChat = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:CHAT]-(b:Chat) where a.username = '" +
                callerUsername + "' and r.with = '" + friendUsername + "' return b",
                                                            queryDictGetChatNode, CypherResultMode.Set);
            List<Chat> foundChatNodes = ((IRawGraphClient)client).ExecuteGetCypherResults<Chat>(queryGetChat).ToList();
           
            if (foundChatNodes.Count != 0)
            {
                // Sada kada znamo od kog chat node uzimamo chat entries, treba videti odakle da krenemo pribavljanje chat entries
                int clientsChatHistoryCacheSizeInt;
                Int32.TryParse(clientsChatHistoryCacheSize, out clientsChatHistoryCacheSizeInt);
                if (clientsChatHistoryCacheSizeInt == -1)
                {
                    // Ako nam je klijent poslao -1 onda znaci da nema nista u smom kešu, i vraticem mu najnovije
                    clientsChatHistoryCacheSizeInt = 0;
                }
                else
                {
                    clientsChatHistoryCacheSizeInt++;
                }

                // Sada pribaviti chat entries
                Dictionary<string, object> queryDictGetChatEntries = new Dictionary<string, object>();
                queryDictGetChatEntries.Add("id", "0");
                queryDictGetChatEntries.Add("date", "datum");
                queryDictGetChatEntries.Add("sender", "kosalje");
                queryDictGetChatEntries.Add("text", "sadrzaj");
                var queryGetChatEntries = new Neo4jClient.Cypher.CypherQuery("match (a:Chat)-[r:CHAT_ENTRY]->(b:ChatEntry) where a.id = " +
                   foundChatNodes[0].id + " return b order by b.id desc skip " + clientsChatHistoryCacheSizeInt + " limit 20",
                                                                queryDictGetChatEntries, CypherResultMode.Set);

                List<ChatEntry> foundChatEntries = ((IRawGraphClient)client).ExecuteGetCypherResults<ChatEntry>(queryGetChatEntries).ToList();

                string resultString = "";
                resultString += friendUsername;
                resultString += "#";
                for (int i = 0; i < foundChatEntries.Count; i++)
                {
                    resultString += foundChatEntries[i].date;
                    resultString += '\n';
                    resultString += foundChatEntries[i].sender;
                    resultString += ": \n";
                    resultString += foundChatEntries[i].text;
                    if (i != (foundChatEntries.Count - 1))
                    {
                        resultString += "#";
                    }
                }

                Clients.Caller.GetChatHistoryResult(resultString);
            }
        }

        public void SendMessage(string friendUsername, string message)
        {
            string callerUsername;
            connectedUsersConnectionIDToUsername.TryGetValue(Context.ConnectionId, out callerUsername);

            // Prvo nabaviti chat node za ova dva korisnika
            Dictionary<string, object> queryDictChatNode = new Dictionary<string, object>();
            queryDictChatNode.Add("id", "0");
            var queryChatNode = new Neo4jClient.Cypher.CypherQuery("match (a:Account)-[r:CHAT]->(b:Chat) where a.username = '" + callerUsername
                                                                            + "' and r.with = '" + friendUsername +
                                                                            "' return b",
                                                            queryDictChatNode, CypherResultMode.Set);
            List<Chat> foundChatNode = ((IRawGraphClient)client).ExecuteGetCypherResults<Chat>(queryChatNode).ToList();

            // Sada pribaviti koji id treba da dodelimo novoj poruci
            string maxId = GetChatEntryMaxId(foundChatNode[0].id);
            if (maxId == null)
            {
                maxId = "0";
            }
            else
            {
                try
                {
                    int mId = Int32.Parse(maxId);
                    mId++;
                    maxId = mId.ToString();
                }
                catch (Exception exception)
                {
                    maxId = "";
                }
            }

            // Sada mozemo da napravimo novi ChatEntry u databazi
            string currentDateTime = DateTime.Now.ToString("f");
            Dictionary<string, object> queryDictCreateChatEntry = new Dictionary<string, object>();
            queryDictCreateChatEntry.Add("id", "0");
            queryDictCreateChatEntry.Add("date", "datum");
            queryDictCreateChatEntry.Add("sender", "kosalje");
            queryDictCreateChatEntry.Add("text", "sadrzaj");
            var queryChatCreateChatEntry = new Neo4jClient.Cypher.CypherQuery("match (b:Chat) where b.id = " + foundChatNode[0].id +
                                                                            " create (a:ChatEntry {id: " + maxId + ", date: '" + currentDateTime + 
                                                                            "', sender: '" + callerUsername + "', text:'" + message +
                                                                            "'})<-[r:CHAT_ENTRY]-(b) return a",
                                                            queryDictCreateChatEntry, CypherResultMode.Set);
            List<ChatEntry> createdChatEntry = ((IRawGraphClient)client).ExecuteGetCypherResults<ChatEntry>(queryChatCreateChatEntry).ToList();

            
            
            string resultCaller = friendUsername;
            resultCaller += "#";
            resultCaller += currentDateTime;
            resultCaller += "\n";
            resultCaller += callerUsername;
            resultCaller += ": \n";
            resultCaller += message;
            Clients.Caller.SendMessageSuccessful(resultCaller);

            // Ako je korisnik trenutno online treba ga obavestiti o novo poslatoj poruci
            string friendUsernameConnectionID;
            connectedUsersUsernameToConnectionID.TryGetValue(friendUsername, out friendUsernameConnectionID);
            string resultFriend = "";
            if (friendUsernameConnectionID != null)
            {
                resultFriend += callerUsername;
                resultFriend += "#";
                resultFriend += currentDateTime;
                resultFriend += "\n";
                resultFriend += callerUsername;
                resultFriend += ": \n";
                resultFriend += message;
                Clients.Client(friendUsernameConnectionID).ReceivedMessage(resultFriend);
            }
        }


        private string GetChatEntryMaxId(string chatNode)
        {
            var query = new Neo4jClient.Cypher.CypherQuery("match (a:Chat)-[r:CHAT_ENTRY]->(b:ChatEntry) where a.id = " + chatNode + " and exists(b.id) return max(b.id)",
                                                            new Dictionary<string, object>(), CypherResultMode.Set);

            String maxId = ((IRawGraphClient)client).ExecuteGetCypherResults<String>(query).ToList().FirstOrDefault();

            return maxId;
        }
    }
}

