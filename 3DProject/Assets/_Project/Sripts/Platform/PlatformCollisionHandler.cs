using UnityEngine;

namespace Platformer
{
    public class PlatformCollisionHandler : MonoBehaviour 
    {
        Transform platform; //오브젝트가 서 있을 플랫폼

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                //만약 contact.normal이 위를 가리키고 있다면 플랫폼 윗 부분과 충돌한 것
                ContactPoint contact = collision.GetContact(0);
                if (contact.normal.y < 0.5f) return;

                platform = collision.transform;
                transform.SetParent(platform);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.CompareTag("MovingPlatform"))
            {
                transform.SetParent(null);
                platform = null;
            }
        }
    }
}
