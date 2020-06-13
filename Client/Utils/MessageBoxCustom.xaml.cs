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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Client.Utils
{
    /// <summary>
    /// Interaction logic for MessageBoxCustom.xaml
    /// </summary>
    /// <summary>
    /// Interaction logic for MessageBoxCustom.xaml
    /// </summary>
    public partial class MessageBoxCustom : Window
    {
        private DispatcherTimer dispatcherTimer = new DispatcherTimer();
        public MessageBoxCustom(string Message, MessageTypeBox Type, MessageButtons Buttons, bool autoClose = false)
        {
            InitializeComponent();
            
            //auto close window after x seconds
            if (autoClose == true)
            {
                dispatcherTimer.Tick -= dispatcherTimer_Tick;
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
                dispatcherTimer.Start();
            }

            txtMessage.Text = Message;
            switch (Type)
            {

                case MessageTypeBox.Info:
                    txtTitle.Text = "Info";
                    break;
                case MessageTypeBox.Confirmation:
                    txtTitle.Text = "Confirmation";
                    break;
                case MessageTypeBox.Success:
                    {
                        txtTitle.Text = "Success";
                    }
                    break;
                case MessageTypeBox.Warning:
                    txtTitle.Text = "Warning";
                    break;
                case MessageTypeBox.Error:
                    {
                        txtTitle.Text = "Error";
                    }
                    break;
            }
            switch (Buttons)
            {
                case MessageButtons.OkCancel:
                    btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.YesNo:
                    btnOk.Visibility = Visibility.Collapsed; btnCancel.Visibility = Visibility.Collapsed;
                    break;
                case MessageButtons.Ok:
                    btnOk.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnYes.Visibility = Visibility.Collapsed; btnNo.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //this.DialogResult = false;
            dispatcherTimer.Stop();
            this.Close();
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
    public enum MessageTypeBox
    {
        Info,
        Confirmation,
        Success,
        Warning,
        Error,
    }
    public enum MessageButtons
    {
        OkCancel,
        YesNo,
        Ok,
    }
}
