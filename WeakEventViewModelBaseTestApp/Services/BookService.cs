using System.Collections.Generic;
using System.Linq;

using MakCraft.ViewModels;

using WeakEventViewModelBaseTestApp.DAL;
using WeakEventViewModelBaseTestApp.Models;

namespace WeakEventViewModelBaseTestApp.Services
{
    class BookService : NotifyObject, IBookService
    {
        private IBookRepository _repository;

        public BookService() : this(new BookRepository()) { }
        public BookService(IBookRepository repository)
        {
            _repository = repository;
        }

        public int Count
        {
            get { return _repository.Count; }
        }

        public List<Book> Books
        {
            get { return _repository.FindBooks().ToList(); }
        }

        public Book GetBook(int id)
        {
            return _repository.GetBook(id);
        }

        public void AddBook(Models.Book book)
        {
            _repository.Add(book);

            base.RaisePropertyChanged(() => Books);
        }

        public void UpdateBook(Models.Book book)
        {
            _repository.Update(book);

            base.RaisePropertyChanged(() => Books);
        }

        public void DeleteBook(int id)
        {
            _repository.Delete(id);

            base.RaisePropertyChanged(() => Books);
        }
    }
}
