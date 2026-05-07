using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

namespace GRisk.Interaction.Item
{
    [RequireComponent(typeof(XRGrabInteractable))]
    [RequireComponent(typeof(ItemBehaviour))]
    public class ItemInputRouter : MonoBehaviour
    {
        public InputActionProperty[] primaries = new InputActionProperty[2];
        public InputActionProperty[] secondaries = new InputActionProperty[2];

        private XRGrabInteractable grab;
        private ItemBehaviour behaviour;

        private XRBaseInteractor currentInteractor;
        private ActionBasedController controller;

        private void Awake()
        {
            grab = GetComponent<XRGrabInteractable>();
            behaviour = GetComponent<ItemBehaviour>();

            grab.selectEntered.AddListener(OnGrab);
            grab.selectExited.AddListener(OnRelease);

            grab.activated.AddListener(OnActivate);
            grab.deactivated.AddListener(OnDeactivate);

            Array.ForEach(primaries, iap => iap.action.performed += OnPrimary);
            Array.ForEach(secondaries, iap => iap.action.performed += OnSecondary);
        }

        private void OnDestroy()
        {
            Array.ForEach(primaries, iap => iap.action.performed -= OnPrimary);
            Array.ForEach(secondaries, iap => iap.action.performed -= OnSecondary);
        }

        private void OnGrab(SelectEnterEventArgs args)
        {
            currentInteractor = args.interactorObject as XRBaseInteractor;

            if (currentInteractor == null) return;

            controller = currentInteractor.transform.parent.GetComponent<ActionBasedController>();
        }

        private void OnRelease(SelectExitEventArgs args)
        {
            currentInteractor = null;
        }

        private void OnActivate(ActivateEventArgs args)
        {
            behaviour.onTrigger();
        }

        private void OnDeactivate(DeactivateEventArgs args)
        {
            behaviour.offTrigger();
        }

        private bool deviceMatching(InputAction.CallbackContext context)
        {
            if (controller == null) return false;

            return context.action.activeControl.device == controller.positionAction.action.activeControl.device;
        }

        private void OnPrimary(InputAction.CallbackContext context)
        {
            if (!deviceMatching(context)) return;

            Debug.Log("OnPrimary");

            behaviour.onPrimary();
        }

        private void OnSecondary(InputAction.CallbackContext context)
        {
            if (!deviceMatching(context)) return;

            Debug.Log("OnSecondary");

            behaviour.onSecondary();
        }
    }
}