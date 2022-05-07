using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] float maxHunger = 100;
    [SerializeField] float hungerDecreaseRate;
    [SerializeField] float hungerDamageRate;
    [SerializeField] float hungerDamageAmount;
    [SerializeField] Image healthDisplay;
    [SerializeField] Image hungerDisplay;
    float playerHealth;
    float playerHunger;
    float timer;

    private void Awake()
    {
        playerHealth = maxHealth;
        playerHunger = maxHunger;
    }

    private void Update()
    {
        if (playerHunger > 0)
        {
            playerHunger -= hungerDecreaseRate * Time.deltaTime;
            if (playerHunger > 0.5f * maxHunger)
            {
                ChangeHealth(Time.deltaTime * 0.5f);
            }
        }
        else
        {
            playerHunger = 0;
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                playerHealth -= hungerDamageAmount;
                timer = hungerDamageRate;
            }
        }

        healthDisplay.fillAmount = playerHealth / maxHealth;
        hungerDisplay.fillAmount = playerHunger / maxHunger;
    }

    public void AddFood(float amount)
    {
        playerHunger = Mathf.Clamp(playerHunger + amount, 0, maxHunger);
    }

    public void ChangeHealth(float amount)
    {
        playerHealth = Mathf.Clamp(playerHealth + amount, 0, maxHealth);
    }
}