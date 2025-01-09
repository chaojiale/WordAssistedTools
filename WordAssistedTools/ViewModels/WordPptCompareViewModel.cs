using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WordAssistedTools.ViewModels {
  public class WordPptCompareViewModel: ObservableObject {
		private int _wordId;
		public int WordId {
			get => _wordId;
			set => SetProperty(ref _wordId, value);
		}

    private bool _isChecked;
    public bool IsChecked {
			get => _isChecked;
			set {
        if (SetProperty(ref _isChecked, value)) {
          UpdateCheckedItemsEvent?.Invoke(this, EventArgs.Empty);
        }
      }
		}

		private string _wordTime;
		public string WordTime {
			get => _wordTime;
			set => SetProperty(ref _wordTime, value);
		}

		private string _wordText;
		public string WordText {
			get => _wordText;
			set => SetProperty(ref _wordText, value);
		}

		private string _pptTime;
		public string PptTime {
			get => _pptTime;
			set {
        if (SetProperty(ref _pptTime, value)) {
          IsSameTime = PptTime == WordTime;
        }
      }
    }

		private string _pptText;
		public string PptText {
			get => _pptText;
			set {
        if (SetProperty(ref _pptText, value)) {
          IsSameText = PptText.TrimEnd() == WordText.TrimEnd();
        }
      }
    }

		private bool _isSameTime;
		public bool IsSameTime {
			get => _isSameTime;
			set => SetProperty(ref _isSameTime, value);
		}

		private bool _isSameText;
		public bool IsSameText {
			get => _isSameText;
			set => SetProperty(ref _isSameText, value);
		}

    private bool _isSelected;
		public bool IsSelected {
			get => _isSelected;
			set => SetProperty(ref _isSelected, value);
		}

		private bool _isShowDiffer;
		public bool IsShowDiffer {
			get => _isShowDiffer;
			set => SetProperty(ref _isShowDiffer, value);
		}

    public event EventHandler UpdateCheckedItemsEvent;
	}
}
