using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObservedReference<T> : ScriptableObject
{
    private T reference;
    private List<Action<T, T>> onReferenceChangedActions = new();

    public void SetReference(T newReference)
    {
        foreach (Action<T, T> action in onReferenceChangedActions)
        {
            action.Invoke(reference, newReference);
        }
        reference = newReference;
        
    }

    public T GetReference()
    {
        return reference;
    }
    public void Register(Action<T, T> a)
    {
        if(!onReferenceChangedActions.Contains(a))
            onReferenceChangedActions.Add(a);
    }
    public void Unregister(Action<T, T> a)
    {
        if(onReferenceChangedActions.Contains(a))
            onReferenceChangedActions.Remove(a);
    }
}

