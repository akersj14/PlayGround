using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using PlayGround.Vision;

namespace PlayGround.WPF;

public partial class ListBoxDragAndDrop : IDisposable
{
    public ListBoxDragAndDrop()
    {
        InitializeComponent();

        this
            .Events()
            .PreviewMouseMove
            .Subscribe(args =>
            {
                var point = args.GetPosition(null);
                var diff = _dragStartPoint - point;
                if (args.LeftButton != MouseButtonState.Pressed ||
                    (!(Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance) &&
                     !(Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))) return;
                var lb = args.Source as ListBox;
                var lbi = FindVisualParent<ListBoxItem>(((DependencyObject) args.OriginalSource));
                if (lbi != null)
                {
                    DragDrop.DoDragDrop(lbi, lbi.DataContext, DragDropEffects.Move);
                }
            })
            .DisposeWith(_compositeDisposable);

        var style = new Style(typeof(ListBoxItem));
        style.Setters.Add(new Setter(ListBoxItem.AllowDropProperty, true));
        style.Setters.Add(
            new EventSetter(
                ListBoxItem.PreviewMouseLeftButtonDownEvent,
                new MouseButtonEventHandler(ListBoxItem_PreviewMouseLeftButtonDown)));
        style.Setters.Add(
            new EventSetter(
                ListBoxItem.DropEvent, 
                new DragEventHandler(ListBoxItem_Drop)));
            
        ItemContainerStyle = style;
    }

    public Action<int, int> Move { get; set; }

    private Point _dragStartPoint;
    private readonly CompositeDisposable _compositeDisposable = new();
    private T FindVisualParent<T>(DependencyObject child)
        where T : DependencyObject
    {
        var parentObject = VisualTreeHelper.GetParent(child);
        return parentObject switch
        {
            null => null,
            T parent => parent,
            _ => FindVisualParent<T>(parentObject)
        };
    }
    private void ListBoxItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        _dragStartPoint = e.GetPosition(null);
    }

    private void ListBoxItem_Drop(object sender, DragEventArgs e)
    {
        if (sender is not ListBoxItem item) return;
        var target = item.DataContext as ListBoxBlockViewModel;

        if (e.Data.GetData(typeof(ListBoxBlockViewModel)) is not ListBoxBlockViewModel source || target == null) return;
        var sourceIndex = Items.IndexOf(source); 
        var targetIndex = Items.IndexOf(target);

        Move?.Invoke(sourceIndex, targetIndex);
    }

    public void Dispose()
    {
        _compositeDisposable.Dispose();
    }
}