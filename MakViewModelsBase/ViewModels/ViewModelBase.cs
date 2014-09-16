using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
