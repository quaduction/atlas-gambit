using System;
using GRisk.Interaction.Item;

namespace GRisk.Content.Grabber
{
    public class GrabberConsumable : ConsumableItem
    {
        private void Reset()
        {
            destroyOnConsume = false;
            applyFocus = true;
            focusLock = true;
        }
    }
}