using System.Windows;

namespace MakCraft.Behaviors.Interfaces
{
    /// <summary>
    /// MessageDialogAction へ渡すパラメーターのインターフェイス
    /// IsDialog が false のときには Button の設定は反映されません。
    /// </summary>
    public interface IMessageDialogActionParameter
    {
        /// <summary>
        /// MessageBoxに表示するメッセージ
        /// </summary>
        string Message { get; }
        /// <summary>
        /// MessageBox に表示するタイトル
        /// </summary>
        string Caption { get; }
        /// <summary>
        /// MessageBox に表示するボタン
        /// </summary>
        MessageBoxButton Button { get; }
        /// <summary>
        /// true:ダイアログ(ユーザ応答を処理する)、false:メッセージ
        /// </summary>
        bool IsDialog { get; }
    }
}
