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
        
        DragAndDropListBox = new ListBoxDragAndDrop();
        DragAndDropListBox.Style = new Style(targetType: typeof(ListBox),
            basedOn: (Style)ThemeManager.Current.DetectTheme()?.Resources["MahApps.Styles.ListBox"]);
        DragAndDropListBox.Move = (source, target) => ViewModel?.MoveFilter(source, target);
        DragAndDropListBox.ItemsSource = ViewModel?.FilterNames.Select(vm => vm.GetType().Name);
        DragAndDropListBox.Margin = new Thickness(5);
        ListBoxStackPanel.Children.Insert(0, DragAndDropListBox);
            
        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel,
                    model => model.FilterNames,
                    view => view.DragAndDropListBox.ItemsSource)
                .DisposeWith(disposable);
            
            this.OneWayBind(ViewModel,
                    model => model.OperationsToAdd,
                    view => view.AddFilterButtonContextMenu.ItemsSource)
                .DisposeWith(disposable);
        });
    }
    private ListBoxDragAndDrop DragAndDropListBox { get; }
}