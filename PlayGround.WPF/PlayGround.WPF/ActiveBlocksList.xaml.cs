using System.Linq;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using ControlzEx.Theming;
using PlayGround.Vision;
using ReactiveUI;

namespace PlayGround.WPF;

public partial class ActiveBlocksList 
{
    public ActiveBlocksList()
    {
        InitializeComponent();
        DataContext = VisionModule.Resolve<ActiveBlockListViewModel>();
        ViewModel = (ActiveBlockListViewModel) DataContext;
        
        _listBoxDragAndDrop = new ListBoxDragAndDrop();
        _listBoxDragAndDrop.Style = new Style(targetType: typeof(ListBox),
            basedOn: (Style)ThemeManager.Current.DetectTheme()?.Resources["MahApps.Styles.ListBox"]);
        _listBoxDragAndDrop.Move = (source, target) => ViewModel?.MoveFilter(source, target);
        _listBoxDragAndDrop.ItemsSource = ViewModel?.FilterNames.Select(vm => vm.GetType().Name);
        _listBoxDragAndDrop.Margin = new Thickness(5);
        ListBoxStackPanel.Children.Insert(0, _listBoxDragAndDrop);
            
        this.WhenActivated(disposable =>
        {
            this.OneWayBind(ViewModel,
                    model => model.FilterNames,
                    view => view._listBoxDragAndDrop.ItemsSource)
                .DisposeWith(disposable);
            
            this.OneWayBind(ViewModel,
                    model => model.OperationsToAdd,
                    view => view.AddFilterButtonContextMenu.ItemsSource)
                .DisposeWith(disposable);
        });
    }
    private ListBoxDragAndDrop _listBoxDragAndDrop { get; }
}