using Siren;
using UnityEngine;

/// <summary>
/// a manager responsible for instantiating and managing UI screens and elements. 
/// </summary>
public class UIManager
{
    private RectTransform m_rootTransform;

    public UIManager(string rootCanvas)
    {
        m_rootTransform = LoadUI(rootCanvas).GetComponent<RectTransform>();
    }

    public T LoadUIScreen<T>(string canvasName, FlowState state) where T : MonoBehaviour
    {
        T screen = LoadUI(canvasName, m_rootTransform).GetComponent<T>();
        screen.GetComponent<FlowUIGroup>().AttachFlowState(state);

        return screen;
    }

    private GameObject LoadUI(string uiObject, Transform parent = null)
    {
        GameObject uiPrefab = Resources.Load<GameObject>(uiObject);
        
        Debug.Assert(uiPrefab != null, $"The Ui object: {uiObject} you have tried to load does not exist");
        
        GameObject ui = Object.Instantiate(uiPrefab, parent);

        return ui;
    }
}
