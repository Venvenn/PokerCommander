using System.Collections;
using System.Collections.Generic;
using Siren;
using UnityEngine;
using Yarn.Unity;

public class YarnCommands : MonoBehaviour
{
    [YarnCommand("flow_message")]
    public static void SendFlowMessage(string message)
    {
        FlowUIGroup[] groups = FindObjectsOfType<FlowUIGroup>();

        foreach (FlowUIGroup group in groups)
        {
            if (group.gameObject.activeSelf)
            {
                group.SendMessage(message);
            }
        }
    }
}
