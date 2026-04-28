using GRisk.Interaction.Item;
using UnityEngine;

namespace GRisk.Content.DebugStick
{
    public class DebugStickBehaviour : ItemBehaviour
    {
        public GameObject triggerLight;
        public GameObject aLight;
        public GameObject bLight;
        
        private Renderer triggerRenderer;
        private Renderer aRenderer;
        private Renderer bRenderer;

        private void Start()
        {
            triggerRenderer = triggerLight.GetComponent<Renderer>();
            aRenderer = aLight.GetComponent<Renderer>();
            bRenderer = bLight.GetComponent<Renderer>();
        }

        private void colorise(Renderer lrenderer, Color color)
        {
            lrenderer.material.color = color;
        }

        private void toggleColor(Renderer lrenderer,  Color color)
        {
            lrenderer.material.color = lrenderer.material.color == Color.black ? color : Color.black;
        }
        
        public override void onTrigger(){
            Debug.Log("[debug stick] trigger");
            toggleColor(triggerRenderer, Color.green);
        }
        
        public override void onPrimary(){
            Debug.Log("[debug stick] A");
            toggleColor(aRenderer, Color.red);
        }
        
        public override void onSecondary(){
            Debug.Log("[debug stick] B");
            toggleColor(bRenderer, Color.blue);
        }
    }
}