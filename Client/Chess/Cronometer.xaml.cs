using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Threading;

namespace Chess
{
    public partial class Cronometer : UserControl
    {
        public event EventHandler timeElapsed;
        private int time = 10;
        private DispatcherTimer Timer;

        public int Time { get => time;}

        public Cronometer()
        {
            InitializeComponent();
            time = 10;
            Timer = new DispatcherTimer();
            Timer.Interval = new TimeSpan(0, 0, 1);
            Timer.Tick -= TimerRunning;
            Timer.Tick += TimerRunning;
            clocktxt.Text = string.Format("00:0{0}:{1}", time / 60, time % 60);
        }

        private void TimerRunning(object sender, EventArgs e)
        {
            if (time > 0)
            {
                time--;
                clocktxt.Text = string.Format("00:0{0}:{1}", time / 60, time % 60);
                if (time < 60)
                    clocktxt.Foreground = Brushes.Red;
            }
            else
            {
                timeElapsed?.Invoke(this, e);
                Timer.Stop();
            }
        }

        public void StartTimer()
        {
            Timer.Start();
        }

        public void PauseTimer()
        {
            Timer.Stop();
        }
    }
}
