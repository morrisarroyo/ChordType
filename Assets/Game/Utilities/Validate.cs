using System;
using System.Runtime.CompilerServices;

public static class Validate
{
    public static void Assigned(
        UnityEngine.Object value,
        string fieldName = "")
    {
        if (value == null)
        {
            throw new InvalidOperationException(
                $"Required reference '{fieldName}' has not been assigned.");
        }
    }
}