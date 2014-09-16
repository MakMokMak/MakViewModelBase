using System;
using System.ComponentModel;
using System.Diagnostics;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// 弱いイベントパターンのリスナです。
    /// </summary>
    public class PropertyChangedWeakEventListener : IPropertyChangedWeakEventListener
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PropertyChangedWeakEventListener()
        {
            Trace.WriteLine("=== Create Listener ===");
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~PropertyChangedWeakEventListener()
        {
            Trace.WriteLine("=== Destruct Listener ===");
        }

        /// <summary>
        /// 弱いイベントパターンを用いたプロパティ変更通知のイベントハンドラ
        /// </summary>
        public event PropertyChangedEventHandler WeakPropertyChanged;

        /// <summary>
        /// イベント マネージャーからイベントを受信します。
        /// </summary>
        /// <param name="managerType"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            // PropertyChangedEventManager からのイベント通知であることを確認
            if (typeof(PropertyChangedEventManager) != managerType)
            {
                return false;
            }

            // PropertyChangedEventArgs であることを確認
            var eventArgs = e as PropertyChangedEventArgs;
            if (eventArgs == null)
            {
                return false;
            }

            // コールバックを呼び出す
            var handler = WeakPropertyChanged;
            if (handler != null)
            {
                handler(sender, eventArgs);
            }
            return true;
        }
    }
}
