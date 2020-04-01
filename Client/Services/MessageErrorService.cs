using Client.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Client.Services
{
    public class MessageErrorService : IMessageErrorService
    {
        public bool ShowConfirmationRequest(string message, string caption = "", bool autoClose = false)
        {
            var result = new MessageBoxCustom(message, MessageTypeBox.Confirmation, MessageButtons.YesNo, autoClose).ShowDialog();
            return (bool)result;
        }

        public void ShowNotification(string message, string caption = "", bool autoClose = false) 
            => new MessageBoxCustom(message, MessageTypeBox.Confirmation, MessageButtons.Ok, autoClose).ShowDialog();
    }
}
