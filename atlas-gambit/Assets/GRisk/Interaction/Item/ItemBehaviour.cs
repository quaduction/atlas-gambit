using UnityEngine;

namespace GRisk.Interaction.Item
{
    public class ItemBehaviour: MonoBehaviour
    {
        public virtual void onTrigger() {}
        public virtual void offTrigger() {}
        public virtual void onPrimary() {}
        public virtual void onSecondary() {}
    }
}