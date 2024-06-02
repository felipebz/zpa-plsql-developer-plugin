using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZpaPlugin
{
    delegate IntPtr IdeGetText();

    [return: MarshalAs(UnmanagedType.Bool)]
    delegate bool IdeSetError(int line, int column);

    delegate void IdeClearError();

    public class ZpaPlugin : IPlsqlDevApi
    {
        public static readonly string dependenciesPath = Path.Combine(AppContext.BaseDirectory, "Plugins", "ZPA");

        private const string PLUGIN_NAME = "Z PL/SQL Analyzer";

        private const int PLUGIN_MENU_INDEX = 1;

        private const int GET_TEXT_CALLBACK = 30;
        private const int SET_ERROR_CALLBACK = 36;
        private const int CLEAR_ERROR_CALLBACK = 37;

        private static ZpaPlugin self;
        private static IdeGetText getTextCallback;
        private static IdeSetError setErrorCallback;
        private static IdeClearError clearErrorCallback;

        [UnmanagedCallersOnly(EntryPoint = "IdentifyPlugIn", CallConvs = [typeof(CallConvCdecl)])]
        public static IntPtr IdentifyPlugIn(int id)
        {
            self ??= new ZpaPlugin();

            return Marshal.StringToCoTaskMemAnsi(PLUGIN_NAME);
        }

        [UnmanagedCallersOnly(EntryPoint = "RegisterCallback", CallConvs = [typeof(CallConvCdecl)])]
        public static void RegisterCallback(int index, IntPtr function)
        {
            switch (index)
            {
                case GET_TEXT_CALLBACK:
                    getTextCallback = Marshal.GetDelegateForFunctionPointer<IdeGetText>(function);
                    break;
                case SET_ERROR_CALLBACK:
                    setErrorCallback = Marshal.GetDelegateForFunctionPointer<IdeSetError>(function);
                    break;
                case CLEAR_ERROR_CALLBACK:
                    clearErrorCallback = Marshal.GetDelegateForFunctionPointer<IdeClearError>(function);
                    break;
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "CreateMenuItem", CallConvs = [typeof(CallConvCdecl)])]
        public static IntPtr CreateMenuItem(int index)
        {
            var retValue = "";
            if (index == PLUGIN_MENU_INDEX)
            {
                retValue = "Tools / Analyze with ZPA";
            }

            return Marshal.StringToCoTaskMemAnsi(retValue);
        }

        [UnmanagedCallersOnly(EntryPoint = "OnMenuClick", CallConvs = [typeof(CallConvCdecl)])]
        public static void OnMenuClick(int index)
        {
            if (index == PLUGIN_MENU_INDEX)
            {
                var guiPath = Path.Combine(dependenciesPath, "ZpaPlugin.Gui.exe");
                Process.Start(guiPath);
            }
        }

        [UnmanagedCallersOnly(EntryPoint = "About", CallConvs = [typeof(CallConvCdecl)])]
        public static IntPtr About() => Marshal.StringToCoTaskMemAnsi("Z PL/SQL Analyzer");

        public bool SetError(int line, int column)
        {
            return setErrorCallback?.Invoke(line, column + 1) ?? false;
        }

        public void ClearError()
        {
            clearErrorCallback();
        }
    }
}