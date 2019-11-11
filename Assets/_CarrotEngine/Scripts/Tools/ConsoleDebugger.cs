using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarrotEngine
{
    public static class ConsoleDebugger
    {
        public static void Log(string debug)
        {
#if UNITY_EDITOR
            Debug.Log(debug);
#endif
        }

        public static void LogFormat(string debug, params object[] args)
        {
#if UNITY_EDITOR
            Debug.LogFormat(debug, args);
#endif
        }

        public static void LogWarning(string debug)
        {
#if UNITY_EDITOR
            Debug.LogWarning(debug);
#endif
        }

        public static void LogWarningFormat(string debug,  params object[] args)
        {
#if UNITY_EDITOR
            Debug.LogWarningFormat(debug, args);
#endif
        }

        public static void LogError(string debug)
        {
#if UNITY_EDITOR
            Debug.LogError(debug);
#endif
        }

        public static void LogErrorFormat(string debug, params object[] args)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat(debug, args);
#endif
        }

        private static string DebugSource(string debug, object source)
        {
            if (source != null)
            {
                debug = string.Format("(Source: {0}) ", source.ToString()) + debug;
            }

            return debug;
        }
    }
}