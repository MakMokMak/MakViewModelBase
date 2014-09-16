using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading;
using System.Windows.Threading;

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
        public NotifyObject()
        {
            // インスタンスが作られたときの Diapatcher を取得
            _uiDispatcher = Dispatcher.CurrentDispatcher;
        }

        /// <summary>
        /// PropertyChanged イベント処理用のデリゲート
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;


        private readonly Dispatcher _uiDispatcher;
        /// <summary>
        /// UI スレッドのディスパッチャ
        /// </summary>
        protected Dispatcher UiDispatcher
        {
            get { return _uiDispatcher; }
        }

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
                if (isUiThread())
                {   // UI スレッドなら、そのまま実行
                    handler(this, new PropertyChangedEventArgs(propertyName));
                }
                else
                {   // UI スレッドじゃなかったら、Dispatcher に依頼
                    UiDispatcher.Invoke(new Action(() => RaisePropertyChanged(propertyName)));
                }
            }
        }

        // UI スレッドからのアクセスかどうかを判定する
        private bool isUiThread()
        {
            // 今のスレッドと UI スレッドを比較
            return UiDispatcher.Thread == Thread.CurrentThread;
        }
    }
}
