﻿namespace Guiwindows;

public partial class App : Application
{
    public App(IServiceProvider provider)
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        var window = base.CreateWindow(activationState);
        //const int newWidth = 1400;
        //const int newHeight = 800;
        //window.X = 0;
        //window.Y = 0;
        //window.Width = newWidth;
        //window.Height = newHeight;
        //window.MinimumHeight = newHeight;
        //window.MinimumWidth = newWidth;
        //window.MaximumHeight = newHeight;
        //window.MaximumWidth = newWidth;
        return window;
    }
}