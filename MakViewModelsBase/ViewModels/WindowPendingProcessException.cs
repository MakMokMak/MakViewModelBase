using System;
using System.Runtime.Serialization;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// ウィンドウを閉じようとした際にビューモデルが処理途中等で閉じることができない場合にスローされる例外
    /// </summary>
    [Serializable]
    public class WindowPendingProcessException : Exception
    {
        public WindowPendingProcessException() : base() { }
        public WindowPendingProcessException(string message) : base(message) { }
        public WindowPendingProcessException(string message, Exception inner) : base(message, inner) { }
        public WindowPendingProcessException(SerializationInfo info, StreamingContext context) { }
    }
}
