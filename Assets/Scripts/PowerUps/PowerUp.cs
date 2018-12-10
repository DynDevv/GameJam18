using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour {

    private PowerUpManager manager;

    private void OnDestroy()
    {
        if (manager)
        {
            manager.Free();
        }
    }

    public void SetManager(PowerUpManager newManager)
    {
        manager = newManager;
    }
}
