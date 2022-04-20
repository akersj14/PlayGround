using System;
using System.Windows;
using PlayGround.Core;
using PlayGround.Core.ViewModels;
using ReactiveUI;

namespace PlayGround.WPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : IViewFor<MainWindowViewModel>
{
  public MainWindow()
  {
    InitializeComponent();
    ViewModel = ContainerProvider.Resolve<MainWindowViewModel>();
    DataContext = ViewModel;
  }

  object IViewFor.ViewModel
  {
    get => ViewModel;
    set => ViewModel = (MainWindowViewModel)value;
  }

  public MainWindowViewModel ViewModel { get; set; }

  protected override void OnClosed(EventArgs e)
  {
    Application.Current.Shutdown();
    base.OnClosed(e);
  }
}