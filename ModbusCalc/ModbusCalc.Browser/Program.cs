﻿using System.Runtime.Versioning;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Browser;
using Avalonia.Media;
using ModbusCalc;

internal sealed partial class Program
{
    private static Task Main(string[] args) => BuildAvaloniaApp()
        .WithInterFont()
        .StartBrowserAppAsync("out");

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder
        .Configure<App>()
        .With(new FontManagerOptions
        {
            FontFallbacks = [new FontFallback { FontFamily = new FontFamily("avares://ModbusCalc/Assets/SourceHanSansSC-Regular-subset.otf#Source Han Sans SC") }]
        });
}