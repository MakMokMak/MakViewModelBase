﻿using System;
using System.Windows.Input;

using MakCraft.ViewModels;
using MakCraft.Behaviors.Interfaces;

using TransitionTestApp.ViewModels.Container;

namespace TransitionTestApp.ViewModels
{
    class MainWindowViewModel : TransitionViewModelBase
    {
        public MainWindowViewModel() { }

        private string _trans1Text;
        public string Trans1Text
        {
            get { return _trans1Text; }
            set
            {
                _trans1Text = value;
                base.RaisePropertyChanged(() => Trans1Text);
            }
        }

        private string _trans2Text;
        public string Trans2Text
        {
            get { return _trans2Text; }
            set
            {
                _trans2Text = value;
                base.RaisePropertyChanged(() => Trans2Text);
            }
        }

        private string _trans3Text;
        public string Trans3Text
        {
            get { return _trans3Text; }
            set
            {
                _trans3Text = value;
                base.RaisePropertyChanged(() => Trans3Text);
            }
        }

        private string _selectedBook;
        public string SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                base.RaisePropertyChanged(() => SelectedBook);
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

        private object _modalKick;
        /// <summary>
        /// モーダルダイアログの表示のキック用
        /// </summary>
        public object ModalKick
        {
            get { return _modalKick; }
            set
            {
                _modalKick = value;
                base.RaisePropertyChanged(() => ModalKick);
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

        private void showTransitionWindowExecute()
        {
            Trans1Text = string.Empty;
            Trans2Text = string.Empty;
            Trans3Text = string.Empty;
            // 遷移を行う画面を表示
            base.DialogType = typeof(TransitionWindow1);
            var container = new TransitionContainer(transitionName.trans.ToString(), this);
            base.CommunicationDialog = container;
            ModelessKick = new object();
            base.DisplayMode = WindowAction.Hide;
        }
        private ICommand _showTransitionWindowCommand;
        public ICommand ShowTransitionWindowCommand
        {
            get
            {
                if (_showTransitionWindowCommand == null)
                    _showTransitionWindowCommand = new RelayCommand(showTransitionWindowExecute);
                return _showTransitionWindowCommand;
            }
        }

        private void showTransAfterSelectedExecute()
        {
            // 遷移を行う画面を表示
            // A -> B 完了 -> A を経由して C 表示のパターン
            // C の表示は OnFinished メソッド側で行っている
            base.DialogType = typeof(SelectWindow);
            var container = new SelectContainer(transitionName.select.ToString(), this);
            base.CommunicationDialog = container;
            ModelessKick = new object();
            base.DisplayMode = WindowAction.Hide;
        }
        private ICommand _showTransAfterSelectedCommand;
        public ICommand ShowTransAfterSelectedCommand
        {
            get
            {
                if (_showTransAfterSelectedCommand == null)
                    _showTransAfterSelectedCommand = new RelayCommand(showTransAfterSelectedExecute);
                return _showTransAfterSelectedCommand;
            }
        }

        private void showMultipleWindowExecute()
        {
            base.DialogType = typeof(MultipleWindow);
            base.CommunicationDialog = null;
            ModelessKick = new object();
        }
        private ICommand _showMultipleWindowCommand;
        public ICommand ShowMultipleWindowCommand
        {
            get
            {
                if (_showMultipleWindowCommand == null)
                    _showMultipleWindowCommand = new RelayCommand(showMultipleWindowExecute);
                return _showMultipleWindowCommand;
            }
        }

        private void showModalWindowExecute()
        {
            SelectedBook = string.Empty;
            base.DialogType = typeof(ModalWindow1);
            base.CommunicationDialog = null;
            base.DialogActionCallback = dialogResult =>
            {
                if (dialogResult.HasValue && dialogResult.Value)
                {
                    var viewModel = base.ResultViewModel as ModalWindow1ViewModel;
                    SelectedBook = viewModel.SelectedBook.Title;
                }
            };
            ModalKick = new object();
        }
        private ICommand _showModalWindowCommand;
        public ICommand ShowModalWindowCommand
        {
            get
            {
                if (_showModalWindowCommand == null)
                    _showModalWindowCommand = new RelayCommand(showModalWindowExecute);
                return _showModalWindowCommand;
            }
        }

        private void showModalWindow2Execute()
        {
            SelectedBook = string.Empty;
            base.DialogType = typeof(ModalWindow2);
            base.CommunicationDialog = null;
            base.DialogActionCallback = dialogResult =>
            {
                if (dialogResult.HasValue && dialogResult.Value)
                {
                    var viewModel = base.ResultViewModel as ModalWindow2ViewModel;
                    SelectedBook = viewModel.SelectedBook.Title;
                }
            };
            ModalKick = new object();
        }
        private ICommand _showModalWindow2Command;
        public ICommand ShowModalWindow2Command
        {
            get
            {
                if (_showModalWindow2Command == null)
                    _showModalWindow2Command = new RelayCommand(showModalWindow2Execute);
                return _showModalWindow2Command;
            }
        }

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
        // 画面遷移の一連の手順の完了時に行う処理
        public override void OnFinished(ITransContainer container)
        {
            switch ((transitionName)Enum.Parse(typeof(transitionName), container.Key))
            {
                case transitionName.trans:
                    var trans = container as TransitionContainer;
                    Trans1Text = trans.Transition1Text;
                    Trans2Text = trans.Transition2Text;
                    Trans3Text = trans.Transition3Text;
                    break;
                case transitionName.select:
                    // 遷移を行う画面を表示
                    base.DialogType = typeof(AfterSelectWindow);
                    var afterContainer = new SelectContainer(transitionName.after.ToString(), this);
                    afterContainer.ItemName = ((SelectContainer)container).ItemName;
                    base.CommunicationDialog = afterContainer;
                    ModelessKick = new object();
                    base.DisplayMode = WindowAction.Hide;
                    break;
                case transitionName.after:
                    break;
            }

            base.OnFinished(container);
        }

        private bool _isClosable = true;
        protected override bool WindowCloseCanExecute(object param)
        {
            return _isClosable;
        }

        public override void WindowClose()
        {
            try
            {
                ViewModelUtility.CloseViewModels(typeof(MultipleWindowViewModel));
            }
            catch (WindowPendingProcessException)
            {
                // MessageDialogAction ビヘイビアを使用してメッセージボックスを出す
                base.MessageDialogActionParam = new MessageDialogActionParameter("閉じることのできないウィンドウがあります。",
                                            "クローズできません");
                base.RaisePropertyChanged(() => MessageDialogActionParam);
                return;
            }

            base.WindowClose();
        }

        private enum transitionName
        {
            trans,
            select,
            after,
        }
    }
}