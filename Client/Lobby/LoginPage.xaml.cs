using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Chess.Lobby
{
    public partial class LoginPage : UserControl
    {
        public event EventHandler eventSuccesLogin;
        public LoginPage()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            var pass = HashPassword.ComputeSha256Hash(txbPassword.Password);
            var client = new UserService.UserServiceClient();
            var user = client.GetUser(txbUsername.Text);
            if (null == user)
            {
                MessageBox.Show("The username you've entered is incorrect !");
                return;
            }
            if (!user.Password.Equals(pass))
            {
                MessageBox.Show("The password you've entered is incorrect !");
                return;
            }
            if (user.IsLoggedIn == true)
            {
                MessageBox.Show("The user is already logged in !");
                return;
            }

            MessageBox.Show("Login succesfuly");
            //keep username to update list of players
            (App.Current as Chess.App).CurrentUser = user;
            user.IsLoggedIn = true;
            client.UpdateDataBase(user);
            eventSuccesLogin(this, e);
        }
    }
}
