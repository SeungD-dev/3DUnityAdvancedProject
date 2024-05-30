using UnityEngine;
using KBCore.Refs;
using Cinemachine;
using Unity.VisualScripting;
using System;
using System.Collections;

namespace Platformer
{
    public class CameraManager : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField, Anywhere] InputReader input;
        [SerializeField, Anywhere] CinemachineFreeLook freeLookVCam;

        [Header("Settings")]
        [SerializeField, Range(0.5f, 3f)] float speedMuliplier = 1f;

        bool isRMBPressed;
        bool cameraMovementLock;

        private void OnEnable()
        {
            input.Look += OnLook;
            input.EnableMouseControlCamera += OnEnableMouseControlCamera;
            input.DisableMouseControlCamera += OnDisableMouseControlCamera;
        }

        private void OnDisable()
        {
            input.Look -= OnLook;
            input.EnableMouseControlCamera -= OnEnableMouseControlCamera;
            input.DisableMouseControlCamera -= OnDisableMouseControlCamera;
        }

        private void OnLook(Vector2 cameraMovement, bool isDeviceMouse)
        {
            if (cameraMovementLock) return;

            if (isDeviceMouse && !isRMBPressed) return;

            //만약 device가 마우스면 fixedDeltaTime을 사용하고, 아니면 deltaTime을 사용
            float deviceMultiplier = isDeviceMouse ? Time.fixedDeltaTime : Time.deltaTime;

            //카메라의 축 설정
            freeLookVCam.m_XAxis.m_InputAxisValue = cameraMovement.x * speedMuliplier * deviceMultiplier;
            freeLookVCam.m_YAxis.m_InputAxisValue = cameraMovement.y * speedMuliplier * deviceMultiplier;
        }

        private void OnEnableMouseControlCamera()
        {
            isRMBPressed = true;

            //우클릭 시 커서 가리고 잠그기
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            StartCoroutine(DisableMouseForFrame());
        }

        private void OnDisableMouseControlCamera()
        {
            isRMBPressed = false;

            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            
            freeLookVCam.m_XAxis.m_InputAxisValue = 0f;
            freeLookVCam.m_YAxis.m_InputAxisValue = 0f;
        }

        IEnumerator DisableMouseForFrame()
        {
            cameraMovementLock = true;
            yield return new WaitForEndOfFrame();
            cameraMovementLock = false;
        }

     
    }
}
