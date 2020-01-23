﻿using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ZpaPlugin
{
    delegate string IdeGetText();

    public class ZpaPlugin
    {
        public static readonly string dependenciesPath = Path.Combine(Path.GetDirectoryName(typeof(ZpaPlugin).Assembly.Location), "ZPA");

        private const string PLUGIN_NAME = "Z PL/SQL Analyzer";

        private const int PLUGIN_MENU_INDEX = 1;

        private const int GET_TEXT_CALLBACK = 30;

        private static ZpaPlugin self;
        private static IdeGetText getTextCallback;

        [DllImport("msvcrt.dll", CallingConvention = CallingConvention.Cdecl)]
        private static extern int _fpreset();

        private ZpaPlugin()
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(ResolveAssembly);
            _fpreset(); // Fixes "ArithmeticException: Overflow or underflow in the arithmetic operation." when loading the WPF form
        }

        private Assembly ResolveAssembly(object sender, ResolveEventArgs args)
        {
            var assemblyName = new AssemblyName(args.Name).Name;
            if (assemblyName.EndsWith(".resources")) return null;
            return Assembly.Load(AssemblyName.GetAssemblyName(Path.Combine(dependenciesPath, $"{assemblyName}.dll")));
        }

        [DllExport("IdentifyPlugIn", CallingConvention = CallingConvention.Cdecl)]
        public static string IdentifyPlugIn(int id)
        {
            if (self == null)
            {
                self = new ZpaPlugin();
            }
            return PLUGIN_NAME;
        }

        [DllExport("RegisterCallback", CallingConvention = CallingConvention.Cdecl)]
        public static void RegisterCallback(int index, IntPtr function)
        {
            switch (index)
            {
                case GET_TEXT_CALLBACK:
                    getTextCallback = Marshal.GetDelegateForFunctionPointer<IdeGetText>(function);
                    break;
                default:
                    break;
            }
        }

        [DllExport("CreateMenuItem", CallingConvention = CallingConvention.Cdecl)]
        public static string CreateMenuItem(int index)
        {
            if (index == PLUGIN_MENU_INDEX)
            {
                return "Tools / Analyze with ZPA";
            }
            return "";
        }

        [DllExport("OnMenuClick", CallingConvention = CallingConvention.Cdecl)]
        public static void OnMenuClick(int index)
        {
            if (index == PLUGIN_MENU_INDEX)
            {
                var runner = new ZpaRunner();
                runner.Analyze(getTextCallback());
            }
        }

        [DllExport("About", CallingConvention = CallingConvention.Cdecl)]
        public static string About() =>  "Z PL/SQL Analyzer";
    }
}
