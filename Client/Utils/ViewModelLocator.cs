using Client.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using Client.ViewModels;

namespace Client.Utils
{
    public class ViewModelLocator
    {
        private UnityContainer container;

        public ViewModelLocator()
        {
            container = new UnityContainer();
            container.RegisterType<IClientService, ClientService>();
            container.RegisterType<IMessageErrorService, MessageErrorService>();
        }

        public MainViewModel MainVM
        {
            get { return container.Resolve<MainViewModel>(); }
        }
    }
}
