using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace MakCraft.Behaviors
{
    /// <summary>
    /// PowerModeChanged イベント発生時にコマンドを起動するビヘイビア。
    /// コマンドの引数に PowerModeChangedEventArgs をセットします。
    /// </summary>
    public class PowerModeChangedBehavior : Behavior<Window>
    {
        /// <summary>
        /// 依存関係プロパティ CommandProperty
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register(
            "Command", typeof(ICommand), typeof(PowerModeChangedBehavior), new UIPropertyMetadata());

        /// <summary>
        /// Command プロパティ
        /// </summary>
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        /// <summary>
        /// ビヘイビアーが AssociatedObject にアタッチされた後で呼び出されます。 
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            Microsoft.Win32.SystemEvents.PowerModeChanged +=
                new Microsoft.Win32.PowerModeChangedEventHandler(onPowerModeChanged);
            AssociatedObject.Closed += new EventHandler(onClosed); // OnDetaching が呼ばれない場合に備えて Closed イベントでもイベントへのフックを解除する
        }

        /// <summary>
        /// ビヘイビアーが AssociatedObject からデタッチされるとき、その前に呼び出されます。
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            Microsoft.Win32.SystemEvents.PowerModeChanged -=
                new Microsoft.Win32.PowerModeChangedEventHandler(onPowerModeChanged);
            AssociatedObject.Closed -= new EventHandler(onClosed);
        }

        private void onPowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            if (Command == null) return;

            Command.Execute(e);
        }

        private void onClosed(object sender, EventArgs e)
        {
            Microsoft.Win32.SystemEvents.PowerModeChanged -=
                new Microsoft.Win32.PowerModeChangedEventHandler(onPowerModeChanged);
            AssociatedObject.Closed -= new EventHandler(onClosed);
        }
    }
}
