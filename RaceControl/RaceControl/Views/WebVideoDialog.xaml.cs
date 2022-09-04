using Microsoft.Web.WebView2.Core;
using System;

namespace RaceControl.Views;

public partial class WebVideoDialog
{
    private readonly ILogger _logger;

    public WebVideoDialog(ILogger logger)
    {
        InitializeComponent();
        _logger = logger;
    }
    private void LoginDialogLoaded(object sender, RoutedEventArgs e)
    {
        InitializeWebViewAsync().Await(InitializeWebViewSuccess, InitializeWebViewFailed, true);
    }

    private async Task InitializeWebViewAsync()
    {
        var userDataFolder = FolderUtils.GetWebView2UserDataPath();
        var environment = await CoreWebView2Environment.CreateAsync(userDataFolder: userDataFolder);
        await WebView2.EnsureCoreWebView2Async(environment);
    }

    private void InitializeWebViewSuccess()
    {
        WebView2.Source = new Uri("https://f1tv.formula1.com/detail/1000005195/2022-dutch-gp-practice-2?action=play");
    }

    private void NavigationComplete(object sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        var app = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var path = Path.Combine(app, "Scripts/rebuild.js");
        var script = File.ReadAllText(path);

        WebView2.ExecuteScriptAsync(script);
        //WebView2.ExecuteScriptAsync("window.addEventListener('contextmenu', window => {window.preventDefault();});");
    }

    private void InitializeWebViewFailed(Exception ex)
    {
        _logger.Error(ex, "An error occurred while initializing webview.");
    }

    private void WebView2_NavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
    {

    }
}