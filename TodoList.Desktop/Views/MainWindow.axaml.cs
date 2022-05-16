using Avalonia.Controls;
using TodoList.Desktop.ViewModels;

namespace TodoList.Desktop.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (DataContext is not null)
            {
                var viewModel = (MainWindowViewModel)DataContext;
                viewModel.OnFilterBySummary();
            }
        }
    }
}