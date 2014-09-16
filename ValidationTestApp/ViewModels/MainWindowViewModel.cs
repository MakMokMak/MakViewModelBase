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
                // set アクセサの先頭で検証エラーの削除を行っておく
                base.RemoveItemValidationError(() => MemoTitle);
                // 本文中にタイトルが書かれているかのチェックを行う。
                _service.CheckTitleNote(PropertyHelper.GetName(() => MemoTitle), MemoNote, value);
                _title = value;
                // 関連チェックを行っている関連プロパティの set アクセサを通す
                doRelation(operateProperty.MemoTitle);
                // set アクセサの最後でプロパティ変更通知を行う
                base.RaisePropertyChanged(() => MemoTitle);
            }
        }

        [MaxLength(20, ErrorMessage = "本文の長さは20文字までにしてください。")]
        [Required(ErrorMessage = "この項目は必須項目です。")]
        public string MemoNote
        {
            get { return _note; }
            set
            {
                // set アクセサの先頭で検証エラーの削除を行っておく
                base.RemoveItemValidationError(() => MemoNote);
                // 本文中にタイトルが書かれているかのチェックを行う。
                _service.CheckTitleNote(PropertyHelper.GetName(() => MemoNote), value, MemoTitle);
                _note = value;
                // 関連チェックを行っている関連プロパティの set アクセサを通す
                doRelation(operateProperty.MemoNote);
                // set アクセサの最後でプロパティ変更通知を行う
                base.RaisePropertyChanged(() => MemoNote);
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
                base.RaisePropertyChangedWithRemoveItemValidationError(() => MemoAge);
            }
        }

        private bool _option;
        public bool Option
        {
            get { return _option; }
            set
            {
                _option = value;
                Remark = Remark;
                base.RaisePropertyChanged(() => Option);
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
                _remark = value;
                // 属性での検証のみであれば、検証エラー削除付きのプロパティ変更通知が使える
                base.RaisePropertyChangedWithRemoveItemValidationError(() => Remark);
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
            _service.AddMemo(memo);
            base.RaisePropertyChanged(() => Memos);
            MemoTitle = string.Empty;
            MemoNote = string.Empty;
            MemoAge = string.Empty;
            Option = false;
            Remark = string.Empty;
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
