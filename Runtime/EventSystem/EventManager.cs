using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes
{
    public class EventManager : MonoBehaviour
    {
        private static EventManager Instance;

        private Dictionary<string, List<Action<BytesData>>> _eventListeners = new Dictionary<string, List<Action<BytesData>>>();

        private void Awake()
        {
            Instance = this;
        }

        public static void RemoveEventListener(string eventName, Action<BytesData> functionToCall)
        {
            Instance._eventListeners[eventName].Remove(functionToCall);
        }

        public static void AddEventListener(string eventName, Action<BytesData> functionToCall)
        {
            if (Instance._eventListeners.ContainsKey(eventName))
            {
                Instance._eventListeners[eventName].Add(functionToCall);
                return;
            }
            Instance._eventListeners.Add(eventName, new List<Action<BytesData>>() { functionToCall });
        }

        public static void Dispatch(string eventName, BytesData data)
        {
            if (!Instance._eventListeners.ContainsKey(eventName))
            {
                return;
            }
            Instance.CleanEventsFromNull(eventName);
            List<Action<BytesData>> functions = new List<Action<BytesData>>(Instance._eventListeners[eventName]);
            foreach (Action<BytesData> functionToCall in functions)
            {
                functionToCall(data);
            }
        }

        private void CleanEventsFromNull(string eventName)
        {
            Instance._eventListeners[eventName].RemoveAll(item => item == null);
        }
    }
}