using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    eHealthChange,
    eStaminaChange
}

namespace Platformer
{
    public interface IListener
    {
        void OnEvent(EventType EventType, Component Sender, object Param = null);
    }
}
