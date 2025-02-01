﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;

namespace ModbusCalc.ViewModels;

public class FunctionCode
{
    public byte Code { get; set; }
    public string Name { get; set; } = string.Empty;
    public override string ToString() => $"0x{Code:X2} - {Name}";
}

public partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private List<FunctionCode> _functionCodes = new();

    [ObservableProperty]
    private FunctionCode _selectedFunction = new FunctionCode { Code = 0, Name = ModbusCalc.Localizations.Resources.DefaultTheme };

    [ObservableProperty]
    private int _slaveAddress = 1;

    [ObservableProperty]
    private int _startAddress = 0;

    [ObservableProperty]
    private int _dataValue = 1;

    [ObservableProperty]
    private string _separator = ModbusCalc.Localizations.Resources.Separator;

    [ObservableProperty]
    private string _result = "";

    private static readonly ushort[] CrcTable = new ushort[256];

    public MainViewModel()
    {
        InitializeCrcTable();
        InitializeFunctionCodes();
    }

    private void InitializeFunctionCodes()
    {
        FunctionCodes = new List<FunctionCode>
        {
            new FunctionCode { Code = 0x01, Name = ModbusCalc.Localizations.Resources.FunctionCode1 },
            new FunctionCode { Code = 0x02, Name = ModbusCalc.Localizations.Resources.FunctionCode2 },
            new FunctionCode { Code = 0x03, Name = ModbusCalc.Localizations.Resources.FunctionCode3 },
            new FunctionCode { Code = 0x04, Name = ModbusCalc.Localizations.Resources.FunctionCode4 },
            new FunctionCode { Code = 0x05, Name = ModbusCalc.Localizations.Resources.FunctionCode5 },
            new FunctionCode { Code = 0x06, Name = ModbusCalc.Localizations.Resources.FunctionCode6 },
            new FunctionCode { Code = 0x0F, Name = ModbusCalc.Localizations.Resources.FunctionCode7 },
            new FunctionCode { Code = 0x10, Name = ModbusCalc.Localizations.Resources.FunctionCode8 }
        };
        SelectedFunction = FunctionCodes[0];
    }

    private void InitializeCrcTable()
    {
        for (ushort i = 0; i < 256; i++)
        {
            ushort value = 0;
            ushort temp = i;
            for (byte j = 0; j < 8; j++)
            {
                if (((value ^ temp) & 0x0001) != 0)
                {
                    value = (ushort)((value >> 1) ^ 0xA001);
                }
                else
                {
                    value >>= 1;
                }
                temp >>= 1;
            }
            CrcTable[i] = value;
        }
    }

    private ushort CalculateCRC16(byte[] data)
    {
        ushort crc = 0xFFFF;
        for (int i = 0; i < data.Length; i++)
        {
            byte index = (byte)(crc ^ data[i]);
            crc = (ushort)((crc >> 8) ^ CrcTable[index]);
        }
        return crc;
    }

    [RelayCommand]
    private void Generate()
    {
        // Ensure SelectedFunction is not null
        if (SelectedFunction == null)
        {
            Result = ModbusCalc.Localizations.Resources.NoFunctionSelected;
            return;
        }

        // Basic parameter check
        if (StartAddress < 0 || StartAddress > 65535 || DataValue < 1 || DataValue > 65535)
        {
            Result = "参数错误：地址或数据超出范围";
            return;
        }

        // 准备数据帧
        List<byte> frame = new()
        {
            (byte)SlaveAddress, // 使用用户设定的从站地址
            SelectedFunction?.Code ?? 0,
            (byte)(StartAddress >> 8),
            (byte)(StartAddress & 0xFF),
            (byte)(DataValue >> 8),
            (byte)(DataValue & 0xFF)
        };

        // 计算CRC
        ushort crc = CalculateCRC16(frame.ToArray());
        frame.Add((byte)(crc & 0xFF)); // CRC低字节
        frame.Add((byte)(crc >> 8));   // CRC高字节

        // 生成结果字符串
        StringBuilder sb = new();
        for (int i = 0; i < frame.Count; i++)
        {
            if (i > 0 && !string.IsNullOrEmpty(Separator))
            {
                sb.Append(Separator);
            }
            sb.Append($"{frame[i]:X2}");
        }
        Result = sb.ToString();
    }

    partial void OnSlaveAddressChanged(int value) => Generate();
    partial void OnStartAddressChanged(int value) => Generate();
    partial void OnDataValueChanged(int value) => Generate();
    partial void OnSelectedFunctionChanged(FunctionCode value) => Generate();
    partial void OnSeparatorChanged(string value) => Generate();

    [RelayCommand]
    private async Task Copy()
    {
        if (!string.IsNullOrEmpty(Result))
        {
            TopLevel? topLevel = Application.Current?.ApplicationLifetime switch
            {
                IClassicDesktopStyleApplicationLifetime desktop => TopLevel.GetTopLevel(desktop.MainWindow),
                ISingleViewApplicationLifetime singleViewPlatform => TopLevel.GetTopLevel(singleViewPlatform.MainView),
                _ => null
            };

            if (topLevel?.Clipboard != null)
            {
                await topLevel.Clipboard.SetTextAsync(Result);
            }
        }
    }

    [RelayCommand]
    private void SetFunctionCode(string parameter)
    {
        if (int.TryParse(parameter, out int index))
        {
            index--; // 从1开始
            if (index >= 0 && index < FunctionCodes.Count)
            {
                SelectedFunction = FunctionCodes[index];
            }
        }
    }
}
