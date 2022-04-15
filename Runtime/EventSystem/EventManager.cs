using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes
{
    public class EventManager : MonoBehaviour
    {
        private static EventManager Instance;

        private Dictionary<string, List<Action<Data>>> _eventListeners = new Dictionary<string, List<Action<Data>>>();

        private void Awake()
        {
            Instance = this;
        }

        public static void RemoveEventListener(string eventName, Action<Data> functionToCall)
        {
            Instance._eventListeners[eventName].Remove(functionToCall);
        }

        public static void AddEventListener(string eventName, Action<Data> functionToCall)
        {
            if (Instance._eventListeners.ContainsKey(eventName))
            {
                Instance._eventListeners[eventName].Add(functionToCall);
                return;
            }
            Instance._eventListeners.Add(eventName, new List<Action<Data>>() { functionToCall });
        }

        public static void Dispatch(string eventName, Data data)
        {
            if (!Instance._eventListeners.ContainsKey(eventName))
            {
                return;
            }
            Instance.CleanEventsFromNull(eventName);
            List<Action<Data>> functions = new List<Action<Data>>(Instance._eventListeners[eventName]);
            foreach (Action<Data> functionToCall in functions)
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