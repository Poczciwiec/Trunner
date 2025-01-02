using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyColliderReceiver : MonoBehaviour
{
    GameObject Walker;
    

    private void Awake()
    {
        try
        {
            Walker = transform.parent.parent.gameObject;

        }
        catch
        {
            throw new System.Exception("Walker GameObject or it's component is not found!");
        }
    }

    void OnDamaged()
    {
        Walker.SendMessage("OnDamaged");
    }
}
