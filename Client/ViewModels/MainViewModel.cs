using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.IO;
using System.Drawing;
using Client.Services;
using Client.Enums;
using Client.Models;
using Client.Commands;
using Client.Utils;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using System.Reactive.Linq;

namespace Client.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private IClientService chatService;
        private IMessageErrorService dialogService;
        private TaskFactory ctxTaskFactory;

        #region Register Bindings

        private string _newEmail;
        public string NewEmail
        {
            get => _newEmail;
            set
            {
                _newEmail = value;
                OnPropertyChanged();
            }
        }

        private string _newPassword;
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged();
            }
        }

        private string _newUsername;
        public string NewUserName
        {
            get => _newUsername;
            set
            {
                _newUsername = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Login Bindings

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        private ObservableCollection<Participant> _participants = new ObservableCollection<Participant>();
        public ObservableCollection<Participant> Participants
        {
            get { return _participants; }
            set
            {
                _participants = value;
                OnPropertyChanged();
            }
        }

        private Participant _selectedParticipant;
        public Participant SelectedParticipant
        {
            get { return _selectedParticipant; }
            set
            {
                _selectedParticipant = value;
                if (SelectedParticipant.HasSentNewMessage) SelectedParticipant.HasSentNewMessage = false;
                OnPropertyChanged();
            }
        }

        private UserState _userState;
        public UserState UserState
        {
            get { return _userState; }
            set
            {
                _userState = value;
                OnPropertyChanged();
            }
        }

        private string _textMessage;
        public string TextMessage
        {
            get { return _textMessage; }
            set
            {
                _textMessage = value;
                OnPropertyChanged();
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get { return _isConnected; }
            set
            {
                _isConnected = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                _isLoggedIn = value;
                OnPropertyChanged();
            }
        }
        #region SwitchViews Command
        private ICommand _openRegisterPage;
        public ICommand OpenRegisterPage
        {
            get 
            {
                return _openRegisterPage ?? (_openRegisterPage = new Command((o) => OpenRegister()));
            }
        }
        private void OpenRegister()
        {
            UserState = UserState.Register;
            //do not let the user and password written
            UserName = string.Empty;
            Password = string.Empty;
        }

        private ICommand _openLoginPage;
        public ICommand OpenLoginPage
        {
            get
            {
                return _openLoginPage?? (_openLoginPage = new Command((o) => OpenLogin()));
            }
        }
        private void OpenLogin()
        {
            UserState = UserState.Login;
            //do not let the user, pass and email written

            NewUserName = string.Empty;
            NewPassword = string.Empty;
            NewEmail = string.Empty;
        }

        #endregion

        #region Connect Command
        private ICommand _connectCommand;
        public ICommand ConnectCommand
        {
            get
            {
                return _connectCommand ?? (_connectCommand = new CommandAsync(() => Connect()));
            }
        }

        private async Task<bool> Connect()
        {
            try
            {
                await chatService.ConnectAsync();
                IsConnected = true;
                return true;
            }
            catch (Exception) { return false; }
        }
        #endregion

        #region Register Command
        private ICommand _registerCommand;
        public ICommand RegisterCommand
        {
            get
            {
                return _registerCommand ?? (_registerCommand =
                    new CommandAsync(() => Register(), (o) => CanRegister()));
            }
        }

        private async Task<bool> Register()
        {
            try
            {
                string result = await chatService.RegisterAsync(_newUsername, _newPassword, _newEmail);
                if (result == string.Empty)
                {
                    UserState = UserState.Login;
                    dialogService.ShowNotification("Register succesfully!");

                    NewUserName = string.Empty;
                    NewPassword = string.Empty;
                    NewEmail = string.Empty;
                    return true;
                }
                else
                {
                    dialogService.ShowNotification(result as string);
                    return false;
                }

            }
            catch (Exception) { return false; }
        }

        private bool CanRegister()
        {
            return !string.IsNullOrEmpty(NewUserName) && NewUserName.Length >= 2
                && !string.IsNullOrEmpty(NewPassword) && NewPassword.Length >= 2
                && (NewEmail != null && ValidatorExtensions.IsValidEmailAddress(NewEmail))
                && IsConnected;
        }


        #endregion

        #region Login Command
        private ICommand _loginCommand;
        public ICommand LoginCommand
        {
            get
            {
                return _loginCommand ?? (_loginCommand =
                    new CommandAsync(() => Login(), (o) => CanLogin()));
            }
        }

        private async Task<bool> Login()
        {
            try
            {
                Tuple<List<User>, string> result = null;
                result = await chatService.LoginAsync(_userName, _password);
                if (result.Item1 != null)
                {
                    result.Item1.ForEach(u => Participants.Add(new Participant { Username = u.Username}));
                    UserState = UserState.Chat;
                    IsLoggedIn = true;  
                    dialogService.ShowNotification(result.Item2);
                    return true;
                }
                else
                {
                    dialogService.ShowNotification("Username is already in use");
                    return false;
                }

            }
            catch (Exception ) { return false; }
        }

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(UserName) && UserName.Length >= 2
                && !string.IsNullOrEmpty(Password) && Password.Length >= 2
                && IsConnected;
        }
        #endregion

        #region Logout Command
        private ICommand _logoutCommand;
        public ICommand LogoutCommand
        {
            get
            {
                return _logoutCommand ?? (_logoutCommand =
                    new CommandAsync(() => Logout(), (o) => CanLogout()));
            }
        }

        private async Task<bool> Logout()
        {
            try
            {
                await chatService.LogoutAsync();
                UserState = UserState.Login;
                dialogService.ShowNotification("Client was disconected!");
                return true;
            }
            catch (Exception) { return false; }
        }

        private bool CanLogout()
        {
            return IsConnected && IsLoggedIn;
        }
        #endregion

        #region Typing Command
        private ICommand _typingCommand;
        public ICommand TypingCommand
        {
            get
            {
                return _typingCommand ?? (_typingCommand =
                    new CommandAsync(() => Typing(), (o) => CanUseTypingCommand()));
            }
        }

        private async Task<bool> Typing()
        {
            try
            {
                await chatService.TypingAsync(SelectedParticipant.Username);
                return true;
            }
            catch (Exception e) { return false; }
        }

        private bool CanUseTypingCommand()
        {
            bool result = (SelectedParticipant != null && SelectedParticipant.IsLoggedIn);
            return result;
        }
        #endregion

        #region Send Text Message Command
        private ICommand _sendTextMessageCommand;
        public ICommand SendTextMessageCommand
        {
            get
            {
                return _sendTextMessageCommand ?? (_sendTextMessageCommand =
                    new CommandAsync(() => SendTextMessage(), (o) => CanSendTextMessage()));
            }
        }

        private async Task<bool> SendTextMessage()
        {
            try
            {
                var recepient = _selectedParticipant.Username;
                await chatService.SendUnicastMessageAsync(recepient, _textMessage);
                return true;
            }
            catch (Exception e) { return false; }
            finally
            {
                ChatMessage msg = new ChatMessage
                {
                    Author = UserName,
                    Message = _textMessage,
                    Time = DateTime.Now,
                    IsOriginNative = true
                };
                SelectedParticipant.Chatter.Add(msg);
                TextMessage = string.Empty;
            }
        }

        private bool CanSendTextMessage()
        {
            return (!string.IsNullOrEmpty(TextMessage) && IsConnected &&
                _selectedParticipant != null && _selectedParticipant.IsLoggedIn);
        }
        #endregion

        #region Event Handlers
        private void NewTextMessage(string name, string msg, MessageType mt)
        {
            if (mt == MessageType.Unicast)
            {
                ChatMessage cm = new ChatMessage { Author = name, Message = msg, Time = DateTime.Now };
                var sender = _participants.Where((u) => string.Equals(u.Username, name)).FirstOrDefault();
                ctxTaskFactory.StartNew(() => sender.Chatter.Add(cm)).Wait();

                if (!(SelectedParticipant != null && sender.Username.Equals(SelectedParticipant.Username)))
                {
                    ctxTaskFactory.StartNew(() => sender.HasSentNewMessage = true).Wait();
                }
            }
        }

        private void ParticipantLogin(User u)
        {
            var ptp = Participants.FirstOrDefault(p => string.Equals(p.Username, u.Username));
            if (_isLoggedIn && ptp == null)
            {
                ctxTaskFactory.StartNew(() => Participants.Add(new Participant
                {
                    Username = u.Username,
                })).Wait();
            }
        }

        private void ParticipantDisconnection(string name)
        {
            var person = Participants.Where((p) => string.Equals(p.Username, name)).FirstOrDefault();
            if (person != null) person.IsLoggedIn = false;
        }

        private void ParticipantReconnection(string name)
        {
            var person = Participants.Where((p) => string.Equals(p.Username, name)).FirstOrDefault();
            if (person != null) person.IsLoggedIn = true;
        }

        private void Reconnecting()
        {
            IsConnected = false;
            IsLoggedIn = false;
        }

        private async void Reconnected()
        {
            if (!string.IsNullOrEmpty(_userName))
                await chatService.LoginAsync(_userName, _password);
            IsConnected = true;
            IsLoggedIn = true;
        }

        private async void Disconnected()
        {
            var connectionTask = chatService.ConnectAsync();
            await connectionTask.ContinueWith(t => {
                if (!t.IsFaulted)
                {
                    IsConnected = true;
                    chatService.LoginAsync(_userName, _password).Wait();
                    IsLoggedIn = true;
                }
            });
        }

        private void ParticipantTyping(string name)
        {
            var person = Participants.Where((p) => string.Equals(p.Username, name)).FirstOrDefault();
            if (person != null && !person.IsTyping)
            {
                person.IsTyping = true;
                Observable.Timer(TimeSpan.FromMilliseconds(1500)).Subscribe(t => person.IsTyping = false);
            }
        }
        #endregion

        public MainViewModel(IClientService chatSvc, IMessageErrorService diagSvc)
        {
            dialogService = diagSvc;
            chatService = chatSvc;

            chatSvc.NewTextMessage += NewTextMessage;
            chatSvc.ParticipantLoggedIn += ParticipantLogin;
            chatSvc.ParticipantLoggedOut += ParticipantDisconnection;
            chatSvc.ParticipantDisconnected += ParticipantDisconnection;
            chatSvc.ParticipantReconnected += ParticipantReconnection;
            chatSvc.ParticipantTyping += ParticipantTyping;
            chatSvc.ConnectionReconnecting += Reconnecting;
            chatSvc.ConnectionReconnected += Reconnected;
            chatSvc.ConnectionClosed += Disconnected;

            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
        }

    }
}