using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Platformer
{
    public class UI_Stamina : MonoBehaviour
    {
        [SerializeField] Image staminaBar;
        [SerializeField] FloatEventChannel playerStaminaChannel;


        public void UpdateStaminaBar(float amount)
        {
            StartCoroutine(DecreaseStaminaBar(amount));
        }

        IEnumerator RestoreStaminaBar(float amount)
        {
            yield return new WaitForSeconds(amount);
        }

        IEnumerator DecreaseStaminaBar(float amount)
        {
            while (staminaBar.fillAmount > amount)
            {
                staminaBar.fillAmount -= Time.deltaTime * amount;
                yield return null;
            }
        }
    }
}
