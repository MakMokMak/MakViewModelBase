using System;
using MakCraft.ViewModels;

namespace AppSettingsSample.ViewModels
{
    internal class OptionWindowViewModel : ModalViewModelBase
    {
        private readonly IOptionSources _optionSource;

        public OptionWindowViewModel() : this(OptionSources.Instance) { }
        public OptionWindowViewModel(IOptionSources optionSource)
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] Create OptionWindowViewModel");
            _optionSource = optionSource;
            FontSize = _optionSource.FontSize;
            FontSizeMagnification = _optionSource.FontSizeMagnification;
        }

        private double _fontSize;
        public double FontSize
        {
            get => _fontSize;
            set
            {
                SetProperty(ref _fontSize, value);
                _optionSource.FontSize = value;
                TitleFontSize = value * _optionSource.FontSizeMagnification;
            }
        }

        private double _fontSizeMagnification;
        public double FontSizeMagnification
        {
            get => _fontSizeMagnification;
            set
            {
                SetProperty(ref _fontSizeMagnification, value);
                _optionSource.FontSizeMagnification = value;
                TitleFontSize = _fontSize * value;
            }
        }

        private double _titleFontSize;
        public double TitleFontSize
        {
            get => _titleFontSize;
            set => SetProperty(ref _titleFontSize, value);
        }

        ~OptionWindowViewModel()
        {
            System.Diagnostics.Debug.WriteLine($"[{DateTime.Now}] Destruct OptionWindowViewModel");
        }
    }
}
