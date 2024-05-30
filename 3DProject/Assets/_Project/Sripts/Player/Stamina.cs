using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class Stamina : MonoBehaviour
    {
        [SerializeField] float maxStamina = 100.0f;
        [SerializeField] FloatEventChannel playerStaminaChannel;
        InputReader input;

        float currentStamina;

        private void Awake()
        {
            currentStamina = maxStamina;
        }

        private void Start()
        {
            PublishStaminaPercentage();
        }

        public void RestoreStamina(float amount)
        {
            if(currentStamina == maxStamina)
            {
                return;
            }
            currentStamina = Mathf.Min(currentStamina + amount, maxStamina);
        }

        //대시할 때 스태미나 감소
        public void DecreaseStamina(float amount)
        {
            if(currentStamina - amount < 0)
            {
                return;
            }
            currentStamina = Mathf.Max(currentStamina - amount, 0.0f);
            
        }



        public void PublishStaminaPercentage()
        {
            if(playerStaminaChannel!=null)
                playerStaminaChannel.Invoke(currentStamina / (float)maxStamina);
        }
    }
}
