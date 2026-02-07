using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyMap 
{
    public EnemyMap(GameObject gameObject)
    {
        enemy=gameObject.transform;
    }
    public bool NullCheck()
    {
        if(enemy!=null&&rectTransform!=null&&Icon!=null)
        {
            //Icon.SetActive(true);
            return true;
        }
        //Icon.SetActive(false);
        return false;
    }


    public Transform enemy;

    public RectTransform rectTransform;

    public GameObject Icon;
}
