using System;
using UdonSharp;
using UnityEngine;
using USPPNet;
using VRC.SDK3.Data;
using VRC.SDKBase;
using DateTime = System.DateTime;

// ReSharper disable once CheckNamespace
namespace Dilbert {
    [UdonBehaviourSyncMode(BehaviourSyncMode.Manual)]
    public class Logger : USPPNetUdonSharpBehaviour
    {
        public int maxBPSSync = 150;
        public bool logToConsole = false;
     
        private DataDictionary _logMessages = new DataDictionary();

        public void Start() {
            _BPSCheck();
        }

        public void PrintLog() {
            Debug.Log(GetLog());
        }

        public string GetLog() {
            var keys = _logMessages.GetKeys();
            keys.Sort();
            var arrayKeys = keys.ToArray();
            
            var log = "";
            foreach (var key in arrayKeys) {
                log += _logMessages[key].String + ", ";
            }

            return log;
        }

        public void Log(object message) {
            if (!Networking.IsMaster)
                return;

            var time = DateTime.UtcNow;
            USPPNET_NetLog(time, message.ToString());
            r_NetLog(time, message.ToString());
        }

        private void r_NetLog(DateTime time, string message) {
            if (_logMessages.ContainsKey(time.Ticks)) {
                var oldMessageContent = _logMessages[time.Ticks].String;
                if (oldMessageContent.Equals(message))
                    return;

                message = oldMessageContent + "  |  " +  message;
                
                if (logToConsole)
                    Debug.Log(message);
                
                _logMessages[time.Ticks] = message;
                return;
            }
            if (logToConsole)
                Debug.Log(message);
            
            _logMessages.Add(time.Ticks, message);
        }
        private void USPPNET_NetLog(DateTime time, string message) {
            r_NetLog(time, message);
        }

        public override void OnPlayerJoined(VRCPlayerApi player) {
            Log($"({player.displayName}, {player.playerId}) Joined.");
            if (!Networking.IsMaster || player.isLocal)
                return;
            StartSync();
        }

        public override void OnPlayerLeft(VRCPlayerApi player) {
            if (!Networking.IsMaster)
                return;
            Log($"({player.displayName}, {player.playerId}) Left.");
        }

        #region SyncFromMaster

        private void StartSync() {
            _syncIndex = 0;

            if (!_isSyncing)
                _SyncTick();
            
            _isSyncing = true;
        }

        private int _byteCounter;
        private bool _isSyncing;
        private int _syncIndex;
        public void _SyncTick() {
            if (_syncIndex >= _logMessages.Count) {
                _isSyncing = false;
                return;
            }
            if (_byteCounter > maxBPSSync) {
                SendCustomEventDelayedSeconds(nameof(_SyncTick), 0.15f);
                return;
            }

            var messageTime = new DateTime(_logMessages.GetKeys()[_syncIndex].Long);
            var message = _logMessages[messageTime.Ticks].String;

            USPPNET_NetLog(messageTime, message);
            RequestSerialization();
            _syncIndex++;
            
            SendCustomEventDelayedSeconds(nameof(_SyncTick), 0.10f);
        }

        public void _BPSCheck() {
            _byteCounter = Math.Max(0, _byteCounter - maxBPSSync);
            SendCustomEventDelayedSeconds(nameof(_BPSCheck), 1);
        }
        

        #endregion

        #region Net Init
        
        public override void OnDeserialization() {
            // USPPNet OnDeserialization
        }
   
        public override void OnPreSerialization() {
            // USPPNet OnPreSerialization
        }
       
        public override void OnPostSerialization(VRC.Udon.Common.SerializationResult result) {
            // USPPNet OnPostSerialization
            _byteCounter += bytesSent;
        }
        // USPPNet Init
        
        #endregion
        
    }
}
