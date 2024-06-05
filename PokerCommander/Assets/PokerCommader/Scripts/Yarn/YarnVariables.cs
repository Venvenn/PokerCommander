using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

public class YarnVariables : VariableStorageBehaviour
{
    private Dictionary<string, object> m_variables = new Dictionary<string, object>();
    
    public override void SetValue(string variableName, string stringValue)
    {
        m_variables[variableName] = stringValue;
    }

    public override void SetValue(string variableName, float floatValue)
    {
        m_variables[variableName] = floatValue;
    }

    public override void SetValue(string variableName, bool boolValue)
    {
        m_variables[variableName] = boolValue;
    }

    public override bool TryGetValue<T>(string variableName, out T result)
    {
        result = (T)m_variables[variableName];
        return result != null;
    }

    public override void Clear()
    {
        m_variables.Clear();
    }

    public override bool Contains(string variableName)
    {
        return m_variables.ContainsKey(variableName);
    }

    public override void SetAllVariables(Dictionary<string, float> floats, Dictionary<string, string> strings, Dictionary<string, bool> bools, bool clear = true)
    {
        if (clear)
        {
            Clear();
        }
        
        foreach (var item in floats)
        {
            m_variables[item.Key] = item.Value;
        }
        foreach (var item in strings)
        {
            m_variables[item.Key] = item.Value;
        }
        foreach (var item in bools)
        {
            m_variables[item.Key] = item.Value;
        }
    }

    public override (Dictionary<string, float>, Dictionary<string, string>, Dictionary<string, bool>) GetAllVariables()
    {
        Dictionary<string, float> floatVariables = new Dictionary<string, float>();
        Dictionary<string, string> stringVariables = new Dictionary<string, string>();
        Dictionary<string, bool> boolVariables = new Dictionary<string, bool>();

        foreach (var variable in m_variables)
        {
            switch (variable.Value)
            {
                case float value:
                {
                    floatVariables[variable.Key] = value;
                    break;
                }
                case string value:
                {
                    stringVariables[variable.Key] = value;
                    break;
                }
                case bool value:
                {
                    boolVariables[variable.Key] = value;
                    break;
                }
            }
        }
        
        return (floatVariables, stringVariables, boolVariables);
    }
}

