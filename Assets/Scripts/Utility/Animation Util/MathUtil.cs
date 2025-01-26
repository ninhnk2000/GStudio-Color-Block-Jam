using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtil
{
    public static Vector2 GetAbsoluteVector(Vector2 initialVector)
    {
        return new Vector2(Mathf.Abs(initialVector.x), Mathf.Abs(initialVector.y));
    }
}
