using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using ControlzEx.Theming;
using PlayGround.Core;
using PlayGround.Vision;
using ReactiveUI;

namespace PlayGround.WPF;

public partial class AppliedFilters 
{
    public AppliedFilters()
    {
        InitializeComponent();
        ViewModel = ContainerProvider.Resolve<AppliedFiltersViewModel>();
        DataContext = ViewModel;
        this.WhenActivated(disposable =>
        {
            DragAndDropListBox = new ListBoxDragAndDrop();
            DragAndDropListBox.Style = new Style(targetType: typeof(ListBox),
                basedOn: (Style)ThemeManager.Current.DetectTheme()?.Resources["MahApps.Styles.ListBox"]);
            DragAndDropListBox.Move = (source, target) => ViewModel?.MoveFilter(source, target);
            DragAndDropListBox.DisposeWith(disposable);
            DragAndDropListBox.ItemsSource = ViewModel?.FilterNames.Select(vm => vm.GetType().Name);
            DragAndDropListBox.Margin = new Thickness(5);
            
            this.OneWayBind(ViewModel,
                    model => model.FilterNames,
                    view => view.DragAndDropListBox.ItemsSource)
                .DisposeWith(disposable);
            
            ListBoxStackPanel.Children.Add(DragAndDropListBox);
        });
    }
    private ListBoxDragAndDrop DragAndDropListBox { get; set; }
}