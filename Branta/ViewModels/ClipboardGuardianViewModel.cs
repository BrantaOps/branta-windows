﻿using Branta.Classes;
using Branta.Commands;
using System.Timers;
using System.Windows.Input;
using Timer = System.Timers.Timer;

namespace Branta.ViewModels;

public class ClipboardGuardianViewModel : BaseViewModel
{
    private readonly Timer _clipboardGuardianTimer;

    private ClipboardItemViewModel _clipboardItem;
    public ClipboardItemViewModel ClipboardItem
    {
        get
        {
            return _clipboardItem;
        }
        set
        {
            _clipboardItem = value;
            OnPropertyChanged(nameof(ClipboardItem));
        }
    }

    public ICommand ClipboardGuardianCommand { get; }

    public ClipboardGuardianViewModel(NotificationCenter notificationCenter, Settings settings)
    {
        ClipboardGuardianCommand = new ClipboardGuardianCommand(this, notificationCenter, settings);

        _clipboardGuardianTimer = new Timer(new TimeSpan(0, 0, 2));
        _clipboardGuardianTimer.Elapsed += (object sender, ElapsedEventArgs e) => ClipboardGuardianCommand.Execute(null);
        _clipboardGuardianTimer.Start();
    }
}