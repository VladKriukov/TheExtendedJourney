using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] float weaponRange = 100f;
    [SerializeField] float weaponDamage = 25f;
    [SerializeField] GameObject weaponHitEffect;
    [SerializeField] float zoomInDistance = 20f;
    [SerializeField] float originalZoomDistance = 60f;
    Camera playerCamera;

    private void Start()
    {
        playerCamera = transform.parent.parent.GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) // same as Input.GetMouseButtonDown(0)
        {
            Fire();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ToggleZoom(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            ToggleZoom(false);
        }
    }

    private void Fire()
    {
        RaycastHit objectHit;
        if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out objectHit, weaponRange))
        {
            EnemyHealth enemyHealthScript = objectHit.transform.GetComponent<EnemyHealth>();
            HitEffect(objectHit);
            if (enemyHealthScript == null) return;
            enemyHealthScript.TakeDamage(weaponDamage);
        }
    }

    private void HitEffect(RaycastHit objectHit)
    {
        GameObject bulletHit = Instantiate(weaponHitEffect, objectHit.point, Quaternion.identity);
        Destroy(bulletHit, 0.5f);
        Debug.Log(objectHit.transform.name);
    }

    private void ToggleZoom(bool zoom)
    {
        Debug.Log("Toggle zoom");
        if (zoom == true) playerCamera.fieldOfView = zoomInDistance;
        else playerCamera.fieldOfView = originalZoomDistance;
    }
}