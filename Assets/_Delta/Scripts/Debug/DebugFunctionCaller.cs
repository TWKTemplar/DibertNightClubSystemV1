using UdonSharp;
using VRC.SDKBase;
using VRC.Udon;

// ReSharper disable once CheckNamespace
namespace Dilbert.Debugging {
    [UdonBehaviourSyncMode(BehaviourSyncMode.None)]
    public class DebugFunctionCaller : UdonSharpBehaviour
    {
        public UdonBehaviour target;
        public string targetFunction;
        
        public override void Interact() {
            if (Utilities.IsValid(target) && !string.IsNullOrWhiteSpace(targetFunction))
                target.SendCustomEvent(targetFunction);
        }
    }
}
