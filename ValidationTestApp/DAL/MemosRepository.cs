using System.Collections.Generic;
using System.Linq;

using ValidationTestApp.Models;

namespace ValidationTestApp.DAL
{
    public class MemosRepository : IMemosRepository
    {
        private List<Memo> _db;
        private int _nextId;

        public MemosRepository()
        {
            _db = new List<Memo>();
            _nextId = 0;
        }

        public IList<Memo> Find()
        {
            return _db.ToList();
        }

        public Memo Get(int id)
        {
            return _db.Find(w => w.MemoId == id);
        }

        public Memo Add(Memo memo)
        {
            memo.MemoId = _nextId++;
            _db.Add(memo);
            return memo;
        }

        public void Update(Memo memo)
        {
            var dbMemo = Get(memo.MemoId);
            dbMemo.Title = memo.Title;
            dbMemo.Note = memo.Note;
            dbMemo.Age = memo.Age;
            dbMemo.Remark = memo.Remark;
            dbMemo.Remark2 = memo.Remark2;
        }

        public void Delete(int id)
        {
            var dbMemo = Get(id);
            _db.Remove(dbMemo);
        }
    }
}
