using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

using MakCraft.ViewModels;

using WeakEventViewModelBaseTestApp.Models;
using WeakEventViewModelBaseTestApp.Services;

namespace WeakEventViewModelBaseTestApp.ViewModels
{
    class MainWindowViewModel : WeakEventViewModelBase
    {
        private IBookService _service;
        private IPropertyChangedWeakEventListener _listener;

        public MainWindowViewModel() : this(new BookService(), new PropertyChangedWeakEventListener()) { }
        public MainWindowViewModel(IBookService service, IPropertyChangedWeakEventListener listener)
        {
            _service = service;
            _listener = listener;
            _listener.WeakPropertyChanged += onWeakListenerPropertyChanged;
            base.AddListener(_service, _listener);
        }

        public int BooksCount
        {
            get { return _service.Count; }
        }

        public List<Book> Books
        {
            get { return _service.Books; }
        }

        private void addBookCommandExecute()
        {
            var book = new Book
            {
                Id = 0,
                Title = "くまの本",
                Author = "森のくまさん",
                Price = 1600,
                Feature = "森のくまさんの生活を描いた自信作"
            };
            _service.AddBook(book);
        }
        private ICommand addBookCommand;
        public ICommand AddBookCommand
        {
            get
            {
                if (addBookCommand == null)
                    addBookCommand = new RelayCommand(addBookCommandExecute);
                return addBookCommand;
            }
        }

        private void onWeakListenerPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // nameof 演算子でプロパティの名前を得る例
            base.RaisePropertyChanged(nameof(BooksCount));
            // PropertyHelper.GetName メソッドでプロパティの名前を得る例
            base.RaisePropertyChanged(() => Books);
        }
    }
}
