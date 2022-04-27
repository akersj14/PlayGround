using System;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PlayGround.Core;
using PlayGround.Core.ViewModels;
using ReactiveUI;

namespace PlayGround.WPF;

public partial class DroppableObject 
{
    public DroppableObject()
    {
        InitializeComponent();
        ViewModel = ContainerProvider.Resolve<DroppableObjectViewModel>();
        DataContext = ViewModel;
        this.WhenActivated(disposable =>
        {
            this.Events().MouseMove.Subscribe(args =>
                {
                    if (args.LeftButton != MouseButtonState.Pressed) return;
                    IsHitTestVisible = false;
                    DragDrop.DoDragDrop(this, new DataObject(DataFormats.Serializable, this), DragDropEffects.Move);
                    IsHitTestVisible = true;
                })
                .DisposeWith(disposable);
        });
    }
}