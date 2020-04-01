using Client;
using Client.Enums;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IClientService
    {
        event Action<User> ParticipantLoggedIn;
        event Action<string> ParticipantLoggedOut;
        event Action<string> ParticipantDisconnected;
        event Action<string> ParticipantReconnected;
        event Action ConnectionReconnecting;
        event Action ConnectionReconnected;
        event Action ConnectionClosed;
        event Action<string, string, MessageType> NewTextMessage;
        event Action<string> ParticipantTyping;
        event Action<string> InviteToPlay;
        event Action<string, string> GetResponse;

        Task ConnectAsync();
        //receive list of users and message 
        Task<Tuple<List<User>, string>> LoginAsync(string name, string password);
        Task<string> RegisterAsync(string name, string password, string email);
        Task LogoutAsync();
        Task SendBroadcastMessageAsync(string msg);
        Task SendUnicastMessageAsync(string recepient, string msg);
        Task TypingAsync(string recepient);
        Task SendInviteToPlayAsync(string recepient);
        Task SendResponseAsync(string recepient, object response);
    }
}