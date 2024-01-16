using System.Diagnostics;
using Branta.Automation;
using Branta.Domain;
using Branta.Utils;
using System.Windows;
using System.Windows.Controls;
using Color = Branta.Enums.Color;

namespace Branta;

public partial class MainWindow : Window
{
    private readonly System.Timers.Timer _timer;
    private const int VerifyInterval = 10;

    public MainWindow()
    {
        InitializeComponent();

        Verify();

        _timer = new System.Timers.Timer(VerifyInterval * 1000);
        _timer.Elapsed += (_, _) => Dispatcher.Invoke(Verify);
        _timer.AutoReset = true;
        _timer.Start();
    }

    private void Verify()
    {
        Trace.WriteLine("Started: Verify Wallets");
        var sw = Stopwatch.StartNew();

        var wallets = VerifyWallet.Run();

        SetWalletsDetected(wallets.Count);
        BuildWalletTable(wallets);

        sw.Stop();
        Trace.WriteLine($"Stopped: Verify Wallets. Took {sw.Elapsed}");
    }

    private void SetWalletsDetected(int count)
    {
        TbWalletsDetected.Text = $"{count} Wallets Detected.";
    }

    private void BuildWalletTable(List<Wallet> wallets)
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
}