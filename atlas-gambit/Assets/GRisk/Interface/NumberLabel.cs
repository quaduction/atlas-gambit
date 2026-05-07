using System;
using TMPro;
using UnityEngine;

namespace GRisk.Interface
{
    public class NumberLabel : MonoBehaviour
    {
        public float tiltAngle = 80.0f;
        public bool lockHorizontal = true;

        public TMP_Text label;
        private Transform labelTransform;

        private Camera vrCamera;
        private Vector3 lastCamPos;

        private bool forceUpdate = true;


        private void Start()
        {
            // label = GetComponent<TMP_Text>();
            labelTransform = label.transform;

            vrCamera = Camera.main;

            lastCamPos = vrCamera != null ? vrCamera.transform.position : Vector3.zero;
            updateRotation();
        }

        private void LateUpdate()
        {
            if (forceUpdate)
            {
                updateRotation();
            }
            else
            {
                forceUpdate = false;
            }
        }

        private void updateRotation()
        {
            if (vrCamera == null) return;

            Vector3 direction = -(vrCamera.transform.position - labelTransform.position);

            if (lockHorizontal)
            {
                direction.y = 0f;
                if (direction.sqrMagnitude < 0.0001f) direction = Vector3.forward;
            }

            // Base rotation facing camera (or its horizontal projection)
            Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);

            // Apply fixed tilt around local X axis
            targetRot *= Quaternion.Euler(tiltAngle, 0f, 0f);

            // Direct assignment for crisp VR response. Use Slerp if you prefer smoothing.
            labelTransform.rotation = targetRot;
        }

        public void setValue(uint value)
        {
            label.SetText(formatCompact(value));
            forceUpdate = true;
        }

        public static string formatCompact(uint value)
        {
            return value switch
            {
                >= 1000000u => formatCondDecimal(value / 1000000f) + "M",
                >= 1000u => formatCondDecimal(value / 1000f) + "K",
                _ => value.ToString()
            };
        }

        private static string formatCondDecimal(float num)
        {
            // Only show .0 if it's not a clean integer
            return Mathf.Approximately(num, Mathf.Round(num))
                ? Mathf.Round(num).ToString("0")
                : num.ToString("0.0");
        }
    }
}