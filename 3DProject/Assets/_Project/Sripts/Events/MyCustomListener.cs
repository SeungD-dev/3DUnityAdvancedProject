using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MyCustomListener : MonoBehaviour, IListener
    {
        public void OnEvent(EventType EventType, Component Sender, object Param = null)
        {
            
        }
    }
}
