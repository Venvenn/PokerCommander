using UnityEngine;

namespace Siren
{
    public class SliderFlowMessage : FlowMessage
    {
        public float SliderValue;
        public override object GetMessage() => this;
    }
}