using System.Collections.Generic;

using ValidationTestApp.Models;

namespace ValidationTestApp.DAL
{
    public interface IMemosRepository
    {
        IList<Memo> Find();
        Memo Get(int id);
        Memo Add(Memo memo);
        void Update(Memo memo);
        void Delete(int id);
    }
}
