﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MakCraft.Behaviors
{
    /// <summary>
    /// イベント発生時にコマンドを実行するアクション
    /// 実行するコマンドの引数に発生したイベントの EventArgs を設定します。
    /// </summary>
    public class EventCommandAction : TriggerAction<UIElement>
    {
        /// <summary>
        /// 呼び出すコマンドを取得または設定します。
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(EventCommandAction), new UIPropertyMetadata
            {
                DefaultValue = null
            });
        /// <summary>
        /// 呼び出すコマンドを取得または設定します。
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// Invokes the action.
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            var eventArgs = parameter as EventArgs;
            if (eventArgs == null)
            {
                throw new InvalidOperationException(
                    "EventCommandAction が呼び出されましたが、引数が EventArgs 型ではありませんでした。");
            }

            Command.Execute(parameter);
        }
    }
}
