using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Chess
{
    public class MainWindowDataContex : GeneralDataContex
    {
        private bool _isVisible = false;

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value; OnPropertyChanged("IsVisible");
            }
        }

        public MainWindowDataContex()
        {
            Timer timer = new Timer(5000);
            timer.Elapsed += timer_Elapsed;
            timer.Start();
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            IsVisible = !IsVisible;
        }
    }
}
