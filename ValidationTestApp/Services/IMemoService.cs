using System.Collections.Generic;

using ValidationTestApp.Models;

namespace ValidationTestApp.Services
{
    public interface IMemoService
    {
        IList<Memo> GetMemos();
        Memo GetMemo(int id);
        Memo AddMemo(Memo memo);
        void UpdateMemo(Memo memo);
        void DeleteMemo(int id);

        void CheckTitleNote(string propatyName, string note, string title);
    }
}
