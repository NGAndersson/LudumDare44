using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Utilities : MonoBehaviour
{
    public class Scene
    {
        static public T findExactlyOne<T>() where T : UnityEngine.Object
        {
            T[] objects = FindObjectsOfType<T>();
            Assert.IsNotNull(objects);
            Assert.IsTrue(1 == objects.Length);
            return objects[0];
        }
    }
}
