using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNet.SignalR.Client;
using Client.Enums;
using Client.Models;

namespace Client.Services
{
    public class ClientService : IClientService
    {
        public event Action<string, string, MessageType> NewTextMessage;
        public event Action<string> ParticipantDisconnected;
        public event Action<User> ParticipantLoggedIn;
        public event Action<string> ParticipantLoggedOut;
        public event Action<string> ParticipantReconnected;
        public event Action ConnectionReconnecting;
        public event Action ConnectionReconnected;
        public event Action ConnectionClosed;
        public event Action<string> ParticipantTyping;

        public event Action<string> InviteToPlay;
        public event Action<string, string> GetResponse;
        public event Action<string, int, int, int, int, bool> ReceiveMove;

        private IHubProxy hubProxy;
        private HubConnection connection;
        private string url = "http://localhost:8080/signalchat";

        public async Task ConnectAsync()
        {
            connection = new HubConnection(url);

            connection.TraceLevel = TraceLevels.All;
            connection.TraceWriter = Console.Out;

            hubProxy = connection.CreateHubProxy("ChatHub");
            hubProxy.On<string>("ParticipantDisconnection", (n) => ParticipantDisconnected?.Invoke(n));
            hubProxy.On<string>("ParticipantLogout", (n) => ParticipantLoggedOut?.Invoke(n));
            hubProxy.On<User>("ParticipantLogin", (u) => ParticipantLoggedIn?.Invoke(u));
            hubProxy.On<string>("ParticipantReconnection", (n) => ParticipantReconnected?.Invoke(n));
            hubProxy.On<string, string>("BroadcastTextMessage", (n, m) => NewTextMessage?.Invoke(n, m, MessageType.Broadcast));
            hubProxy.On<string, string>("UnicastTextMessage", (n, m) => NewTextMessage?.Invoke(n, m, MessageType.Unicast));
            hubProxy.On<string>("ParticipantTyping", (p) => ParticipantTyping?.Invoke(p));
            hubProxy.On<string>("InviteToPlay", (p) => InviteToPlay?.Invoke(p));
            hubProxy.On<string, string>("GetResponse", (p, q) => GetResponse?.Invoke(p, q));
            hubProxy.On<string, int, int, int, int, bool>("ReceiveMove", (p1, p2, p3, p4, p5, p6) => ReceiveMove?.Invoke(p1, p2, p3, p4, p5, p6));

            connection.Reconnecting += Reconnecting;
            connection.Reconnected += Reconnected;
            connection.Closed += Disconnected;

            ServicePointManager.DefaultConnectionLimit = 10;
            await connection.Start();
        }

        private void Disconnected()
        {
            ConnectionClosed?.Invoke();
        }

        private void Reconnected()
        {
            ConnectionReconnected?.Invoke();
        }

        private void Reconnecting()
        {
            ConnectionReconnecting?.Invoke();
        }

        //return dynamic element because if the username or password 
        //are wrong maybe i want to know as user
        public async Task<Tuple<List<User>, string>> LoginAsync(string name, string password)
        {
            return await hubProxy.Invoke<Tuple<List<User>, string>>("Login", new object[] { name, Utils.HashPassword.ComputeSha256Hash(password)});
        }

        public async Task<string> RegisterAsync(string name, string password, string email)
        {
            return await hubProxy.Invoke<string>("Register", new object[] { name, Utils.HashPassword.ComputeSha256Hash(password), email });
        }

        public async Task LogoutAsync()
        {
            await hubProxy.Invoke("Logout");
        }

        public async Task SendBroadcastMessageAsync(string msg)
        {
            await hubProxy.Invoke("BroadcastTextMessage", msg);
        }

        public async Task SendUnicastMessageAsync(string recepient, string msg)
        {
            await hubProxy.Invoke("UnicastTextMessage", new object[] { recepient, msg });
        }

        public async Task TypingAsync(string recepient)
        {
            await hubProxy.Invoke("Typing", recepient);
        }

        public async Task SendInviteToPlayAsync(string recepient)
        {
            await hubProxy.Invoke("SendInviteToPlay", recepient);
        }

        public async Task SendResponseAsync(string recepient, object response)
        {
            await hubProxy.Invoke("SendResponseBack", new object[] { recepient, response });
        }

        public async Task SendMoveAsync(string recepient, int fromR, int fromC, int toR, int toC, bool isFinish)
        {
            await hubProxy.Invoke("SendMoveAsync", new object[] {recepient, fromR, fromC, toR, toC, isFinish});
        }
    }
}