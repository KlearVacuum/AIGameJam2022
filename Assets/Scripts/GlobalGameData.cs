using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalGameData
{
    public static GameObject playerGO;

    public static void Reset()
    {
        playerGO = null;
    }
}
