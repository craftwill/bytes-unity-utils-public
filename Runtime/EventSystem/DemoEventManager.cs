using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bytes
{
    public class DemoEventManager : MonoBehaviour
    {
        private string demoEventName = "DemoEvent";

        private void Start()
        {
            EventManager.AddEventListener(demoEventName, HandleOnPressE);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Usually this would be called in another extern Script, 
                //  since this is the purpose of the Event system.
                EventManager.Dispatch(demoEventName, new DemoData(3));
            }
        }

        private void HandleOnPressE(BytesData data)
        {
            var demoData = (data as DemoData);

            print("Custom data Number = " + demoData.Number);
        }
    }

    public class DemoData : BytesData
    {
        public DemoData(int number) { Number = number; }
        public int Number { get; private set; }
    }
}