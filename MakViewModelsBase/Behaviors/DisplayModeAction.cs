﻿using System;
using System.Windows;
using System.Windows.Interactivity;

using MakCraft.Behaviors.Interfaces;

namespace MakCraft.Behaviors
{
    /// <summary>
    /// モードレス ウィンドウの表示変更アクション
    /// </summary>
    public class DisplayModeAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// 変更する表示状態
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register(
            "DisplayMode", typeof(WindowAction), typeof(DisplayModeAction), new UIPropertyMetadata
            {
                DefaultValue = WindowAction.Show
            });
        /// <summary>
        /// 変更する表示状態
        /// </summary>
        public WindowAction DisplayMode
        {
            get { return (WindowAction)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        /// <summary>
        /// Invokes the action. 
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            if (AssociatedObject == null) return;
            var window = Window.GetWindow(AssociatedObject);
            switch (DisplayMode)
            {
                case WindowAction.Hide:
                    hide(window);
                    break;
                case WindowAction.Show:
                    show(window);
                    break;
                case WindowAction.Close:
                    window.Close();
                    break;
            }
        }

        private static void hide(Window window)
        {
            // window が既に閉じられていたら何もしない
            if (!windowContains(window)) return;

            window.Hide();

            // ViewModel のステータスを完了にする
            var viewModel = window.DataContext as IViewModelStatus;
            if (viewModel == null) throw new InvalidCastException(
                string.Format("ViewModel が ITransitionViewModel インターフェイスを実装していません。",
                window.DataContext.GetType().Name));
            viewModel.CurrentStatus = ViewModelStatus.Completed;
        }

        private static void show(Window window)
        {
            // window が既に閉じられていたら何もしない
            if (!windowContains(window)) return;

            window.Show();
            // ウィンドウのアクティブ化を試みます。再試行回数:5
            var count = 0;
            while (!window.Activate() && count < 4)
            {
                ++count;
                System.Diagnostics.Debug.WriteLine(string.Format("Retry Activate: {0}", count));
                System.Threading.Thread.Sleep(50);
            }

            // ViewModel のステータスを未完了にする
            var viewModel = window.DataContext as IViewModelStatus;
            if (viewModel == null) throw new InvalidCastException(
                string.Format("ViewModel が ITransitionViewModel インターフェイスを実装していません。",
                window.DataContext.GetType().Name));
            viewModel.CurrentStatus = ViewModelStatus.Halfway;
        }

        // window が閉じられていないかを返す
        private static bool windowContains(Window window)
        {
            foreach (var n in Application.Current.Windows)
            {
                if (n == window) return true;
            }

            return false;
        }
    }
}
