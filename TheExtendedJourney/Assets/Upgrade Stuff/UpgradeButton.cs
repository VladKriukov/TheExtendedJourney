using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeButton : MonoBehaviour
{
    
    [SerializeField] int output = 0;
    public void Upgrade()
    {
        transform.parent.parent.parent.parent.GetComponent<UpgradeTable>().Upgrade(output);
    }

}
