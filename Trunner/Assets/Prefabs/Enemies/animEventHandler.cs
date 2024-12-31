using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animEventHandler : MonoBehaviour
{
    GameObject Walker;

    private void Awake()
    {
        try
        {
            Walker = transform.parent.gameObject;
        }
        catch
        {
            throw new System.Exception("Failed to fetch parent GameObject");
        }
    }

    void Attack()
    {
        Walker.SendMessage("Attack", SendMessageOptions.DontRequireReceiver);
    }

    void AttackCompleted()
    {
        Walker.SendMessage("StartCooldown", SendMessageOptions.DontRequireReceiver);
    }
}
