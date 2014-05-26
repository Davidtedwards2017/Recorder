using UnityEngine;
using System.Collections;
using System;
using System.Reflection;

public static class ExtentionMethods {

    public static T GetCopyOf<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if(type != other.GetType()) return null; //type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;
        PropertyInfo[] pinfos = type.GetProperties(flags);
        foreach(var pinfo in pinfos)
        {
            if(pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch{ }
            }
        }

        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos) {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().GetCopyOf(toAdd) as T;
    }

    public static void MoveTo(this GameObject go, GameObject targetGo)
    {
        if(targetGo == null)
        {
            go.transform.parent = null;
        }
        else
        {
            go.transform.parent = targetGo.transform;
            targetGo.SendMessage("onGameObjectAttached", go, SendMessageOptions.DontRequireReceiver);
        }
    }

    public static T CopyComponent<T>(this T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }
}
