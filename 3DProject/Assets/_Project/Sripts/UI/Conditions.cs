using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Platformer
{
    public class Conditions : MonoBehaviour
    {
        public float curValue;
        public float maxValue;
        public float startValue;
        public float passiveValue;
        public Image uiBar;

        private void Start()
        {
            curValue = startValue;
        }

        private void Update()
        {
            uiBar.fillAmount = GetPercentage();
        }

        public void Add(float amount)
        {
            curValue = Mathf.Min(curValue + amount, maxValue);
        }

        public void Subtract(float amount)
        {
            curValue = Mathf.Max(curValue - amount, 0.0f);
        }

        public float GetPercentage()
        {
            return curValue / maxValue;
        }

    }
}
