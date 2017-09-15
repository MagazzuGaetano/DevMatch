using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformExtesions
{
    public static IEnumerator Move(this Transform t, Vector3 target, float duration)
    {
        Vector3 DiffVector = (target - t.position);
        float DiffLenght = DiffVector.magnitude;
        DiffVector.Normalize();
        float counter = 0;
        while (counter < duration)
        {
            float movAmount = ((Time.deltaTime * DiffLenght) / duration);
            t.position += (DiffVector * movAmount);
            counter += Time.deltaTime;
            yield return null;
        }
        t.position = target;
    }

    public static IEnumerator Scale(this Transform t, Vector3 target, float duration)
    {
        Vector3 DiffVector = (target - t.localScale);
        float DiffLenght = DiffVector.magnitude;
        DiffVector.Normalize();
        float counter = 0;
        while (counter < duration)
        {
            float movAmount = ((Time.deltaTime * DiffLenght) / duration);
            t.localScale += (DiffVector * movAmount);
            counter += Time.deltaTime;
            yield return null;
        }
        t.localScale = target;
    }
}
