using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float playerHealth = 100f;
    [SerializeField] float maxPlayerHealth = 100f;
    [SerializeField] float playerHunger = 100f;
    [SerializeField] float maxPlayerHunger = 100f;
    [SerializeField] float hungerDecreaseRate = 5f;
    [SerializeField] Canvas playerDeadCanvas;
    [SerializeField] Image healthIcon;
    [SerializeField] Image foodIcon;

    private void Awake()
    {
        playerDeadCanvas.enabled = false;
        healthIcon.fillAmount = playerHealth / maxPlayerHealth;
        foodIcon.fillAmount = playerHunger / maxPlayerHunger;
    }

    private void Update()
    {
        DecreasePlayerHunger();
    }

    public void Damage(float damage)
    {
        playerHealth -= damage;
        healthIcon.fillAmount = playerHealth / maxPlayerHealth;

        if (playerHealth <= 0)
        {
            PlayerDead();
        }
    }

    public void DecreasePlayerHunger()
    {
        playerHunger -= hungerDecreaseRate * Time.deltaTime;
        foodIcon.fillAmount = playerHunger / maxPlayerHunger;

        if (playerHunger <= 0)
        {
            PlayerDead();
        }
    }

    public void IncreasePlayerHunger(float hungerAdd)
    {
        playerHunger += hungerAdd;
    }

    public void PlayerDead()
    {
        playerDeadCanvas.enabled = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}