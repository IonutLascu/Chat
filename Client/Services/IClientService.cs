using Client;
using Client.Enums;
using Client.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
       
        event Action<string, int> InviteToPlay;
        event Action<string, string> GetResponse;
        event Action<string, int, int, int, int, bool?, string> ReceiveMove;
        event Action<string, bool> NotifyIsInGame;
        event Action ParticipantDisconnectedWinGame;

        Task ConnectAsync();
        //receive list of users and message 
        Task<Tuple<List<User>, string>> LoginAsync(string name, string password);
        Task<string> RegisterAsync(string name, string password, string email);
        Task LogoutAsync();
        Task SendBroadcastMessageAsync(string msg);
        Task SendUnicastMessageAsync(string recepient, string msg);
        Task TypingAsync(string recepient);

        Task SendInviteToPlayAsync(string recepient, int time);
        Task SendResponseAsync(string recepient, object response);
        Task SendMoveAsync(string recepient, int fromR, int fromC, int toR, int toC, 
            bool? isFinish, string pieceWasChanged);
        Task NotifyAllAsync(string recepient, bool isInGame);
        Task NotifyOpponentGameIsFinished(string name);
    }
}