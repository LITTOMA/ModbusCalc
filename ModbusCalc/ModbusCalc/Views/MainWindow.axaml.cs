using Avalonia.Controls;

namespace ModbusCalc.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Attach event to make the entire window draggable except input controls
        this.PointerPressed += (sender, e) =>
        {
            var pointerOver = e.Source as Control;
            if (pointerOver is Panel)
            {
                BeginMoveDrag(e);
            }
        };
    }
}
