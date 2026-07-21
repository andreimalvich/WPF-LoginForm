using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace WPF_LoginForm.Views;


public partial class MainView : Window
{
    private const int WM_NCLBUTTONDOWN = 0xA1;
    private static readonly IntPtr HTCAPTION = new(2);


    public MainView()
    {
        InitializeComponent();        
    }

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

    private void pnlControlBar_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        WindowInteropHelper helper = new(this);
        SendMessage(helper.Handle, WM_NCLBUTTONDOWN, HTCAPTION, IntPtr.Zero);
    }

    private void pnlControlBar_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
    }

    private void btnClose_Click(object sender, RoutedEventArgs e)
    {
        Application.Current.Shutdown();
    }

    private void btnMinimize_Click(object sender, RoutedEventArgs e)
    {
        SystemCommands.MinimizeWindow(this);
    }

    private void btnMaximize_Click(object sender, RoutedEventArgs e)
    {
        if (this.WindowState == WindowState.Normal)
            this.WindowState = WindowState.Maximized;
        else this.WindowState = WindowState.Normal;
    }
}
