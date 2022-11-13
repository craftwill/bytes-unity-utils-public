using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes
{
    public class EventManager : MonoBehaviour
    {
        public const string AllNamespace = "All";
        private static EventManager Instance;

        private Dictionary<string, Dictionary<string, List<Action<BytesData>>>> _eventListeners = new Dictionary<string, Dictionary<string, List<Action<BytesData>>>>();

        private void Awake()
        {
            Instance = this;
        }

        public static void RemoveEventListener(string pEventName, Action<BytesData> pFunctionToCall)
        {
            RemoveEventListener(AllNamespace, pEventName, pFunctionToCall);
        }

        public static void RemoveEventListener(string pEventNamespace, string pEventName, Action<BytesData> pFunctionToCall)
        {
            Instance._eventListeners[pEventNamespace][pEventName].Remove(pFunctionToCall);
        }

        public static void AddEventListener(string pEventName, Action<BytesData> pFunctionToCall)
        {
            AddEventListener(AllNamespace, pEventName, pFunctionToCall);
        }

        public static void AddEventListener(string pEventNamespace, string pEventName, Action<BytesData> pFunctionToCall)
        {
            if (!Instance._eventListeners.ContainsKey(pEventNamespace))
            {
                Instance._eventListeners[pEventNamespace] = new Dictionary<string, List<Action<BytesData>>>();
            }

            if (Instance._eventListeners[pEventNamespace].ContainsKey(pEventName))
            {
                Instance._eventListeners[pEventNamespace][pEventName].Add(pFunctionToCall);
                return;
            }
            Instance._eventListeners[pEventNamespace].Add(pEventName, new List<Action<BytesData>>() { pFunctionToCall });
        }

        public static void Dispatch(string pEventName, BytesData pBytesData = null)
        {
            Dispatch(AllNamespace, pEventName, pBytesData);
        }

        public static void Dispatch(string pEventNamespace, string pEventName, BytesData pBytesData = null)
        {
            if (!Instance._eventListeners[pEventNamespace].ContainsKey(pEventName))
            {
                return;
            }
            Instance.CleanEventsFromNull(pEventNamespace, pEventName);
            List<Action<BytesData>> functions = new List<Action<BytesData>>(Instance._eventListeners[pEventNamespace][pEventName]);
            foreach (Action<BytesData> functionToCall in functions)
            {
                functionToCall(pBytesData);
            }
        }

        private void CleanEventsFromNull(string pEventNamespace, string pEventName)
        {
            Instance._eventListeners[pEventNamespace][pEventName].RemoveAll(item => item == null);
        }
    }
}