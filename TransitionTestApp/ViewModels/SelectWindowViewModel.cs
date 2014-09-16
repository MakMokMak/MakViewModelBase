using System.Collections.Generic;
using System.Windows.Input;

using MakCraft.ViewModels;

using TransitionTestApp.ViewModels.Container;

namespace TransitionTestApp.ViewModels
{
    class SelectWindowViewModel : TransitionViewModelBase
    {
        public SelectWindowViewModel()
        {
            Items = initList();
        }

        private List<string> _items;
        public List<string> Items
        {
            get { return _items; }
            set
            {
                _items = value;
                base.RaisePropertyChanged(() => Items);
            }
        }

        private string _selectedItem;
        public string SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                base.RaisePropertyChanged(() => SelectedItem);
            }
        }

        protected override void TransitionComplete()
        {
            ((SelectContainer)base.Container).ItemName = SelectedItem;

            base.TransitionComplete();
        }

        private void completeExecute()
        {
            TransitionComplete();
        }
        private bool completeCanExecute(object param)
        {
            return !string.IsNullOrEmpty(_selectedItem);
        }
        private ICommand _completeCommand;
        public ICommand CompleteCommand
        {
            get
            {
                if (_completeCommand == null)
                    _completeCommand = new RelayCommand(completeExecute, completeCanExecute);
                return _completeCommand;
            }
        }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** SelectWindowViewModel disposed.");
            base.Dispose(disposing);
        }

        private List<string> initList()
        {
            return new List<string> {
                "りんご", "柿", "ぶどう", "みかん"
            };
        }
    }
}
