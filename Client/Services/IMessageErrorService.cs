using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Services
{
    public interface IMessageErrorService
    {
        void ShowNotification(string message, string caption = "", bool autoClose = false);
        bool ShowConfirmationRequest(string message, string caption = "", bool autoClose = false);
    }
}
