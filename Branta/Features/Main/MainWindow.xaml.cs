﻿using Branta.Classes;
using Microsoft.Extensions.Logging;
using System.ComponentModel;
using System.Windows;
using Application = System.Windows.Application;

namespace Branta.Features.Main;

public partial class MainWindow
{
    public MainWindow(LanguageStore languageStore, AppSettings appSettings,
        ILogger<MainWindow> logger)
    {
        MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

        try
        {
            InitializeComponent();

            TbVersion.Text = Helper.GetBrantaVersionWithoutCommitHash();

            this.SetLanguageDictionary(languageStore);
            Analytics.Init(appSettings);
            SetResizeImage(ImageScreenSize);
        }
        catch (Exception ex)
        {
            logger.LogError(ex.Message);
            MessageBox.Show("Branta Failed to Start.", "Branta Exception");
            Application.Current.Shutdown();
        }
    }

    protected sealed override void OnClosing(CancelEventArgs e)
    {
        e.Cancel = true;
        Hide();
        base.OnClosing(e);
    }
}