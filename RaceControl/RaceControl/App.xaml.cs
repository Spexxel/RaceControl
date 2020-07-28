﻿using Prism.Ioc;
using Prism.Modularity;
using RaceControl.Modules.ModuleName;
using RaceControl.Services;
using RaceControl.Services.F1TV;
using RaceControl.Services.Interfaces;
using RaceControl.Services.Interfaces.F1TV;
using RaceControl.Services.Interfaces.Lark;
using RaceControl.Services.Lark;
using RaceControl.ViewModels;
using RaceControl.Views;
using System.Windows;

namespace RaceControl
{
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<LoginView, LoginViewViewModel>();

            containerRegistry.RegisterSingleton<IRestClient, RestClient>();
            containerRegistry.RegisterSingleton<IAuthorizationService, AuthorizationService>();
            containerRegistry.RegisterSingleton<IApiService, ApiService>();
            containerRegistry.Register<IF1TVClient, F1TVClient>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleNameModule>();
        }
    }
}