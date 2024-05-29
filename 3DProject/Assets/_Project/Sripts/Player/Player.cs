using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class Player : MonoBehaviour
    {
       public PlayerController controller;
        //public PlayerStatus status;

        //public ItemData itemData;
        public Action addItem;

        private void Awake()
        {
            CharacterManager.Instance.player = this;
            controller = GetComponent<PlayerController>();
            //status
        }

    }
}
