using System;
using System.Reflection;
using System.Resources;

namespace ModbusCalc.Localizations
{
    public class Resources
    {
        private static ResourceManager resourceManager;
        
        static Resources()
        {
            resourceManager = new ResourceManager("ModbusCalc.Localizations.Resources", Assembly.GetExecutingAssembly());
        }
        
        public static string DefaultTheme => resourceManager.GetString("DefaultTheme");
        public static string DarkTheme => resourceManager.GetString("DarkTheme");
        public static string LightTheme => resourceManager.GetString("LightTheme");
        public static string FunctionCode1 => resourceManager.GetString("FunctionCode1");
        public static string FunctionCode2 => resourceManager.GetString("FunctionCode2");
        public static string FunctionCode3 => resourceManager.GetString("FunctionCode3");
        public static string FunctionCode4 => resourceManager.GetString("FunctionCode4");
        public static string FunctionCode5 => resourceManager.GetString("FunctionCode5");
        public static string FunctionCode6 => resourceManager.GetString("FunctionCode6");
        public static string FunctionCode7 => resourceManager.GetString("FunctionCode7");
        public static string FunctionCode8 => resourceManager.GetString("FunctionCode8");
        public static string DefaultFunction => resourceManager.GetString("DefaultFunction");
        public static string Separator => resourceManager.GetString("Separator");
        public static string NoFunctionSelected => resourceManager.GetString("NoFunctionSelected");
        public static string 功能码_Label => resourceManager.GetString("功能码_Label");
        public static string 从站地址_Label => resourceManager.GetString("从站地址_Label");
        public static string 起始地址_Label => resourceManager.GetString("起始地址_Label");
        public static string 数据值_Label => resourceManager.GetString("数据值_Label");
        public static string 生成结果_Label => resourceManager.GetString("生成结果_Label");
    }
}
