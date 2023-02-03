using UnityEngine;

public class WeaponSelector : MonoBehaviour
{
    int index;

    private void Awake()
    {
        Activate();
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            index += (int)Input.mouseScrollDelta.y;
            if (index > transform.childCount - 1) index = 0;
            if (index < 0) index = transform.childCount - 1;
            Activate();
        }
        SelectWeapon();
    }

    private void Activate()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
            if (index == i) transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    private void SelectWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            index = 0;
            Activate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            index = 1;
            Activate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            index = 2;
            Activate();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            index = 3;
            Activate();
        }
    }
}