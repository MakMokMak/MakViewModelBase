using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.ModelBinding;

namespace MakCraft.ViewModels.Validations
{
    /// <summary>
    /// データ検証に用いるディクショナリ。
    /// </summary>
    class ValidationDictionary : IValidationDictionary
    {
        private readonly Dictionary<string, ModelState> _innerDic = new Dictionary<string, ModelState>(StringComparer.OrdinalIgnoreCase);

        public ValidationDictionary()
        {
        }

        private ModelState getModelStateForKey(string key)
        {
            if (key == null)
            {
                throw new ArgumentException("key");
            }

            ModelState modelState;
            if (!TryGetValue(key, out modelState))
            {
                modelState = new ModelState();
                this[key] = modelState;
            }

            return modelState;
        }

        #region IvalidationDictionary Members

        /// <summary>
        /// データ検証エラーの発生の有無を取得する。
        /// </summary>
        public bool IsValid
        {
            get { return Values.All(modelState => modelState.Errors.Count == 0); }
        }

        /// <summary>
        /// データ検証エラーメッセージを追加する。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="errorMessage"></param>
        public void AddError(string key, string errorMessage)
        {
            getModelStateForKey(key).Errors.Add(errorMessage);
        }

        /// <summary>
        /// propertyName に設定されているエラーメッセージを削除します。
        /// </summary>
        /// <param name="propertyName"></param>
        public void RemoveErrorByKey(string propertyName)
        {
            if (_innerDic.ContainsKey(propertyName))
            {
                var errorCollection = this[propertyName];
                errorCollection.Errors.Clear();
                Remove(propertyName);
            }
        }

        /// <summary>
        /// propertyName に対するエラーメッセージを返します。エラーがない場合は null を返します。
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string GetValidationError(string propertyName)
        {
            if (!ContainsKey(propertyName))
            {
                return null;
            }
            return this[propertyName].Errors.First().ErrorMessage;
        }

        #endregion

        #region IDictionary Members

        public void Add(string key, ModelState value)
        {
            _innerDic.Add(key, value);
        }

        public bool ContainsKey(string key)
        {
            return _innerDic.ContainsKey(key);
        }

        public ICollection<string> Keys
        {
            get { return _innerDic.Keys; }
        }

        public bool Remove(string key)
        {
            return _innerDic.Remove(key);
        }

        public bool TryGetValue(string key, out ModelState value)
        {
            return _innerDic.TryGetValue(key, out value);
        }

        public ICollection<ModelState> Values
        {
            get { return _innerDic.Values; }
        }

        public ModelState this[string key]
        {
            get
            {
                ModelState value;
                _innerDic.TryGetValue(key, out value);
                return value;
            }
            set
            {
                _innerDic[key] = value;
            }
        }

        public void Add(KeyValuePair<string, ModelState> item)
        {
            (_innerDic as IDictionary<string, ModelState>).Add(item);
        }

        public void Clear()
        {
            _innerDic.Clear();
        }

        public bool Contains(KeyValuePair<string, ModelState> item)
        {
            return (_innerDic as IDictionary<string, ModelState>).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, ModelState>[] array, int arrayIndex)
        {
            (_innerDic as IDictionary<string, ModelState>).CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return _innerDic.Count; }
        }

        public bool IsReadOnly
        {
            get { return (_innerDic as IDictionary<string, ModelState>).IsReadOnly; }
        }

        public bool Remove(KeyValuePair<string, ModelState> item)
        {
            return (_innerDic as IDictionary<string, ModelState>).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, ModelState>> GetEnumerator()
        {
            return _innerDic.GetEnumerator();
        }

        #endregion IDictionary

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return (_innerDic as IEnumerable).GetEnumerator();
        }

        #endregion IEnumerable
    }
}
