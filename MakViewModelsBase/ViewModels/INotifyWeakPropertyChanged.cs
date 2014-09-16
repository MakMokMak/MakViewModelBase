using System.ComponentModel;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// 弱いイベントパターンを用いたプロパティ変更通知のイベントハンドラを持ちます。
    /// </summary>
    public interface INotifyWeakPropertyChanged
    {
        /// <summary>
        /// 弱いイベントパターンを用いたプロパティ変更通知のイベントハンドラ
        /// </summary>
        event PropertyChangedEventHandler WeakPropertyChanged;
    }
}
