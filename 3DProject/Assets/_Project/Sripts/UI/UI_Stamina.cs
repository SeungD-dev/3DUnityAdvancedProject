using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class UI_Stamina : MonoBehaviour, IListener
    {
        [SerializeField] private Conditions stamina;

        void Start()
        {
            EventManager.Instance.AddListener(EventType.eStaminaChange, this);
        }

        public void OnEvent(EventType EventType, Component Sender, object Param = null)
        {
            
        }
    }
}
