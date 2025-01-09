using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordAssistedTools.ViewModels {
  public class ParagraphInfoViewModel : ObservableObject {
		private string _text;
    public string Text {
			get => _text;
			set => SetProperty(ref _text, value);
		}

		private bool _isChecked;
    public bool IsChecked {
			get => _isChecked;
			set {
        if (SetProperty(ref _isChecked, value)) {
          UpdateParaInfoEvent?.Invoke(this, EventArgs.Empty);
        }
      }
		}

		private int _realParaWordCount;
    public int RealParaWordCount {
			get => _realParaWordCount;
			set => SetProperty(ref _realParaWordCount, value);
		}

		private int _estimateParaWordCount;
    public int EstimateParaWordCount {
			get => _estimateParaWordCount;
			set {
        if (SetProperty(ref _estimateParaWordCount, value)) {
          UpdateParaInfoEvent?.Invoke(this, EventArgs.Empty);
        }
      }
		}

		private double _oldOnlyParaSeconds;
    public double OldOnlyParaSeconds {
			get => _oldOnlyParaSeconds;
			set => SetProperty(ref _oldOnlyParaSeconds, value);
		}

		private double _oldStartToParaStartSeconds;
    public double OldStartToParaStartSeconds {
			get => _oldStartToParaStartSeconds;
			set => SetProperty(ref _oldStartToParaStartSeconds, value);
		}

		private double _oldStartToParaEndSeconds;
    public double OldStartToParaEndSeconds {
			get => _oldStartToParaEndSeconds;
			set => SetProperty(ref _oldStartToParaEndSeconds, value);
		}

    private double _newOnlyParaSeconds;
    public double NewOnlyParaSeconds {
      get => _newOnlyParaSeconds;
      set => SetProperty(ref _newOnlyParaSeconds, value);
    }

    private double _newStartToParaStartSeconds;
    public double NewStartToParaStartSeconds {
      get => _newStartToParaStartSeconds;
      set => SetProperty(ref _newStartToParaStartSeconds, value);
    }

    private double _newStartToParaEndSeconds;
    public double NewStartToParaEndSeconds {
      get => _newStartToParaEndSeconds;
      set => SetProperty(ref _newStartToParaEndSeconds, value);
    }

		private int _originWordParaId;
    public int OriginWordParaId {
			get => _originWordParaId;
			set => SetProperty(ref _originWordParaId, value);
		}

		private bool _validState;
    public bool ValidState {
			get => _validState;
			set => SetProperty(ref _validState, value);
		}

		private bool _isSelected;
    public bool IsSelected {
			get => _isSelected;
			set => SetProperty(ref _isSelected, value);
		}

		public event EventHandler UpdateParaInfoEvent;
  }
}
