using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public sealed class Table : MonoBehaviour
{
    public event Action<string, object> OnVariableChanged;
    public event Action<string, object> OnVariableRemoved;

    [ReadOnly]
    [Serialize]
    private readonly Dictionary<string, object> variables = new();

    public T GetVariable<T>(string key)
    {
        return (T) variables[key];
    }

}