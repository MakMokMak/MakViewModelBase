using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using MakCraft.ViewModels.Validations;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// データ検証を実装したビューモデルの基底クラス。
    /// </summary>
    public abstract class ValidationViewModelBase : WeakEventViewModelBase, IDataErrorInfo
    {
        private IValidationDictionary _validationDic;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public ValidationViewModelBase()
        {
            _validationDic = new ValidationDictionary();
        }

        /// <summary>
        /// データ検証エラーの発生の有無を取得します。
        /// </summary>
        public bool IsValid
        {
            get { return _validationDic.IsValid; }
        }

        /// <summary>
        /// propertyName に設定されている検証エラーメッセージを削除します。
        /// </summary>
        /// <param name="propertyName"></param>
        public void RemoveItemValidationError<T>(Expression<Func<T>> propertyName)
        {
            RemoveItemValidationError(PropertyHelper.GetName(propertyName));
        }
        /// <summary>
        /// propertyName に設定されている検証エラーメッセージを削除します。
        /// propertyName が省略された場合、呼び出し元のメソッドまたはプロパティの名前を用います。
        /// </summary>
        /// <param name="propertyName"></param>
        public void RemoveItemValidationError([CallerMemberName] string propertyName = null)
        {
            _validationDic.RemoveErrorByKey(propertyName);
        }

        /// <summary>
        /// 指定されたプロパティの System.ComponentModel.DataAnnotations のデータ検証アトリビュート検査の結果を確認します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>検証エラーが発生していれば true</returns>
        public bool IsPropertyAnnotationError<T>(Expression<Func<T>> propertyName)
        {
            return IsPropertyAnnotationError(PropertyHelper.GetName(propertyName));
        }
        /// <summary>
        /// 指定されたプロパティの System.ComponentModel.DataAnnotations のデータ検証アトリビュート検査の結果を確認します。
        /// propertyName が省略された場合、呼び出し元のメソッドまたはプロパティの名前を用います。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>検証エラーが発生していれば true</returns>
        public bool IsPropertyAnnotationError([CallerMemberName] string propertyName = null)
        {
            return (this[propertyName] != null);
        }

        /// <summary>
        /// ビューモデルの状態及びバインディングの検証の状態を格納するビューモデル状態ディクショナリ オブジェクトを取得します。
        /// </summary>
        public IValidationDictionary ViewModelState
        {
            get { return _validationDic; }
        }

        /// <summary>
        /// PropertyChanged イベントを発火します。
        /// プロパティ変更通知まえに当該プロパティの検証エラーの削除を行います。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        protected void RaisePropertyChangedWithRemoveItemValidationError<T>(Expression<Func<T>> e)
        {
            RaisePropertyChangedWithRemoveItemValidationError(PropertyHelper.GetName(e));
        }

        /// <summary>
        /// PropertyChanged イベントを発火します。
        /// プロパティ変更通知まえに当該プロパティの検証エラーの削除を行います。
        /// propertyName が省略された場合、呼び出し元のプロパティの名前を用います。
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void RaisePropertyChangedWithRemoveItemValidationError([CallerMemberName] string propertyName = null)
        {
            // プロパティ変更通知まえに検証エラーを削除しておく
            RemoveItemValidationError(propertyName);

            base.RaisePropertyChanged(propertyName);
        }

        /// <summary>
        /// プロパティ名 property を value の値で書き換え、PropertyChanged イベントを発火します。
        /// プロパティ変更通知まえに当該プロパティの検証エラーの削除を行います。
        /// propertyName が省略された場合、呼び出し元のプロパティの名前を用います。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property"></param>
        /// <param name="value"></param>
        /// <param name="propertyName"></param>
        protected void SetPropertyWithRemoveItemValidationError<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            // プロパティ変更通知まえに検証エラーを削除しておく
            RemoveItemValidationError(propertyName);

            base.SetProperty(ref property, value, propertyName);
        }

        #region IDataErrorInfo Members

        /// <summary>
        /// オブジェクトに関する間違いを示すエラー メッセージを取得します。
        /// </summary>
        public string Error
        {
            get
            {
                if (IsValid) return string.Empty;
                // インスタンスが持つオブジェクト検証の全結果を連結して返す
                var results = new List<string>();
                foreach (var n in _validationDic)
                {
                    var propertyName = n.Key;
                    results.Add(_validationDic.GetValidationError(propertyName));

                }
                return string.Join(Environment.NewLine, results.Select(n => n));
            }
        }


        /// <summary>
        /// 指定した名前のプロパティに関するエラー メッセージを取得します。
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        public string this[string columnName]
        {
            get
            {
                // System.ComponentModel.DataAnnotations のデータ検証アトリビュートを利用したデータ検証
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateProperty(
                    GetType().GetProperty(columnName).GetValue(this, null),
                    new ValidationContext(this, null, null) { MemberName = columnName },
                    results))
                {
                    results.ForEach(n => _validationDic.AddError(columnName, n.ErrorMessage));
                }
                // データ検証を行う条件を確認して条件が成立しなかったら検証エラーを削除する
                conditionalValidate(columnName);
                // ビューモデル状態ディクショナリからエラーメッセージを返す。
                RaisePropertyChanged(PropertyHelper.GetName(() => Error));
                return _validationDic.GetValidationError(columnName);
            }
        }

        // データ検証を行う条件を確認して条件が成立しなかったら検証エラーを削除する
        private void conditionalValidate(string columnName)
        {
            var attrib = Attribute.GetCustomAttribute(this.GetType().GetProperty(columnName),
                typeof(ValidateConditionalAttribute));
            if (attrib != null)
            {
                var conditional = attrib as ValidateConditionalAttribute;
                var targetAttrib = this.GetType().GetProperty(conditional.ComparedProperty);
                if (targetAttrib == null)
                {
                    var message = string.Format(
                        "データ検証を行う比較対象として指定されたプロパティが見つかりませんでした(検証プロパティ名: {0}, 比較対象プロパティ名: {1})。",
                        columnName, conditional.ComparedProperty);
                    throw new MissingMemberException(message);
                }
                var target = targetAttrib.GetValue(this);
                bool condition = false;
                if (target == null)
                {
                    // 比較対象プロパティが null なので、条件値が null であるか比較する
                    if (conditional.Value == null)
                    {
                        condition = true;
                    }
                }
                else
                {
                    // 比較対象プロパティの動的な型が持つ Equals メソッドを呼び出して条件値と比較する
                    // (動的型付け変数を利用することで毎回リフレクションを呼び出すコストを回避)
                    dynamic equalsObj = target;
                    condition = (bool)equalsObj.Equals(conditional.Value);
                }
                if (!condition)
                {
                    // 条件が成立しないので検証エラーを削除
                    RemoveItemValidationError(columnName);
                }
            }
        }

        #endregion IDataErrorInfo
    }
}
