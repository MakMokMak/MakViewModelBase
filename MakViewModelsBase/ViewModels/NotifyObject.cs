using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// プロパティ変更通知を実装した基底クラス
    /// </summary>
    public abstract class NotifyObject : INotifyPropertyChanged
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NotifyObject() {}

        /// <summary>
        /// PropertyChanged イベント処理用のデリゲート
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// PropertyChanged イベントを発火します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        protected void RaisePropertyChanged<T>(Expression<Func<T>> e)
        {
            RaisePropertyChanged(PropertyHelper.GetName(e));
        }

        /// <summary>
        /// PropertyChanged イベントを発火します。
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
