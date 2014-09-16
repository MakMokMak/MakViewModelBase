using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

using MakCraft.Behaviors.Interfaces;
using MakCraft.ViewModels;

using TransitionTestApp.ViewModels.Container;

namespace TransitionTestApp.ViewModels
{
    class TransitionWindow1ViewModel : TransitionViewModelBase
    {
        public TransitionWindow1ViewModel()
        {
        }

        private string _trans1Text;
        [Required(ErrorMessage = "この項目は必須項目です。")]
        public string Trans1Text
        {
            get { return _trans1Text; }
            set
            {
                _trans1Text = value;
                base.RaisePropertyChangedWithRemoveItemValidationError(() => Trans1Text);
            }
        }

        private object _modelessKick;
        /// <summary>
        /// モードレスダイアログの表示のキック用
        /// </summary>
        public object ModelessKick
        {
            get { return _modelessKick; }
            set
            {
                _modelessKick = value;
                base.RaisePropertyChanged(() => ModelessKick);
            }
        }

        private void showTransitionWindow2Execute()
        {
            base.DialogType = typeof(TransitionWindow2);
            // コンテナに前画面のビューモデルを記録
            ((TransitionContainer)base.Container).PreviousViewModel = this;
            ((TransitionContainer)base.Container).Transition1Text = _trans1Text;
            base.CommunicationDialog = base.Container;
            ModelessKick = new object();
            base.DisplayMode = WindowAction.Hide;
        }
        private bool showTransitionWindow2CanExecute(object param)
        {
            return base.IsValid;
        }
        private ICommand _showTransitionWindow2Command;
        public ICommand ShowTransitionWindow2Command
        {
            get
            {
                if (_showTransitionWindow2Command == null)
                    _showTransitionWindow2Command = new RelayCommand(showTransitionWindow2Execute, showTransitionWindow2CanExecute);
                return _showTransitionWindow2Command;
            }
        }

        private void gcExecute()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        private ICommand _gcCommand;
        public ICommand GcCoomand
        {
            get
            {
                if (_gcCommand == null)
                    _gcCommand = new RelayCommand(gcExecute);
                return _gcCommand;
            }
        }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** TransitionWindowViewModel1 disposed.");
            base.Dispose(disposing);
        }
    }
}
