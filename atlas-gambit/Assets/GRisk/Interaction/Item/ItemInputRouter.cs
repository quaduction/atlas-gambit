using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace GRisk.Interaction.Item
{
    public class ItemInputRouter : MonoBehaviour
    {
        private XRGrabInteractable grab;
        private ItemBehaviour behaviour;

        private XRBaseInteractor currentInteractor;

        void Awake()
        {
            grab = GetComponent<XRGrabInteractable>();
            behaviour = GetComponent<ItemBehaviour>();

            grab.selectEntered.AddListener(OnGrab);
            grab.selectExited.AddListener(OnRelease);
        }

        void OnGrab(SelectEnterEventArgs args)
        {
            currentInteractor = args.interactorObject as XRBaseInteractor;
        }

        void OnRelease(SelectExitEventArgs args)
        {
            currentInteractor = null;
        }

        void Update()
        {
            if (currentInteractor == null || behaviour == null) return;

            XRController controller = currentInteractor.GetComponent<XRController>();

            if (controller == null) return;

            if (controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool trigger))
            {
                if (trigger) behaviour.onTrigger();
            }

            if (controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool a))
            {
                if (a) behaviour.onPrimary();
            }

            if (controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool b))
            {
                if (b) behaviour.onSecondary();
            }
        }
    }
}