using System.Collections.Generic;
using System.Web.ModelBinding;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// サービス層とビューモデル層のデータ検証との間のインターフェイス
    /// </summary>
    public interface IValidationDictionary : IDictionary<string, ModelState>
    {
        /// <summary>
        /// データ検証エラーの発生の有無を取得する。
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// データ検証エラーメッセージを追加する。
        /// </summary>
        /// <param name="key">プロパティ名</param>
        /// <param name="errorMessage">エラーメッセージ</param>
        void AddError(string key, string errorMessage);

        /// <summary>
        /// propertyName に設定されているエラーメッセージを削除します。
        /// </summary>
        /// <param name="key"></param>
        void RemoveErrorByKey(string propertyName);

        /// <summary>
        /// propertyName に対するエラーメッセージを返します。エラーがない場合は null を返します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        string GetValidationError(string propertyName);
    }
}
