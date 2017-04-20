using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

using MakCraft.ViewModels;

using ValidationTestApp.Models;
using ValidationTestApp.Services;

namespace ValidationTestApp.ViewModels
{
    class MainWindowViewModel : ValidationViewModelBase
    {
        private IMemoService _service;
        private string _title;
        private string _note;
        private string _age;

        private operateProperty _op = operateProperty.None;
        private enum operateProperty
        {
            None,
            MemoTitle,
            MemoNote
        }

        public MainWindowViewModel()
        {
            _service = new MemoService(base.ViewModelState);
        }
        public MainWindowViewModel(IMemoService service)
        {
            _service = service;
        }

        public IList<Memo> Memos
        {
            get { return _service.GetMemos(); }
        }

        [Required(ErrorMessage = "この項目は必須項目です。")]
        public string MemoTitle
        {
            get { return _title; }
            set
            {
                // set アクセサの先頭で検証エラーの削除を行っておく(PropertyHelper.GetName メソッドでプロパティの名前を得る例)
                base.RemoveItemValidationError(PropertyHelper.GetName(() => MemoTitle));
                // 本文中にタイトルが書かれているかのチェックを行う(nameof 演算子を利用してプロパティの名前を得る例)
                _service.CheckTitleNote(nameof(MemoTitle), MemoNote, value);
                // プロパティの変更及び変更通知を行う
                base.SetProperty(ref _title, value);
                // 関連チェックを行っている関連プロパティの set アクセサを通す
                doRelation(operateProperty.MemoTitle);
            }
        }

        [MaxLength(20, ErrorMessage = "本文の長さは20文字までにしてください。")]
        [Required(ErrorMessage = "この項目は必須項目です。")]
        public string MemoNote
        {
            get { return _note; }
            set
            {
                // set アクセサの先頭で検証エラーの削除を行っておく(propertyName を省略する例)
                base.RemoveItemValidationError();
                // 本文中にタイトルが書かれているかのチェックを行う。
                _service.CheckTitleNote(nameof(MemoNote), value, MemoTitle);
                // プロパティの変更及び変更通知を行う
                base.SetProperty(ref _note, value);
                // 関連チェックを行っている関連プロパティの set アクセサを通す
                doRelation(operateProperty.MemoNote);
            }
        }

        private void doRelation(operateProperty current)
        {
            if (_op == operateProperty.None)
            {
                _op = current;
                switch (current)
                {
                    case operateProperty.MemoTitle:
                        MemoNote = MemoNote;
                        break;
                    case operateProperty.MemoNote:
                        MemoTitle = MemoTitle;
                        break;
                }
                _op = operateProperty.None;
            }
        }

        [Required(ErrorMessage = "この項目は必須項目です。")]
        [Range(minimum: 0, maximum: 130, ErrorMessage = "年齢は 0 から 130 までの数値です。")]
        public string MemoAge
        {
            get { return _age; }
            set
            {
                _age = value;
                // 属性での検証のみであれば、検証エラー削除付きのプロパティ変更通知が使える
                // propertyName は省略
                base.RaisePropertyChangedWithRemoveItemValidationError();
            }
        }

        private bool _option;
        public bool Option
        {
            get { return _option; }
            set
            {
                base.SetProperty(ref _option, value);
                // Remark プロパティの検証条件が変わるため、変更通知を行う
                base.RaisePropertyChanged(nameof(Remark));
            }
        }

        private string _remark;
        // データ検証の条件を指定
        [ValidateConditional("Option", true)]
        [Required(ErrorMessage = "この項目は必須項目です。")]
        [MaxLength(20, ErrorMessage = "備考の長さは20文字までにしてください。")]
        public string Remark
        {
            get { return _remark; }
            set
            {
                // 属性での検証のみであれば、検証エラー削除付きのプロパティ変更が使える
                base.SetPropertyWithRemoveItemValidationError(ref _remark, value);
            }
        }

        private bool _option2;
        public bool Option2
        {
            get { return _option2; }
            set
            {
                base.SetProperty(ref _option2, value);
                base.RaisePropertyChanged(nameof(Remark2));
            }
        }

        private string _remark2;
        [ValidateConditional("Option2", true)]
        [Required(ErrorMessage = "この項目は必須項目です。")]
        [MaxLength(5, ErrorMessage = "備考2の長さは5文字までにしてください。")]
        public string Remark2
        {
            get { return _remark2; }
            set
            {
                // 属性での検証のみであれば、検証エラー削除付きのプロパティ変更が使える
                base.SetPropertyWithRemoveItemValidationError(ref _remark2, value);
            }
        }

        private void addMemoComamndExecute()
        {
            var memo = new Memo
            {
                Title = _title,
                Note = _note,
                Age = int.Parse(_age),
            };
            if (Option)
            {
                memo.Remark = Remark;
            }
            if (Option2)
            {
                memo.Remark2 = Remark2;
            }
            _service.AddMemo(memo);
            base.RaisePropertyChanged(() => Memos);
            MemoTitle = string.Empty;
            MemoNote = string.Empty;
            MemoAge = string.Empty;
            Option = false;
            Remark = string.Empty;
            Option2 = false;
            Remark2 = string.Empty;
        }
        private bool addMemoComamndCanExecute(object param)
        {
            return base.IsValid;
        }
        private ICommand addMemoCommand;
        public ICommand AddMemoCommand
        {
            get
            {
                if (addMemoCommand == null)
                    addMemoCommand = new RelayCommand(addMemoComamndExecute, addMemoComamndCanExecute);
                return addMemoCommand;
            }
        }
    }
}
