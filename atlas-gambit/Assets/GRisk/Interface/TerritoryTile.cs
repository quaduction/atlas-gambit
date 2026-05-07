using System;
using GRisk.Engine;
using GRisk.Interaction.Item;
using GRisk.Util;
using UnityEngine;

namespace GRisk.Interface
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    [RequireComponent(typeof(Rigidbody))]
    public class TerritoryTile : MonoBehaviour
    {
        public string territoryId;
        public TileManager manager;
        public NumberLabel tokenPrefab;

        private new MeshRenderer renderer;
        private Rigidbody rigidBody;
        private NumberLabel token;
        private Renderer tokenRenderer;

        private uint manpower = 0;
        private uint owner = (uint)GRTypes.Player.NONE;
        private bool focused = false;

        private void Reset()
        {
            territoryId = name;

            rigidBody = GetComponent<Rigidbody>();
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
        }

        private void Awake()
        {
            renderer = GetComponent<MeshRenderer>();


            token = Instantiate(tokenPrefab);


            // token.transform.localScale = transform.lossyScale * 0.6f;
            // token.transform.parent = transform;


            tokenRenderer = token.GetComponentInChildren<Renderer>();

            Vector3 tokenSize = tokenRenderer.bounds.size;

            float tokenTrySize = Math.Min(
                (renderer.bounds.size.x + renderer.bounds.size.z) / 2 * 0.5f,
                0.1f
            );


            GRThings.absSizerOne(token.gameObject, tokenTrySize);

            // if (territoryId == "TAI")
            // {
            //     Debug.Log($"SCALES local: {transform.localScale} lossy: {transform.lossyScale}");
            //     Debug.Log($"SIZE: {renderer.bounds.size}");
            //     Debug.Log(
            //         $"TOKENTRYSIZE: {tokenTrySize} (current: {tokenSize.x}, factor: {tokenTrySize / tokenSize.x})");
            //     Debug.Log($"REALSULT: {tokenRenderer.bounds.size.x}");
            // }

            token.transform.position = renderer.bounds.center + new Vector3(
                0,
                renderer.bounds.size.y / 2 + tokenSize.y / 2,
                0
            );
        }

        private void Start()
        {
            manager.registerTile(this);
        }

        private void collide(GameObject detectedObject)
        {
            if (detectedObject.TryGetComponent(out ConsumableItem consumable))
            {
                manager.onConsumable(this, consumable);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            collide(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            collide(other.gameObject);
        }

        public void setState(uint[] state)
        {
            manpower = state[0];
            owner = state[1];

            updateVisuals();
        }

        public void setFocus(bool focus)
        {
            focused = focus;

            updateVisuals();
        }

        public bool getFocus()
        {
            return focused;
        }

        public void updateVisuals()
        {
            float brighten = focused ? 0.4f : 0f;

            GRThings.ownerColorize(renderer, owner, 1f + brighten);
            GRThings.ownerColorize(tokenRenderer, owner, 0.6f + brighten);

            token.setValue(manpower);
        }
    }
}