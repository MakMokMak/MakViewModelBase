using System;
using System.Windows.Input;

using MakCraft.ViewModels;

namespace TransitionTestApp.ViewModels
{
    class MultipleWindowViewModel : TransitionViewModelBase
    {
        public MultipleWindowViewModel() { }

        private void changeClosableExecute()
        {
            _isClosable = !_isClosable;
        }
        private ICommand _changeClosableCommand;
        public ICommand ChangeClosableCommand
        {
            get
            {
                if (_changeClosableCommand == null)
                    _changeClosableCommand = new RelayCommand(changeClosableExecute);
                return _changeClosableCommand;
            }
        }

        protected override void Dispose(bool disposing)
        {
            Console.WriteLine("** MultipleWindowViewModel disposed.");
            base.Dispose(disposing);
        }

        private bool _isClosable = false;
        protected override bool WindowCloseCanExecute(object param)
        {
            return _isClosable;
        }
    }
}
