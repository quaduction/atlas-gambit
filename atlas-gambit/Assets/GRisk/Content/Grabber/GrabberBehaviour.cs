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

        public uint moveAmount = 0u;
        public uint moveIncrement = 10u;


        public override void onTrigger()
        {
            head.applyFocus = true;
        }

        public override void offTrigger()
        {
            head.applyFocus = false;
            head.releaseFocus();
        }

        public override void onPrimary()
        {
            moveAmount += moveIncrement;
            updateMoveAmount();
        }

        public override void onSecondary()
        {
            if (moveAmount < moveIncrement) return;

            moveAmount -= moveIncrement;
            updateMoveAmount();
        }

        private void updateMoveAmount()
        {
            head.moveAmount = moveAmount;
            moveAmountLabel.SetText(NumberLabel.formatCompact(moveAmount));
        }
    }
}