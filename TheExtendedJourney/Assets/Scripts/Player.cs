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
    [SerializeField] Animator canvasAnimator;
    [SerializeField] PlayerHealthVisuals healthVisuals;
    float playerHealth;
    float playerHunger;
    float timer;
    public bool isAlive = true;

    private void Awake()
    {
        playerHealth = maxHealth;
        playerHunger = maxHunger;
    }

    private void Update()
    {
        if (isAlive == false) return;
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

        if (transform.position.z > Game.stats.furthestDistanceTravelled)
        {
            Game.stats.furthestDistanceTravelled = transform.position.z;
        }
    }

    public void AddFood(float amount)
    {
        playerHunger = Mathf.Clamp(playerHunger + amount, 0, maxHunger);
    }

    public void ChangeHealth(float amount)
    {
        if (isAlive == false) return;

        playerHealth = Mathf.Clamp(playerHealth + amount, 0, maxHealth);

        if (amount < 0)
        {
            healthVisuals.TookDamage();
        }

        healthVisuals.CheckHealth(playerHealth);

        if (playerHealth <= 0)
        {
            Debug.Log("Game Over");
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<FirstPersonController>().enableHeadBob = false;
            GetComponent<FirstPersonController>().enableSprint = false;
            GetComponent<FirstPersonController>().enableZoom = false;
            GetComponent<FirstPersonController>().cameraCanMove = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isAlive = false;
            Game.inGame = false;
            Invoke(nameof(GameOver), 3.5f);
        }
    }

    void GameOver()
    {
        canvasAnimator.SetTrigger("GameOver");
    }
}