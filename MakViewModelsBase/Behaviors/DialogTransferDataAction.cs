﻿using System;
using System.Windows;
using System.Windows.Interactivity;
using System.Windows.Input;

using MakCraft.Behaviors.Interfaces;

namespace MakCraft.Behaviors
{
    /// <summary>
    /// データを渡してダイアログ ウィンドウを表示するアクション
    /// ダイアログ側のビューモデルにデータ受取り用の「public object Container」プロパティが必要
    /// </summary>
    public class DialogTransferDataAction : TriggerAction<FrameworkElement>
    {
        /// <summary>
        /// ダイアログウィンドウに渡すデータを格納
        /// </summary>
        public static readonly DependencyProperty ParameterProperty = DependencyProperty.Register(
            "Parameter", typeof(object), typeof(DialogTransferDataAction),
            new UIPropertyMetadata(null)
            );
        /// <summary>
        /// ダイアログウィンドウに渡すデータを格納
        /// </summary>
        public object Parameter
        {
            get { return (object)GetValue(ParameterProperty); }
            set { SetValue(ParameterProperty, value); }
        }

        /// <summary>
        /// 表示するダイアログのクラス名
        /// </summary>
        public static readonly DependencyProperty DialogTypeProperty = DependencyProperty.Register(
            "DialogType", typeof(Type), typeof(DialogTransferDataAction),
            new UIPropertyMetadata()
            );
        /// <summary>
        /// 表示するダイアログのクラス名
        /// </summary>
        public Type DialogType
        {
            get { return (Type)GetValue(DialogTypeProperty); }
            set { SetValue(DialogTypeProperty, value); }
        }

        /// <summary>
        /// ダイアログの表示種別
        /// </summary>
        public static readonly DependencyProperty DialogModeProperty = DependencyProperty.Register(
            "DialogMode", typeof(DialogModes), typeof(DialogTransferDataAction),
            new UIPropertyMetadata()
            );
        /// <summary>
        /// ダイアログの表示種別
        /// </summary>
        public DialogModes DialogMode
        {
            get { return (DialogModes)GetValue(DialogModeProperty); }
            set { SetValue(DialogModeProperty, value); }
        }

        /// <summary>
        /// ダイアログを閉じた際に実行するコールバック
        /// </summary>
        public static readonly DependencyProperty ActionCallBackProperty = DependencyProperty.Register(
            "ActionCallBack", typeof(Action<bool?>), typeof(DialogTransferDataAction),
            new UIPropertyMetadata()
            );
        /// <summary>
        /// ダイアログを閉じた際に実行するコールバック
        /// </summary>
        public Action<bool?> ActionCallBack
        {
            get { return (Action<bool?>)GetValue(ActionCallBackProperty); }
            set { SetValue(ActionCallBackProperty, value); }
        }

        /// <summary>
        /// 作成したウィンドウのビューモデルオブジェクトへの参照
        /// ダイアログ側で設定したデータの参照用
        /// </summary>
        public static readonly DependencyProperty ResultViewModelProperty = DependencyProperty.Register(
            "ResultViewModel", typeof(object), typeof(DialogTransferDataAction),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        /// <summary>
        /// 作成したウィンドウのビューモデルオブジェクトへの参照
        /// ダイアログ側で設定したデータの参照用
        /// </summary>
        public object ResultViewModel
        {
            get { return GetValue(ResultViewModelProperty); }
            set { SetValue(ResultViewModelProperty, value); }
        }

        /// <summary>
        /// Invokes the action. 
        /// </summary>
        /// <param name="parameter"></param>
        protected override void Invoke(object parameter)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // 指定された DialogType の確認
            if (DialogType == null) throw new InvalidOperationException(
                "DialogType が null のため、ダイアログを生成できません。");
            if (!DialogType.IsSubclassOf(typeof(Window))) throw new InvalidOperationException(
                string.Format("表示するように指定された {0} は Window の派生クラスではありません。", DialogType.Name));
            // ダイアログの型からダイアログのインスタンスを作成
            var instance = Activator.CreateInstance(DialogType);
            // 生成したダイアログの Owner プロパティを設定
            var window = instance as Window;
            var current = Window.GetWindow(AssociatedObject);
            window.Owner = current;
            Mouse.OverrideCursor = null;
            ResultViewModel = window.DataContext;   // ビューモデルをプロパティへセット
            // Parameter がある場合には ViewModel の Container へデータをセットする
            if (Parameter != null)
            {
                var recievedViewModel = window.DataContext as IDialogTransferContainer;
                if (recievedViewModel == null) throw new InvalidCastException(
                    string.Format("{0} のビューモデルが IDialogTransferContainer インターフェイスを実装していません。", DialogType.Name));

                recievedViewModel.Container = Parameter;
            }

            if (DialogMode == DialogModes.Modal)
            {
                // モーダル ダイアログを表示する
                if (ActionCallBack != null)
                {
                    ActionCallBack((bool?)DialogType.InvokeMember("ShowDialog", System.Reflection.BindingFlags.InvokeMethod,
                        null, window, null));
                }
                else
                {
                    DialogType.InvokeMember("ShowDialog", System.Reflection.BindingFlags.InvokeMethod,
                        null, window, null);
                }
            }
            else
            {
                // モードレス ダイアログを表示する
                DialogType.InvokeMember("Show", System.Reflection.BindingFlags.InvokeMethod,
                    null, window, null);
            }

            ResultViewModel = null;    // 作成された ViewModel オブジェクトへの参照をクリアしておく。
        }
    }
}
