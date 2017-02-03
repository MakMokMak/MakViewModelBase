using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MakCraft.ViewModels;

namespace MultiMonitorUtilTestApp.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private string _textPosition;
        public string TextPosition
        {
            get { return _textPosition; }
            set { base.SetProperty(ref _textPosition, value); }
        }

        private string _TextNotice;
        public string TextNotice
        {
            get { return _TextNotice; }
            set { base.SetProperty(ref _TextNotice, value); }
        }
    }
}
