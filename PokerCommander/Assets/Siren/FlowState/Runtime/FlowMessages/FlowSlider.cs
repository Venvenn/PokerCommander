using UnityEngine;
using UnityEngine.UI;

namespace Siren
{
    [RequireComponent(typeof(Slider))]
    public class FlowSlider : MonoBehaviour
    {
        public FlowUIGroup m_flowGroup;
        public SliderFlowMessage m_flowMessage;

        private Slider m_slider;

        void Start()
        {
            if (m_flowGroup == null)
            {
                m_flowGroup = GetComponentInParent<FlowUIGroup>();
            }

            m_slider = GetComponent<Slider>();
            m_slider.onValueChanged.AddListener(SendMessage);
        }

        private void SendMessage(float value)
        {
            m_flowMessage.SliderValue = value;
            m_flowGroup.SendMessage(m_flowMessage.GetMessage());
        }
    }
}