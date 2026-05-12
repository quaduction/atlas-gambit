using System;
using GRisk.Engine;
using GRisk.Util;
using TMPro;
using UnityEngine;

namespace GRisk.Interface
{
    public class Notifier : MonoBehaviour
    {
        public GRFacade facade;
        public TMP_Text board;
        
        private FixedSizedQueue<string> logs = new();

        private void Awake()
        {
            logs.Limit = 4;
        }

        private void Start()
        {
            update();
        }

        private void update()
        {
            board.SetText($"Player: {facade.engine.currentPlayer()}\n" +
                          $"Phase: {facade.engine.currentPhase().ToString()}\n\n\n" +
                          $"> {string.Join("\n >", logs)}");
        }
        
        public void log(string msg)
        {
            Debug.Log(msg);
            
            logs.Enqueue(msg);
            
            update();
        }

        public void extraLog(string msg, Transform origin)
        {
            log(msg);
            FountainNotification.spawn(msg, origin);
        }
    }
}