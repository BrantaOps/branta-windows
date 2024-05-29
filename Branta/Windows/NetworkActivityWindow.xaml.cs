﻿using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Branta.Exceptions;
using Branta.Models;
using Branta.Stores;

namespace Branta.Windows;

public partial class NetworkActivityWindow
{
    public ObservableCollection<NetworkActivityItem> NetworkActivities { get; } = new();

    public Wallet Wallet { get; set; }

    private Process _process;
    private readonly CancellationTokenSource _cancellationTokenSource = new();

    public NetworkActivityWindow(Wallet wallet, LanguageStore languageStore)
    {
        InitializeComponent();
        DataContext = this;
        SetLanguageDictionary(languageStore);

        Wallet = wallet;

        Task.Run(() =>
        {
            try
            {
                WatchNetworkTraffic(wallet, _cancellationTokenSource.Token);
            }
            catch (InsufficientPrivilegeException)
            {
                Dispatcher.Invoke(() =>
                {
                    TbAdmin.Visibility = Visibility.Visible;
                    GActivity.Visibility = Visibility.Hidden;
                });
            }
            catch
            {
                // Ignore
            }
        });
    }

    private void UpdateDisplay(string line)
    {
        var info = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Dispatcher.Invoke(() => NetworkActivities.Add(new NetworkActivityItem
        {
            ProcessId = info[4],
            IpAddress = info[1],
        }));
    }

    protected new void BtnClose_OnClick(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource.Cancel();
        _process.Close();

        base.BtnClose_OnClick(sender, e);
    }

    private void WatchNetworkTraffic(Wallet wallet, CancellationToken cancellationToken)
    {
        var processIds = Process.GetProcesses()
            .Where(p => p.ProcessName.Contains(wallet.ExeName))
            .Select(p => p.Id)
            .ToList();

        var psi = new ProcessStartInfo
        {
            FileName = "netstat",
            Arguments = "-o",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        _process = new Process
        {
            StartInfo = psi
        };
        _process.Start();

        while (!_process.StandardOutput.EndOfStream)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                break;
            }

            var line = _process.StandardOutput.ReadLine();

            if (line == null)
            {
                continue;
            }

            if (line == "The requested operation requires elevation.")
            {
                throw new InsufficientPrivilegeException();
            }

            Trace.WriteLine(line);

            if (!line.TrimStart().StartsWith("TCP"))
            {
                continue;
            }

            var processId = System.Text.RegularExpressions.Regex.Split(line, @"\s+").Last();

            if (!string.IsNullOrEmpty(processId) && processIds.Contains(int.Parse(processId)))
            {
                UpdateDisplay(line);
            }
        }
    }
}