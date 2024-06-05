using System.Threading.Tasks;
using Siren;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class ActStartUI : FlowScreenUI
{
    [SerializeField]
    private Image m_image;
    [SerializeField]
    private TextMeshProUGUI m_text;
    [SerializeField]
    private CanvasGroup m_textCanvasGroup;
    [SerializeField]
    private CanvasGroup m_fader;

    public void Init(Act act)
    {
        m_image.sprite = act.Background;
        m_text.text = act.name;
    }
    
    public async Task FadeIn(float faderSeconds, float textSeconds)
    {
        await UIUtility.FadeAlpha(m_fader, 1, 0, faderSeconds);
        await UIUtility.FadeAlpha(m_textCanvasGroup, 0, 1, textSeconds);
    }
    
    public async Task FadeOut(float seconds)
    {
        await UIUtility.FadeAlpha(m_fader, 0, 1, seconds);
    }

    public override void UpdateUI()
    {
        
    }

    public override void DestroyUI()
    {
        Destroy(gameObject);
    }
}
