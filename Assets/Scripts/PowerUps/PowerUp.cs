using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour {

    private PowerUpManager manager;

    private void OnDestroy()
    {
        manager.Free();
    }

    public void SetManager(PowerUpManager newManager)
    {
        manager = newManager;
    }
}
