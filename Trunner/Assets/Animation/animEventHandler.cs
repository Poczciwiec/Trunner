using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animEventHandler : MonoBehaviour
{
    GameObject MainScriptObject;
    System.Exception FetchFailException = new System.Exception("Failed to fetch parent GameObject");
    [SerializeField] int main_script_parent;

    private void Awake()
    {
        if(main_script_parent == null)
        {
            MainFetcher();
        }
        else
        {
            MainFetcher(main_script_parent);
        }
    }

    void MainFetcher()
    {
        try
        {
            MainScriptObject = transform.parent.gameObject;
        }
        catch
        {
            throw FetchFailException;
        }
    }
    void MainFetcher(int whichParent)
    {
        switch(whichParent)
        {
            case 0:
                try
                {
                    MainScriptObject = transform.parent.gameObject;
                }
                catch
                {
                    throw FetchFailException;
                }
                break;
            case 1:
                try
                {
                    MainScriptObject = transform.parent.parent.gameObject;
                }
                catch
                {
                    throw FetchFailException;
                }
                break;
        }
    }

    void Attack()
    {
        MainScriptObject.SendMessage("ApplyDamage");
    }

    void AttackCompleted()
    {
        MainScriptObject.SendMessage("StartCooldown");
    }
}
