using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public interface IClient
    {
        void ParticipantDisconnection(string name);
        void ParticipantReconnection(string name);
        void ParticipantLogin(User client);
        void ParticipantLogout(string name);
        void BroadcastTextMessage(string sender, string message);
        void UnicastTextMessage(string sender, string message);
        void ParticipantTyping(string sender);
        Task InviteToPlay(string sender, int time);
        void GetResponse(string sender, object response);
        void ReceiveMove(string sender, int fR, int fC, int tR, int tC, bool? isFinish, string pieceWasChanged);
        void NotifyIsInGame(string sender, bool isInGame);
    }
}
