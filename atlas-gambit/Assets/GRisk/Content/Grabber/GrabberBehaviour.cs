using GRisk.Interaction.Item;
using GRisk.Interface;
using TMPro;
using UnityEngine;

namespace GRisk.Content.Grabber
{
    public class GrabberBehaviour : ItemBehaviour
    {
        public GrabberConsumable head;
        public TMP_Text moveAmountLabel;
        public GRSound soundPlayer;

        public uint moveAmount = 10u;
        public uint moveIncrement = 10u;


        private void Start()
        {
            updateMoveAmount();
        }

        public override void onTrigger()
        {
            head.applyFocus = true;
        }

        public override void offTrigger()
        {
            head.applyFocus = false;
            head.releaseFocus();
            
            soundPlayer.playsound("drag-select", gameObject.transform, 2.0f);
        }

        public override void onPrimary()
        {
            if (moveAmount < moveIncrement) return;

            moveAmount -= moveIncrement;
            updateMoveAmount();
        }

        public override void onSecondary()
        {
            moveAmount += moveIncrement;
            updateMoveAmount();
        }

        private void updateMoveAmount()
        {
            head.moveAmount = moveAmount;
            moveAmountLabel.SetText(NumberLabel.formatCompact(moveAmount));
        }
    }
}