using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour
{
    public delegate void OnHealthChangedDelegate();
    public OnHealthChangedDelegate onHealthChangedCallback;


    #region Sigleton
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStats>();
            }

            return instance;
        }
    }
    #endregion

    private int score = 0;
    [SerializeField]
    private float health;
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float maxTotalHealth = 0;
    [SerializeField]
    private float speed = 0;
    [SerializeField]
    private float damage = 0;
    [SerializeField]
    private float jumpHeight = 0;

    public float Health { get { return health; } }
    public float MaxHealth { get { return maxHealth; } }
    public float MaxTotalHealth { get { return maxTotalHealth; } }
    public float Damage { get { return damage; } }
    public float JumpHeight { get { return jumpHeight; } }

    [HideInInspector]
    public int Score { get { return score; } set { score = value; } }
    public float Speed { get { return speed; } }

    public Image deathScreen;
    public Text gameOverText;

    public bool m_Fading;

    private bool damaged = false;
    public Image damageImage;
    public float flashSpeed = 5f;
    public Color flashColour;
    public new AudioSource audio;
    public AudioClip injureClip;

    bool dead = false;

    private void Start()
    {
        deathScreen.canvasRenderer.SetAlpha(0.01f);
        gameOverText.canvasRenderer.SetAlpha(0.01f);
    }

    public void Update()
    {
        if (dead)
        {
            GetComponent<PlayerMovement>().enabled = false;
            GetComponent<Shooting>().enabled = false;
            deathScreen.CrossFadeAlpha(1.0f, 3.0f, false);
            gameOverText.CrossFadeAlpha(1.0f, 2.0f, false);
        }
        // If the player damaged
        if (damaged)
        {
            // Set to flash colour
            damageImage.color = flashColour;
            if (Health <= 0)
                dead = true;
                
        }
        else
        {
            // Transition the colour back to clear.
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        damaged = false;
    }

    public void Heal(float health)
    {
        this.health += health;
        ClampHealth();
    }

    public void TakeDamage(float dmg)
    {
        audio.clip = injureClip;
        audio.pitch = (Random.Range(0.8f, 1.1f));
        audio.Play();
        health -= dmg;
        ClampHealth();
        damaged = true;
    }

    public void AddHealth()
    {
        if (maxHealth < maxTotalHealth)
        {
            maxHealth += 1;
            health = maxHealth;

            if (onHealthChangedCallback != null)
                onHealthChangedCallback.Invoke();
        }   
    }

    void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);

        if (onHealthChangedCallback != null)
            onHealthChangedCallback.Invoke();
    }






}
