using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using MakCraft.ViewModels;

namespace ViewModelBaseTestApp
{
    public enum CommandParam
    {
        ParamA,
        ParamB,
        ParamC
    }

    class MainWindowViewModel : ViewModelBase
    {
        private string _itemX;
        private string _itemY;
        private string _paramZ;
        private string _output;

        public MainWindowViewModel() { }

        public string ItemX
        {
            get { return _itemX; }
            set
            {
                _itemX = value;
                base.RaisePropertyChanged(PropertyHelper.GetName(() => ItemX));
            }
        }

        public string ItemY
        {
            get { return _itemY; }
            set
            {
                _itemY = value;
                base.RaisePropertyChanged(() => ItemY);
            }
        }

        public string Output
        {
            get { return _output; }
            private set
            {
                _output = value;
                base.RaisePropertyChanged(() => Output);
            }
        }

        private void outputCommandExecute()
        {
            Output = string.Format("\'{0}\' was \'{1}\'.", ItemX, ItemY);

        }
        private bool outputCommandCanExecute
        {
            get { return (!string.IsNullOrEmpty(ItemX) && !string.IsNullOrEmpty(ItemY)); }
        }
        private ICommand outputCommandOld;
        public ICommand OutputCommandOld
        {
            get
            {
                if (outputCommandOld == null)
                    outputCommandOld = new RelayCommand(
                        param => outputCommandExecute(),
                        param => outputCommandCanExecute
                        );
                return outputCommandOld;
            }
        }
        private ICommand outputCommand;
        public ICommand OutputCommand
        {
            get
            {
                if (outputCommand == null)
                    outputCommand = new RelayCommand(
                        outputCommandExecute,
                        param => outputCommandCanExecute
                        );
                return outputCommand;
            }
        }

        private void outputWithParamExecute(CommandParam param)
        {
            Output = string.Format("\'{0}\' was \'{1}\'. parameter: {2}", ItemX, ItemY, param);
        }
        private bool outputWithParamCanExecute(object param)
        {
            return (!string.IsNullOrEmpty(ItemX) && !string.IsNullOrEmpty(ItemY));
        }
        private RelayCommand<CommandParam> _outputWithParamCommand;
        public ICommand OutputWithParamCommand
        {
            get
            {
                if (_outputWithParamCommand == null)
                    _outputWithParamCommand = new RelayCommand<CommandParam>(
                        outputWithParamExecute, outputWithParamCanExecute);
                return _outputWithParamCommand;
            }
        }
    }
}
