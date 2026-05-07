using GRisk.Interaction.Item;

namespace GRisk.Content.Grabber
{
    public class GrabberBehaviour : ItemBehaviour
    {
        public ConsumableItem head;

        public override void offTrigger()
        {
            head.releaseFocus();
        }
    }
}