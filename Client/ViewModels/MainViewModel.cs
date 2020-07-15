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
using System.Windows;
using System.Threading;
using System.Windows.Threading;
using Chess;
using System.Collections.Specialized;
using Client.Chess;
using System.Linq.Expressions;
using GalaSoft.MvvmLight.Command;
using System.Windows.Controls;

namespace Client.ViewModels
{
    public class MainViewModel : ViewModel
    {
        private IClientService chatService;
        private IMessageErrorService dialogService;
        private TaskFactory ctxTaskFactory;

        #region Common Bindings

        private Visibility _visibilityTitle;
        public Visibility VisibilityTitle
        {
            get => _visibilityTitle;
            set
            {
                _visibilityTitle = value;
                OnPropertyChanged();
            }
        }
        #endregion

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
            //do not let the user and password written
            UserName = string.Empty;
            Password = string.Empty;
        }

        private ICommand _openLoginPage;
        public ICommand OpenLoginPage
        {
            get
            {
                return _openLoginPage ?? (_openLoginPage = new Command((o) => OpenLogin()));
            }
        }
        private void OpenLogin()
        {
            VisibilityTitle = Visibility.Visible;
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
                    VisibilityTitle = Visibility.Visible;
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
                    result.Item1.ForEach(u => Participants.Add(new Participant { Username = u.Username, IsInGame = u.isInGame }));
                    UserState = UserState.Chat;
                    VisibilityTitle = Visibility.Collapsed;
                    IsLoggedIn = true;
                    dialogService.ShowNotification(result.Item2);
                    return true;
                }
                else
                {
                    dialogService.ShowNotification(result.Item2);
                    return false;
                }

            }
            catch (Exception) { return false; }
        }

        private bool CanLogin()
        {
            return !string.IsNullOrEmpty(UserName) && UserName.Length >= 2
                && !string.IsNullOrEmpty(Password) && Password.Length >= 2
                && IsConnected;
        }
        #endregion

        #region Play Game Command

        private ICommand _surrenderCommand;
        public ICommand SurrenderCommand {
            get 
            {
                return _surrenderCommand ?? (_surrenderCommand =
                    new CommandAsync(() => Surrender(), (o) => CanSurrender()));
            } 
        
        }

        private async Task<bool> Surrender()
        {
            try
            {
                if (UserState == UserState.InGame)
                {
                    await chatService.NotifyOpponentGameIsFinishedSurrender(Table.InstanceGame.Opponent.Username);
                    await chatService.NotifyAllAsync(Table.InstanceGame.Opponent.Username, false);
                    UserState = UserState.Chat;
                }
                return true;
            }
            catch (Exception) { return false; }

        }

        private bool CanSurrender()
        {
            return true;
        }


        private int _timeInGame;
        public int TimeInGame { get => _timeInGame; set => _timeInGame = value; }

        private ICommand _playChessGameCommand;
        public ICommand PlayChessGameCommand
        {
            get
            {
                return _playChessGameCommand ?? (_playChessGameCommand =
                  new CommandAsync(() =>  SendInviteToPlay(), (o) => CanSendInviteToPlay()));
            }
        }
        private async Task<bool> SendInviteToPlay()
        {
            try
            {
                _timeInGame = dialogService.SelectTimeGame();
                var recepient = _selectedParticipant.Username;
                await chatService.SendInviteToPlayAsync(recepient, _timeInGame);
                return true;
            }
            catch (Exception) { return false; }
        }

        private bool CanSendInviteToPlay()
        {
            return (IsConnected && _selectedParticipant != null && _selectedParticipant.IsLoggedIn && _selectedParticipant.IsInGame != true);
        }

        private async Task SendResponse(string name, object response)
        {
            try
            {
                await chatService.SendResponseAsync(name, response);
            }
            catch (Exception)
            {
                //to do
            }
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
                if (UserState == UserState.InGame)
                {
                    chatService.NotifyOpponentGameIsFinished(Table.InstanceGame.Opponent.Username);
                    chatService.NotifyAllAsync(Table.InstanceGame.Opponent.Username, false);
                }
                await chatService.LogoutAsync();
                UserState = UserState.Lobby;
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
                if (_textMessage.EndsWith("\n"))
                {
                    _textMessage = _textMessage.Remove(_textMessage.Count() - 2);
                    await SendTextMessage();
                }
                else
                    await chatService.TypingAsync(SelectedParticipant.Username);
                return true;
            }
            catch (Exception) { return false; }
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
            catch (Exception) { return false; }
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
                    IsInGame = u.isInGame
                })).Wait();
            }
        }

        private void ParticipantDisconnection(string name)
        {
            var person = Participants.Where((p) => string.Equals(p.Username, name)).FirstOrDefault();
            if (person != null)
                person.IsLoggedIn = false;
        }

        private void ParticipantReconnection(string name)
        {
            var person = Participants.Where((p) => string.Equals(p.Username, name)).FirstOrDefault();
            if (person != null)
                person.IsLoggedIn = true;
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
            await connectionTask.ContinueWith(t =>
            {
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
        /// <summary>
        /// Game
        /// </summary>
        private ICommand _loadedTableCommand;
        public ICommand LoadedTableCommand
        {
            get
            {
                return _loadedTableCommand ?? (_loadedTableCommand = new Command((o) => InitTable()));
            }
        }
        private void InitTable()
        {
            Table.InstanceGame.Player.StpWatch.Time = _timeInGame;
            Table.InstanceGame.Opponent.StpWatch.Time = _timeInGame;
            Table.ArrMoves.CollectionChanged -= CollectionWasChanged;
            Table.ArrMoves.CollectionChanged += CollectionWasChanged;
        }

        private string titleTurn = "It's white turn";
        public string TitleTurn
        {
            get => titleTurn;
            set
            {
                titleTurn = value;
                OnPropertyChanged();
            }
        }

        private async void InviteToPlay(string name, int time)
        {
            //there was a problem with owner of the current application
            bool result = false;
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                result = dialogService.ShowConfirmationRequest($"Player {name} wanna play with you", "", true);
            }));

            //start game
            if (true == result)
            {
                _timeInGame = time;
                UserState = UserState.InGame;
                Table.InstanceGame = new InstanceGame(new Player(_userName) { IsWhite = true, IsBlack = false },
                                                        new Player(name) { IsBlack = true, IsWhite = false });
                await chatService.NotifyAllAsync(name, true);

            }

            //there was also a problem "How to send message to the Hub??? because signalR can't has return type only void or Task"
            //send the result to the server
            await SendResponse(name, result);
        }

        private void GetResponse(string name, object response)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (Convert.ToBoolean(response) == false)
                    dialogService.ShowNotification($"Player {name} does not want to play");

                else if (Convert.ToBoolean(response) == true)
                {
                    //can't wait because timer was started
                    //dialogService.ShowNotification($"Player {name} accepted request", "", true);

                    UserState = UserState.InGame;
                    chatService.NotifyAllAsync(name, true);
                    Table.InstanceGame = new InstanceGame(new Player(_userName) { IsBlack = true, IsWhite = false },
                                                            new Player(name) { IsWhite = true, IsBlack = false });
                }
            }));
        }

        private void NotifyIsInGame(string name, bool isInGame)
        {
            var user = Participants.Where((p) => string.Equals(p.Username, name)).FirstOrDefault();
            if (user != null)
                user.IsInGame = isInGame;
        }

        private void CollectionWasChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ObservableCollection<Moves> collection = sender as ObservableCollection<Moves>;

            SendMove(collection.Last().FromSquare.Row,
                collection.Last().FromSquare.Column,
                collection.Last().ToSquare.Row,
                collection.Last().ToSquare.Column,
                collection.Last().PieceWasChanged);
        }

        private void SendMove(int FromRow, int FromColumn, int ToRow, int ToColumn, string pieceWasChanged)
        {
            try
            {
                var app = Application.Current.MainWindow.Content;
                Application.Current.Dispatcher.Invoke(new Action(async () => 
                {
                    var recepient = Table.InstanceGame.Opponent.Username;
                    await chatService.SendMoveAsync(recepient, FromRow, FromColumn, ToRow, ToColumn, Table.InstanceGame.IsFinishGame, pieceWasChanged);
                    Table.InstanceGame.Player.StpWatch.PauseTimer();
                    Table.InstanceGame.Opponent.StpWatch.StartTimer();
                    if (Table.InstanceGame.IsFinishGame == true)
                    {
                        dialogService.ShowNotification("You win", "", true);
                        chatService.NotifyAllAsync(recepient, false);
                        UserState = UserState.Chat;
                        return;
                    }
                    else if (Table.InstanceGame.IsFinishGame == false)
                    {
                        dialogService.ShowNotification("You lost", "", true);
                        chatService.NotifyAllAsync(recepient, false);
                        UserState = UserState.Chat;
                        return;
                    }
                }));
            }
            catch (Exception)
            {
                //TO DO
            }
        }

        private void ReceiveMove(string name, int fromRow, int fromColumn, int toRow, int toColumn, bool? isFinihed, string pieceWasChanged)
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                Table.InstanceGame.Opponent.StpWatch.PauseTimer();
                //opponent player won
                if (isFinihed == true)
                {
                    dialogService.ShowNotification("You lost", "", true);
                    //back to chat
                    UserState = UserState.Chat;
                    //notify all player game is finish
                    chatService.NotifyAllAsync(name, false);
                    return;
                }
                //opponenet player lose
                else if (isFinihed == false)
                {
                    dialogService.ShowNotification("You win", "", true);
                    //back to chat
                    UserState = UserState.Chat;
                    //notify all player game is finish
                    chatService.NotifyAllAsync(name, false);
                    return;
                }

                Table.ArrOponentMoves.Add(new Moves(fromRow, fromColumn, toRow, toColumn) { PieceWasChanged = pieceWasChanged});  
                Table.InstanceGame.Player.StpWatch.StartTimer();
            }));
        }

        private void GameIsFinished()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                chatService.NotifyAllAsync(Table.InstanceGame.Player.Username, false);
                dialogService.ShowNotification("You win", "Participant disconnected", true);
                UserState = UserState.Chat;
            }));
        }

        private void PlayerSurrender()
        {
            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                chatService.NotifyAllAsync(Table.InstanceGame.Player.Username, false);
                dialogService.ShowNotification("You win", "Participant surrender", true);
                UserState = UserState.Chat;
            }));
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
            chatSvc.InviteToPlay += InviteToPlay;
            chatSvc.GetResponse += GetResponse;
            chatSvc.ReceiveMove += ReceiveMove;
            chatSvc.NotifyIsInGame += NotifyIsInGame;
            chatSvc.ParticipantDisconnectedWinGame += GameIsFinished;
            chatSvc.ParticipantSurrended += PlayerSurrender;

            ctxTaskFactory = new TaskFactory(TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}