using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace MakCraft.ViewModels
{
    /// <summary>
    /// ViewModel 関連のユーティリティクラス
    /// </summary>
    public static class ViewModelUtility
    {
        /// <summary>
        /// MainWindow となっている Window の ViewModel を返します。
        /// </summary>
        /// <returns></returns>
        public static ViewModelBase GetMainWindowViewModel()
        {
            var viewModel = Application.Current.MainWindow.DataContext;
            var result = viewModel as ViewModelBase;
            if (result == null) throw new InvalidCastException(
                "MainWindow の ViewModel が ViewModelBase から派生していません。");
            return result;
        }

        /// <summary>
        /// 指定されたビューモデルのインスタンスの数を返します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static int Count(Type type)
        {
            if (!type.IsSubclassOf(typeof(ViewModelBase))) throw new ArgumentException(
                string.Format("引数の型が ViewModelBase の派生クラスになっていません(引数の型:{0})。", type.Name));
            var result = 0;
            targetViewModelDoAction(type, n => ++result);
            return result;
        }

        /// <summary>
        /// 指定されたビューモデルのインスタンスの一覧を返します。
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IReadOnlyList<ViewModelBase> GetViewModels(Type type)
        {
            if (!type.IsSubclassOf(typeof(ViewModelBase))) throw new ArgumentException(
                string.Format("引数の型が ViewModelBase の派生クラスになっていません(引数の型:{0})。", type.Name));
            var result = new List<ViewModelBase>();
            targetViewModelDoAction(type, n => result.Add(n));
            return result;
        }

        /// <summary>
        /// 指定されたビューモデルのインスタンスの IWindowCloseCommand インターフェイス の
        /// WindowClose メソッドを実行します。
        /// </summary>
        /// <returns></returns>
        public static void CloseViewModels(Type type)
        {
            if (!type.IsSubclassOf(typeof(ViewModelBase))) throw new ArgumentException(
                string.Format("引数の型が ViewModelBase の派生クラスになっていません(引数の型:{0})。", type.Name));
            if (Count(type) == 0) return;

            var list = GetViewModels(type);
            if (list.First() as IWindowCloseCommand == null)
            {
                throw new InvalidCastException(
                "オブジェクトは IWindowCloseCommand インターフェイスを実装していません。: " + type.ToString());
            }

            // ウィンドウが閉じることのできる状態か確認
            if (!isReadyCloseWindows(list))
            {
                throw new WindowPendingProcessException(
                    string.Format("ViewModel:{0} が閉じることの出来る状態にありません。", type.Name));
            }
            // ウィンドウを閉じる
            foreach (var n in list)
            {
                (n as IWindowCloseCommand).WindowClose();
            }
        }

        /// <summary>
        /// すべてのウィンドウが閉じることが可能か確認します。
        /// </summary>
        public static bool IsReadyCloseAllWindows
        {
            get
            {
                var list = new List<ViewModelBase>();
                targetViewModelDoAction(typeof(ViewModelBase), n => list.Add(n), true);
                return isReadyCloseWindows(list);
            }
        }

        // リストで渡されたウィンドウが閉じることが可能か確認します。
        private static bool isReadyCloseWindows(IReadOnlyList<ViewModelBase> list)
        {
            var result = true;
            foreach (var n in list)
            {
                var vm = n as IWindowCloseCommand;
                if (vm == null) throw new InvalidCastException(
                    "ViewModel は IWindowCloseCommand インターフェイスを実装していません。: " + n.GetType().Name);
                if (!vm.CanCloseWindow)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

        // Application クラスで管理しているインスタンス化された Window のコレクションからビューモデルを取得して、action の処理を行います。
        private static void targetViewModelDoAction(Type type, Action<ViewModelBase> action, bool isSubClass = false)
        {
            foreach (var n in Application.Current.Windows)
            {
                var window = n as Window;
                if (window.DataContext == null) continue;

                var vmType = window.DataContext.GetType();
                var cond = isSubClass ? vmType == type || vmType.IsSubclassOf(typeof(ViewModelBase)) :
                                        vmType == type;

                if (cond)
                {
                    var viewModel = window.DataContext as ViewModelBase;
                    if (viewModel != null)
                    {
                        action(viewModel);
                    }
                }
            }
        }
    }
}
