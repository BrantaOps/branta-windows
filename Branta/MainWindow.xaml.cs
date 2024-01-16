using Branta.Domain;
using Branta.Enums;
using Branta.Utils;
using System.Windows;
using System.Windows.Controls;
using Color = Branta.Enums.Color;

namespace Branta;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        var wallets = VerifyWallets();

        SetWalletsDetected(wallets.Count);
        BuildWalletTable(wallets);
    }

    public void SetWalletsDetected(int count)
    {
        TbWalletsDetected.Text = $"{count} Wallets Detected.";
    }

    public void BuildWalletTable(List<Wallet> wallets)
    {
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
                        FontSize = 20
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

    private static List<Wallet> VerifyWallets()
    {
        return new List<Wallet>
        {
            new()
            {
                Name = "Blockstream Green",
                Status = WalletStatus.Verified
            },
            new()
            {
                Name = "Trezor Suite",
                Status = WalletStatus.Verified
            },
            new()
            {
                Name = "Ledger Live",
                Status = WalletStatus.NotVerified
            },
            new()
            {
                Name = "Sparrow",
                Status = WalletStatus.Verified
            }
        };
    }
}