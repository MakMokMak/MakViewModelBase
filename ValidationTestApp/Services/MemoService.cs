using System;
using System.Collections.Generic;

using MakCraft.ViewModels;

using ValidationTestApp.DAL;
using ValidationTestApp.Models;

namespace ValidationTestApp.Services
{
    public class MemoService : IMemoService
    {
        private IMemosRepository _repository;
        private IValidationDictionary _validationDic;

        public MemoService(IValidationDictionary validationDic) : this(validationDic, new MemosRepository()) { }
        public MemoService(IValidationDictionary validationDic, IMemosRepository repository)
        {
            _repository = repository;
            _validationDic = validationDic;
        }

        public IList<Memo> GetMemos()
        {
            return _repository.Find();
        }

        public Memo GetMemo(int id)
        {
            return _repository.Get(id);
        }

        public Memo AddMemo(Memo memo)
        {
            if (_validationDic.IsValid)
                return _repository.Add(memo);

            return memo;
        }

        public void UpdateMemo(Memo memo)
        {
            throw new NotImplementedException();
        }

        public void DeleteMemo(int id)
        {
            throw new NotImplementedException();
        }

        public void CheckTitleNote(string propatyName, string note, string title)
        {
            if (!string.IsNullOrEmpty(note) && !string.IsNullOrEmpty(title) && note.IndexOf(title) < 0)
                _validationDic.AddError(propatyName, "本文中にタイトルが記述されていません。");
        }
    }
}
