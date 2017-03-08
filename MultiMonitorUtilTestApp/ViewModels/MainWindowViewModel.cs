﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using MakCraft.Utils;
using MakCraft.ViewModels;

namespace MultiMonitorUtilTestApp.ViewModels
{
    class MainWindowViewModel : ViewModelBase
    {
        private double _saveTop;
        private double _saveLeft;
        private double _saveHeight;
        private double _saveWidth;

        private double _top;
        public double Top
        {
            get { return _top; }
            set { base.SetProperty(ref _top, value); }
        }

        private double _left;
        public double Left
        {
            get { return _left; }
            set { base.SetProperty(ref _left, value); }
        }

        private double _width = 525.0;
        public double Width
        {
            get { return _width; }
            set { base.SetProperty(ref _width, value); }
        }

        private double _height = 350.0;
        public double Height
        {
            get { return _height; }
            set { base.SetProperty(ref _height, value); }
        }

        private string _textPosition;
        public string TextPosition
        {
            get { return _textPosition; }
            set { base.SetProperty(ref _textPosition, value); }
        }

        private string _TextNotice;
        public string TextNotice
        {
            get { return _TextNotice; }
            set { base.SetProperty(ref _TextNotice, value); }
        }

        private IMutiMonitorUtil _multiMonitorUtil;
        public IMutiMonitorUtil MultiMonitorUtil
        {
            get { return _multiMonitorUtil; }
            set { _multiMonitorUtil = value; }
        }

        private void virtualCloseExecute()
        {
            TextPosition = $"Left:{Left}, Top:{Top} : Width:{Width}, Height:{Height}";
            _saveTop = Top;
            _saveLeft = Left;
            _saveHeight = Height;
            _saveWidth = Width;
        }
        private ICommand _virtualCloseCommand;
        public ICommand VirtualCloseCommand
        {
            get
            {
                if (_virtualCloseCommand == null)
                {
                    _virtualCloseCommand = new RelayCommand(virtualCloseExecute);
                }
                return _virtualCloseCommand;
            }
        }

        private void virtualOpenExecute()
        {
            Top = _saveTop;
            Left = _saveLeft;
            Height = _saveHeight;
            Width = _saveWidth;
        }
        private ICommand _virtualOpenCommand;
        public ICommand VirtualOpenCommand
        {
            get
            {
                if (_virtualOpenCommand == null)
                {
                    _virtualOpenCommand = new RelayCommand(virtualOpenExecute);
                }
                return _virtualOpenCommand;
            }
        }

        private void isInRangeExecute()
        {
            var rect = new System.Windows.Rect(Left, Top, Width, Height);
            var monitorName = (_multiMonitorUtil.GetMoniterNameCenterPosition(rect) ??
                _multiMonitorUtil.GetInRangeMonitorNameExcludingMargin(rect)) ??
                "表示範囲外です。";
            var text = $"{_multiMonitorUtil.IsInRange(rect)}, In monitor: {monitorName}";
            TextNotice = text;
        }
        private ICommand _isRangeCommand;
        public ICommand IsRangeCommand
        {
            get
            {
                if (_isRangeCommand == null)
                {
                    _isRangeCommand = new RelayCommand(isInRangeExecute);
                }
                return _isRangeCommand;
            }
        }

        private void onSourceInitializedExecute()
        {
            _multiMonitorUtil.Margin = 100.0;

            // プライマリモニタの中央に表示させる
            var rect = _multiMonitorUtil.GetWorkingArea(_multiMonitorUtil.GetPrimaryMonitorName());
            Top = rect.Height / 2 - Height / 2;
            Left = rect.Width / 2 - Width / 2;
        }
        private ICommand _onSourceInitializedCommand;
        public ICommand OnSourceInitializedCommand
        {
            get
            {
                if (_onSourceInitializedCommand == null)
                {
                    _onSourceInitializedCommand = new RelayCommand(onSourceInitializedExecute);
                }
                return _onSourceInitializedCommand;
            }
        }

        private void onDisplaySettingChangedExecute()
        {
            TextNotice = $"DisplaySettingChanged at {DateTime.Now}";
        }
        private ICommand _onDisplaySettingChangedCommand;
        public ICommand OnDisplaySettingChangedCommand
        {
            get
            {
                if (_onDisplaySettingChangedCommand == null)
                {
                    _onDisplaySettingChangedCommand = new RelayCommand(onDisplaySettingChangedExecute);
                }
                return _onDisplaySettingChangedCommand;
            }
        }
    }
}
