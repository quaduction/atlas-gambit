using System;
using GRisk.Data;
using GRisk.Engine;
using GRisk.Interaction.Item;
using UnityEngine;
using UnityEngine.Events;

namespace GRisk.Interface
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(MeshCollider))]
    public class TerritoryTile : MonoBehaviour
    {
        public string territoryId;
        public TileManager manager;

        private new MeshRenderer renderer;
        private MeshCollider meshCollider;

        private uint manpower = 0;
        private uint owner = (uint)GRTypes.Player.NONE;
        private bool focused = false;

        private void Reset()
        {
            territoryId = name;
        }

        private void Awake()
        {
            renderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();
        }

        private void Start()
        {
            manager.registerTile(this);
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out ConsumableItem consumable))
            {
                manager.onConsumable(this, consumable);
            }
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

        public void updateVisuals()
        {
            renderer.material.color = GRData.playerStyleDict[owner].color;
        }
    }
}