using UnityEngine;

namespace GRisk.Interaction.Item
{
    public abstract class ItemBehaviour: MonoBehaviour
    {
        public virtual void onTrigger() {}
        public virtual void onPrimary() {}
        public virtual void onSecondary() {}
    }
}