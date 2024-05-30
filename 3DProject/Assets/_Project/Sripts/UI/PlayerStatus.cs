using UnityEngine;

namespace Platformer
{
    public interface IDamagalbe
    {
        void TakePhysicalDamage(int damage);
    }

    public class PlayerStatus : MonoBehaviour, IDamagalbe
    {
        public UIConditions UIConditions;

        Conditions health { get { return UIConditions.health; } }
        Conditions stamina { get { return UIConditions.stamina; } }


        private void Update()
        {
            stamina.Add(stamina.passiveValue * Time.deltaTime);
        }

        public bool UseStamina(float amount)
        {
            if(stamina.curValue - amount < 0)
            {
                return false;
            }
            stamina.Subtract(amount);
            return true;
        }


        public void TakePhysicalDamage(int damage)
        {
            
        }
    }
}
