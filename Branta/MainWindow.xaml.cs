using Branta.Automation;
using Branta.Domain;
using Branta.Utils;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using Branta.Views;
using Color = Branta.Enums.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using ToolTip = System.Windows.Controls.ToolTip;

namespace Branta;

public partial class MainWindow : Window
{
    private readonly System.Timers.Timer _timer;
    private readonly NotifyIcon _notifyIcon;
    private List<Wallet> _wallets = new(); 
    private const int VerifyInterval = 10;

    public MainWindow()
    {
        InitializeComponent();

        _notifyIcon = new NotifyIcon
        {
            Icon = new System.Drawing.Icon("Assets/black_circle.ico"),
            Text = "Branta",
            Visible = true
        };
        _notifyIcon.DoubleClick += NotifyIcon_Click;
        _notifyIcon.ContextMenuStrip = new ContextMenuStrip();
        _notifyIcon.ContextMenuStrip.Items.Add("Quit", null, OnClick_Quit);

        Verify();

        _timer = new System.Timers.Timer(VerifyInterval * 1000);
        _timer.Elapsed += (_, _) => Dispatcher.Invoke(Verify);
        _timer.AutoReset = true;
        _timer.Start();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (Debugger.IsAttached) return;

        e.Cancel = true;
        Hide();
        base.OnClosing(e);
    }

    private void OnClick_Quit(object sender, EventArgs e)
    {
        System.Windows.Application.Current.Shutdown();
    }

    private void NotifyIcon_Click(object sender, EventArgs e)
    {
        WindowState = WindowState.Normal;
        Activate();
        Show();
    }

    private void Verify()
    {
        Trace.WriteLine("Started: Verify Wallets");
        var sw = Stopwatch.StartNew();

        _wallets = VerifyWallet.Run(_wallets.ToDictionary(w => w.Name, w => w.Status), _notifyIcon);

        SetWalletsDetected(_wallets.Count);
        BuildWalletGrid(_wallets);

        sw.Stop();
        Trace.WriteLine($"Stopped: Verify Wallets. Took {sw.Elapsed}");
    }

    private void SetWalletsDetected(int count)
    {
        TbWalletsDetected.Text = $"{count} Wallets Detected.";
    }

    private void BuildWalletGrid(List<Wallet> wallets)
    {
        GWallet.RowDefinitions.Clear();
        GWallet.Children.Clear();

        for (var i = 0; i < wallets.Count; i++)
        {
            var wallet = wallets[i];

            var grid = new GridBuilder(i % 2 == 0 ? Color.Background : Color.BackgroundOffset)
                .AddColumnDefinition(
                    new ColumnDefinition
                    {
                        MinWidth = 250.0
                    },
                    new TextBlock
                    {
                        Text = wallet.Name,
                        Foreground = Color.White.Brush(),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontSize = 16
                    })
                .AddColumnDefinition(200, new TextBlock
                    {
                        Text = wallet.Status.Icon,
                        Foreground = wallet.Status.Color.Brush(),
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        FontWeight = FontWeights.Bold,
                        FontSize = 20,
                        ToolTip = new ToolTip
                        {
                            Content = wallet.Status.Name
                        }
                    })
                .Build();

            GWallet.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(35)
            });

            Grid.SetRow(grid, i);

            GWallet.Children.Add(grid);
        }
    }

    private void Help_Click(object sender, MouseButtonEventArgs e)
    {
        var helpWindow = new HelpWindow();

        helpWindow.Show();
    }
}