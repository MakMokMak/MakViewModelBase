using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

using MakCraft.ViewModels;

namespace TransitionTestApp.ViewModels
{
    class ModalWindow1ViewModel : ModalViewModelBase
    {
        public ModalWindow1ViewModel()
        {
            setup();
        }

        private List<Book> _books;
        public List<Book> Books
        {
            get { return _books; }
            set
            {
                _books = value;
                base.RaisePropertyChanged(() => Books);
            }
        }

        private Book _selectedBook;
        [Required(ErrorMessage = "選択してください。")]
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                base.RaisePropertyChangedWithRemoveItemValidationError(() => SelectedBook);
            }
        }

        private string _messageText;
        public string MessageText
        {
            get { return _messageText; }
            set
            {
                _messageText = value;
                base.RaisePropertyChanged(() => MessageText);
            }
        }

        private void messageExecute()
        {
            MessageText = string.Empty;
            // コールバックの処理を設定
            base.MessageDialogActionCallback = result =>
            {
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        MessageText = "Yes が選択されました。";
                        break;
                    case MessageBoxResult.No:
                        MessageText = "No が選択されました。";
                        break;
                }
            };
            // MessageDialogAction ビヘイビアを使用してメッセージボックスを出す
            base.MessageDialogActionParam = new MessageDialogActionParameter("クリックされました。",
                                        "メッセージ", MessageBoxButton.YesNo, true);
            base.RaisePropertyChanged(() => MessageDialogActionParam);
        }
        private ICommand _messageCommand;
        public ICommand MessageCommand
        {
            get
            {
                if (_messageCommand == null)
                    _messageCommand = new RelayCommand(messageExecute);
                return _messageCommand;
            }
        }

        private void message2Execute()
        {
            MessageText = string.Empty;
            // コールバックの処理を設定
            base.MessageDialogActionCallback = result =>
            {
                switch (result)
                {
                    case MessageBoxResult.OK:
                        MessageText = "OK が選択されました。";
                        break;
                    case MessageBoxResult.Cancel:
                        MessageText = "Cancel が選択されました。";
                        break;
                }
            };
            // MessageDialogAction ビヘイビアを使用してメッセージボックスを出す
            base.MessageDialogActionParam = new MessageDialogActionParameter("クリックされました。",
                                        "メッセージ", MessageBoxButton.OKCancel, true);
            base.RaisePropertyChanged(() => MessageDialogActionParam);
        }
        private ICommand _message2Command;
        public ICommand Message2Command
        {
            get
            {
                if (_message2Command == null)
                    _message2Command = new RelayCommand(message2Execute);
                return _message2Command;
            }
        }

        private void setup()
        {
            Books = new List<Book> {
                new Book { Title = "猫の本", Note = "著者: みけ\r\n出版社: 猫の出版社\r\n概要: 猫ねこネコ" },
                new Book { Title = "犬の本", Note = "著者: ぽち\r\n出版社: 犬の出版社\r\n概要: 犬いぬイヌ" },
                new Book { Title = "金魚の本", Note = "著者: きん\r\n出版社: 金魚の出版社\r\n概要: 金魚きんぎょキンギョ" },
                new Book { Title = "鳥の本", Note = "著者: ぴよ\r\n出版社: 鳥の出版社\r\n概要: 鳥とりトリ" },
            };
        }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** ModalWindow1ViewModel disposed.");
            base.Dispose(disposing);
        }
    }

    public class Book
    {
        public string Title { get; set; }
        public string Note { get; set; }
    }
}
