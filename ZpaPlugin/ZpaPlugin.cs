using System;
using System.Runtime.InteropServices;

namespace ZpaPlugin
{
    public class ZpaPlugin
    {
        private const string PLUGIN_NAME = "Z PL/SQL Analyzer";

        private const int PLUGIN_MENU_INDEX = 1;

        private static ZpaPlugin self;
        private int pluginId;

        //TODO: declare private delegate variable (not necessarilly static), for instance
        //private static IdeCreateWindow createWindowCallback;

        private ZpaPlugin(int id)
        {
            pluginId = id;
        }

        [DllExport("IdentifyPlugIn", CallingConvention = CallingConvention.Cdecl)]
        public static string IdentifyPlugIn(int id)
        {
            if (self == null)
            {
                self = new ZpaPlugin(id);
            }
            return PLUGIN_NAME;
        }

        [DllExport("RegisterCallback", CallingConvention = CallingConvention.Cdecl)]
        public static void RegisterCallback(int index, IntPtr function)
        {
            //TODO: register pointers to PL/SQL Developer callbacks you need, for instance
            //createWindowCallback = (IdeCreateWindow)Marshal.GetDelegateForFunctionPointer(function, typeof(IdeCreateWindow));
        }

        [DllExport("CreateMenuItem", CallingConvention = CallingConvention.Cdecl)]
        public static string CreateMenuItem(int index)
        {
            if (index == PLUGIN_MENU_INDEX)
            {
                return "Tools / Analyze with ZPA";
            }
            else
            {
                return "";
            }
        }

        [DllExport("OnMenuClick", CallingConvention = CallingConvention.Cdecl)]
        public static void OnMenuClick(int index)
        {
            if (index == PLUGIN_MENU_INDEX)
            {
                //TODO: do something when plug-in's menu is clicked.
            }
        }

        [DllExport("About", CallingConvention = CallingConvention.Cdecl)]
        public static string About() =>  "Z PL/SQL Analyzer";
    }
}
