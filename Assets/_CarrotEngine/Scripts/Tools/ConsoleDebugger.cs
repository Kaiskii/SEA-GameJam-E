using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CarrotEngine
{
    public static class ConsoleDebugger
    {
        public static void Log(string debug, object source = null)
        {
#if UNITY_EDITOR
            debug = DebugSource(debug, source);

            Debug.Log(debug);
#endif
        }

        public static void LogFormat(string debug, object source, params object[] args)
        {
#if UNITY_EDITOR
            debug = DebugSource(debug, source);

            Debug.LogFormat(debug, args);
#endif
        }

        public static void LogWarning(string debug, object source = null)
        {
#if UNITY_EDITOR
            debug = DebugSource(debug, source);

            Debug.LogWarning(debug);
#endif
        }

        public static void LogWarningFormat(string debug, object source, params object[] args)
        {
#if UNITY_EDITOR
            debug = DebugSource(debug, source);

            Debug.LogWarningFormat(debug, args);
#endif
        }

        public static void LogError(string debug, object source = null)
        {
#if UNITY_EDITOR
            debug = DebugSource(debug, source);

            Debug.LogError(debug);
#endif
        }

        public static void LogErrorFormat(string debug, object source, params object[] args)
        {
#if UNITY_EDITOR
            debug = DebugSource(debug, source);

            Debug.LogErrorFormat(debug, source, args);
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