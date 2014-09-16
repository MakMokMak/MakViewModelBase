using System.Collections.Generic;

using MakCraft.ViewModels;

namespace TransitionTestApp.ViewModels
{
    class ModalWindow2ViewModel : ModalViewModelBase
    {
        public ModalWindow2ViewModel()
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
        public Book SelectedBook
        {
            get { return _selectedBook; }
            set
            {
                _selectedBook = value;
                base.RaisePropertyChanged(() => SelectedBook);
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

        protected override bool OkCanExecute(object param)
        {
            return (SelectedBook != null);
        }

        protected override void Dispose(bool disposing)
        {
            System.Diagnostics.Debug.WriteLine("** ModalWindow2ViewModel disposed.");
            base.Dispose(disposing);
        }
    }
}
