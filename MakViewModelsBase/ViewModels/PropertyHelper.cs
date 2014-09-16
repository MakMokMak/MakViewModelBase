using System;
using System.Linq.Expressions;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// PropertyChanged イベント通知のヘルパークラスです。
    /// </summary>
    public static class PropertyHelper
    {
        // http://blogs.msdn.com/b/csharpfaq/archive/2010/03/11/how-can-i-get-objects-and-property-values-from-expression-trees.aspx
        /// <summary>
        /// 引数で渡されたプロパティから当該プロパティの名前を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetName<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;
            return member.Member.Name;
        }
    }
}
