using System;
using System.Reactive.Disposables;
using System.Windows;

namespace PlayGround.WPF;

public partial class DragDropCanvas : IDisposable
{
    private readonly CompositeDisposable _compositeDisposable = new();
    public DragDropCanvas()
    {
        InitializeComponent();
            this
                .Events()
                .DragOver
                .Subscribe(args =>
                {
                    var data = args.Data.GetData(DataFormats.Serializable);

                    if (data is not UIElement element) return;
                    var dropPosition = args.GetPosition(this);
                    SetLeft(element, dropPosition.X);
                    SetTop(element, dropPosition.Y);

                    if (!Children.Contains(element))
                    {
                        Children.Add(element);
                    }
                })
                .DisposeWith(_compositeDisposable);
            
            this
                .Events()
                .DragLeave
                .Subscribe(args =>
                {
                    var data = args.Data.GetData(DataFormats.Serializable);
                    if (data is FrameworkElement element)
                    {
                        Children.Remove(element);
                    }
                })
                .DisposeWith(_compositeDisposable);
    }

    public void Dispose() => _compositeDisposable.Dispose();
}