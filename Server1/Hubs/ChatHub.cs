using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    [HubName("ChatHub")]
    public class ChatHub : Hub<IClient>
    {
        private static ConcurrentDictionary<string, User> ChatClients = new ConcurrentDictionary<string, User>();

        public override Task OnDisconnected(bool stopCalled)
        {
            var userName = ChatClients.SingleOrDefault((c) => c.Value.UserId == Context.ConnectionId).Key;
            if (userName != null)
            {
                Clients.Others.ParticipantDisconnection(userName);
                Console.WriteLine($"<> {userName} disconnected");
            }
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            var userName = ChatClients.SingleOrDefault((c) => c.Value.UserId == Context.ConnectionId).Key;
            if (userName != null)
            {
                Clients.Others.ParticipantReconnection(userName);
                Console.WriteLine($"== {userName} reconnected");
            }
            return base.OnReconnected();
        }

        public string Register(string name, string password, string email)
        {
            Console.WriteLine($"User {name} trying to register...");

            if (true == DBQuery.CheckUser(name))
            {
                Console.WriteLine($"Username {name} already exists");
                return $"Username {name} already exists";
            }
            if (true == DBQuery.CheckEmail(email))
            {
                Console.WriteLine($"Email {email} already exists");
                return $"Email {email} already exists";
            }

            //ok register in db
            DBQuery.CreateUser(new User { Username = name, Email = email, Password = password});
            Console.WriteLine("Register succesfully");

            //everything was ok
            return string.Empty;
        }

        public dynamic Login(string name, string password)
        {
            Tuple<List<User>, string> result = null;
            string errorMsg = string.Empty;
            Console.WriteLine($"User {name} is trying to connect...");

            if (false == DBQuery.CheckUser_Password(name, password))
            {
                errorMsg = "Username or password incorect";
                Console.WriteLine(errorMsg);
            }
            else if (!ChatClients.ContainsKey(name))
            {
                Console.WriteLine($"++ {name} logged in");
                List<User> users = new List<User>(ChatClients.Values);
                User newUser = new User { UserId = Context.ConnectionId, Username = name, Password = password };
                var added = ChatClients.TryAdd(name, newUser);
                if (!added)
                    return null;
                Clients.CallerState.UserName = name;
                Clients.Others.ParticipantLogin(newUser);
                result = new Tuple<List<User>, string>(users, $"Login succesfully!");
                return result;
            }
            return new Tuple<List<User>, string>(null, errorMsg);
        }

        public void Logout()
        {
            var name = Clients.CallerState.UserName;
            if (!string.IsNullOrEmpty(name))
            {
                User client = new User();
                ChatClients.TryRemove(name, out client);
                Clients.Others.ParticipantLogout(name);
                Console.WriteLine($"-- {name} logged out");
            }
        }

        public void BroadcastTextMessage(string message)
        {
            var name = Clients.CallerState.UserName;
            if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(message))
            {
                Clients.Others.BroadcastTextMessage(name, message);
            }
        }

        public void UnicastTextMessage(string recepient, string message)
        {
            var sender = Clients.CallerState.UserName;
            if (!string.IsNullOrEmpty(sender) && recepient != sender &&
                !string.IsNullOrEmpty(message) && ChatClients.ContainsKey(recepient))
            {
                User client = new User();
                ChatClients.TryGetValue(recepient, out client);
                Clients.Client(client.UserId).UnicastTextMessage(sender, message);
            }
        }

        public void Typing(string recepient)
        {
            if (string.IsNullOrEmpty(recepient)) return;
            var sender = Clients.CallerState.UserName;
            User client = new User();
            ChatClients.TryGetValue(recepient, out client);
            Clients.Client(client.UserId).ParticipantTyping(sender);
        }
    }
}
