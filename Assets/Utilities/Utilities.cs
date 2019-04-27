using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public static class Utilities
{
    public static class Scene
    {
        static public T findExactlyOne<T>() where T : UnityEngine.Object
        {
            T[] objects = MonoBehaviour.FindObjectsOfType<T>();
            Assert.AreEqual(1, objects.Length, "There were an unexpected number of " + typeof(T).Name + ". Found: "+ objects.Length);
            return objects[0];
        }
    }

    public static Vector3 ResetY(this Vector3 vector)
    {
        vector.y = 0;
        return vector;
    }
}
