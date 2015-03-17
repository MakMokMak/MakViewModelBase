using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// プロパティ変更通知を実装したビューモデルの基底クラス
    /// </summary>
    public abstract class ViewModelBase : NotifyObject, IDisposable
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public ViewModelBase() { }

        /// <summary>
        /// CommandManager.RequerySuggested イベントを強制的に発火させます(呼び出し元のスレッドが UI スレッドでない場合には、UI スレッドにて実行を行います)。
        /// </summary>
        protected void InvalidateRequerySuggested()
        {
            if (base.IsUiThread())
            {
                CommandManager.InvalidateRequerySuggested();
            }
            else
            {
                UiDispatcher.Invoke(CommandManager.InvalidateRequerySuggested);
            }
        }

        #region IDisposable

        private bool _disposed = false;

        /// <summary>
        /// リソースの開放を行います。
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// リソースの開放を行います。
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            _disposed = true;

            if (disposing)
            {
                // マネージ リソースの解放処理
            }
            // アンマネージ リソースの解放処理
        }

        #endregion

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ViewModelBase()
        {
            Dispose(false);
        }
    }
}
