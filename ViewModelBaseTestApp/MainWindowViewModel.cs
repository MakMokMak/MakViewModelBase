using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
        private const int DEFAULT_TIME = 30;
        private const int INTERVAL_TIME = 1000;
        private int _countdownSecInt;
        private Timer _timer;
        private string _itemX;
        private string _itemY;
        private string _paramZ;
        private string _output;

        public MainWindowViewModel()
        {
            CountdownSec = DEFAULT_TIME.ToString();
            _countdownSecInt = DEFAULT_TIME;
            IsEditTime = true;
            _timer = new Timer(noticeTime, null, Timeout.Infinite, 0);
        }

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

        private string _countdownSec;
        public string CountdownSec
        {
            get { return _countdownSec; }
            set
            {
                _countdownSec = value;
                if (int.TryParse(value, out _countdownSecInt))
                {
                    DisplaySec = _countdownSecInt;
                }
                base.RaisePropertyChanged(() => CountdownSec);
            }
        }

        private bool _isEditTime;
        public bool IsEditTime
        {
            get { return _isEditTime; }
            set
            {
                _isEditTime = value;
                base.RaisePropertyChanged(() => IsEditTime);
                if (_isEditTime)
                {
                    StartButtonText = "Start";
                }
                else
                {
                    StartButtonText = "Stop";
                }
            }
        }

        private string _startButtonText;
        public string StartButtonText
        {
            get { return _startButtonText; }
            set
            {
                _startButtonText = value;
                base.RaisePropertyChanged(() => StartButtonText);
            }
        }

        private int _displaySec;
        public int DisplaySec
        {
            get { return _displaySec; }
            set
            {
                _displaySec = value;
                base.RaisePropertyChanged(() => DisplaySec);

                if (_displaySec <= 0)
                {
                    stopTimer();
                    CommandManager.InvalidateRequerySuggested();
                }
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

        private void countdownExecute()
        {
            if (IsEditTime)
            {
                IsEditTime = false;
                _timer.Change(INTERVAL_TIME, INTERVAL_TIME);
            }
            else
            {
                stopTimer();
            }
        }
        private bool countdownCanExecute(object param)
        {
            return DisplaySec != 0;
        }
        private ICommand _countdownCommand;
        public ICommand CountdownCommand
        {
            get
            {
                if (_countdownCommand == null)
                {
                    _countdownCommand = new RelayCommand(countdownExecute, countdownCanExecute);
                }
                return _countdownCommand;
            }
        }

        private void resetExecute()
        {
            DisplaySec = _countdownSecInt;
        }
        private ICommand _resetCommand;
        public ICommand ResetCommand
        {
            get
            {
                if (_resetCommand == null)
                {
                    _resetCommand = new RelayCommand(resetExecute);
                }
                return _resetCommand;
            }
        }

        private void noticeTime(object state)
        {
            --DisplaySec;
        }

        private void stopTimer()
        {
            _timer.Change(Timeout.Infinite, 0);
            IsEditTime = true;
        }

        protected override void Dispose(bool disposing)
        {
            _timer.Dispose();

            base.Dispose(disposing);
        }
    }
}
