using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace GRisk.Interaction.Item
{
    public class ItemInputRouter : MonoBehaviour
    {
        private XRGrabInteractable grab;
        private ItemBehaviour behaviour;

        private XRBaseInteractor currentInteractor;
        private ActionBasedController controller;

        void Awake()
        {
            grab = GetComponent<XRGrabInteractable>();
            behaviour = GetComponent<ItemBehaviour>();

            grab.selectEntered.AddListener(OnGrab);
            grab.selectExited.AddListener(OnRelease);
            
            grab.activated.AddListener(OnActivate);
        }

        void OnGrab(SelectEnterEventArgs args)
        {
            currentInteractor = args.interactorObject as XRBaseInteractor;
            Debug.Log("OnGrab");
            Debug.Log(currentInteractor.transform.parent.name);
            
            if (currentInteractor == null) return;
            
            controller = currentInteractor.GetComponent<ActionBasedController>();
            
            Debug.Log(controller);
        }

        void OnRelease(SelectExitEventArgs args)
        {
            currentInteractor = null;
        }

        public void OnActivate(ActivateEventArgs args)
        {
            behaviour.onTrigger();
        }

        // void Update()
        // {
        //     if (currentInteractor == null || behaviour == null) return;
        //     
        //
        //     if (controller == null) return;
        //     
        //     controller.
        //
        //     if (controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out bool trigger))
        //     {
        //         if (trigger) behaviour.onTrigger();
        //     }
        //
        //     if (controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out bool a))
        //     {
        //         if (a) behaviour.onPrimary();
        //     }
        //
        //     if (controller.inputDevice.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out bool b))
        //     {
        //         if (b) behaviour.onSecondary();
        //     }
        // }
    }
}