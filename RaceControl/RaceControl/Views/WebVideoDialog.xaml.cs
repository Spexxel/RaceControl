﻿using Microsoft.Web.WebView2.Core;
using RaceControl.ViewModels;

namespace RaceControl.Views;

public partial class WebVideoDialog
{
    private readonly ILogger _logger;
    private const string BaseStreamUrl = "https://f1tv.formula1.com/detail/";

    public WebVideoDialog(ILogger logger)
    {
        InitializeComponent();
        _logger = logger;
    }

    private void WebDialogLoaded(object sender, RoutedEventArgs e)
    {
        InitializeWebViewAsync().Await(InitializeWebViewSuccess, InitializeWebViewFailed, true);
    }

    private async Task InitializeWebViewAsync()
    {
        var userDataFolder = FolderUtils.GetWebView2UserDataPath();
        var environment = await CoreWebView2Environment.CreateAsync(userDataFolder: userDataFolder);
        await WebView2.EnsureCoreWebView2Async(environment);

        var window = Window.GetWindow(this);
        window.Closing += OnClosing;
    }

    private void InitializeWebViewSuccess()
    {
        var data = DataContext as WebVideoDialogViewModel;
        var uri = BuildContentUri(data.PlayableContent);
        WebView2.Source = uri;
    }

    private void InitializeWebViewFailed(Exception ex)
    {
        _logger.Error(ex, "An error occurred while initializing webview.");
    }

    private void NavigationComplete(object sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        var app = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var path = Path.Combine(app, "Scripts/strip.js");
        var script = File.ReadAllText(path);

        WebView2.ExecuteScriptAsync(script);        
    }

    private void OnClosing(object sender, CancelEventArgs e)
    {
        WebView2.Dispose();
    }    

    private static Uri BuildContentUri(IPlayableContent content)
    {
        var contentId = content.SyncUID;
        var title = content.Title.Replace(' ', '-');
        var uriString = $"{BaseStreamUrl}{contentId}/{title}?action=play";

        return new(uriString);
    }
}