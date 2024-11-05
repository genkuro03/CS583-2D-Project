using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public CharacterScriptableObject characterData;

    public AudioClip gameOverSound; //Sounds for player, level ip, gmae over, hit
    public AudioClip levelUpSound;
    public AudioClip hitSound;

    private AudioSource audioSource;

    //Fields of player
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;

    #region Current Stats Properties

    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            if(currentHealth != value)
            {
                currentHealth = value;
                if(GameManager.instance != null)
                {
                    GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
                }
            }
        }
    }
    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            if (currentRecovery != value)
            {
                currentRecovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
                }
            }
        }
    }
    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
                }
            }
        }
    }
    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            if (currentMight != value)
            {
                currentMight = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
                }
            }
        }
    }
    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
                }
            }
        }
    }
    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            if (currentMagnet != value)
            {
                currentMagnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;
                }
            }
        }
    }

    #endregion

    //Fields of Experience
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //Grabs level ranges (so easier for me to implement levels in ranges... im lazy)
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }

    //Fields for i frames (so you dsont insta die)
    [Header("I-Frames")]
    public float invicibilityDuration;
    float invicibilityTimer;
    bool isInvincible;

    public List<LevelRange> levelRanges = new List<LevelRange>();

    //Creates list for invetory for player
    InventoryManager inventory;
    public int weaponIndex;
    public int passiveItemIndex;

    //UI fields for hp, xp
    [Header("UI")]
    public Image healthBar;
    public Image expBar;
    public TMP_Text levelText;

    public GameObject startingWeapon; //Starting weapon (cannon)


    public GameObject firstPassiveItemTest, secondPassiveItemTest;

    void Awake()
    {

        audioSource = GetComponent<AudioSource>();  //Instantiates audio
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }


        inventory = GetComponent<InventoryManager>();   //Grab invetory obj

        //Set fields
        CurrentHealth = characterData.MaxHealth;
        CurrentRecovery = characterData.Recovery;
        CurrentMoveSpeed = characterData.MoveSpeed;
        CurrentMight = characterData.Might;
        CurrentProjectileSpeed = characterData.ProjectileSpeed;
        CurrentMagnet = characterData.Magnet;

        SpawnWeapon(startingWeapon);    //Gives player cannon (starting weapon)
    }

    void Start()
    {
        experienceCap = levelRanges[0].experienceCapIncrease;   //store experienceCap for each range

        //For stats when u press esc to see stats
        GameManager.instance.currentHealthDisplay.text = "Health: " + currentHealth;
        GameManager.instance.currentRecoveryDisplay.text = "Recovery: " + currentRecovery;
        GameManager.instance.currentMoveSpeedDisplay.text = "Move Speed: " + currentMoveSpeed;
        GameManager.instance.currentMightDisplay.text = "Might: " + currentMight;
        GameManager.instance.currentProjectileSpeedDisplay.text = "Projectile Speed: " + currentProjectileSpeed;
        GameManager.instance.currentMagnetDisplay.text = "Magnet: " + currentMagnet;

        GameManager.instance.AssignLevelCharacterUI(characterData);

        UpdateHealthBar();  //Updates health bar (full)
        UpdateExpBar(); //Updates xp (empty)
    }
    void Update()   
    {
        if(invicibilityTimer > 0)   //Checks if invicinble
        {
            invicibilityTimer -= Time.deltaTime;    //Decrements invinc time
        }
        else if (isInvincible)  //if invinc timer 0, remove flag
        {
            isInvincible = false;
        }

        Recover();  //Call natural recover
    }

    public void IncreaseExperience(int amount)  //Field to increase xp on player stats
    {
        experience += amount;
        LevelUpChecker();   //Check if level up

        UpdateExpBar(); //Updates xp bar for UI
    }

    void LevelUpChecker()
    {
        if(experience >= experienceCap) //If enough XP
        {
            level++;    //Level up
            experience -= experienceCap;    //decrement total xp (so u save leftover xp)

            int experienceCapIncrease = 0;
            foreach (LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease; //Sets new cap from levelRange field

            UpdateLevelText();  //Updates level text (top right lvl)

            if (levelUpSound != null)
            {
                audioSource.PlayOneShot(levelUpSound, 0.5f);    //Plays level up sound
            }


            GameManager.instance.StartLevelUp();    //Start level up UI and screen
        }
    }

    void UpdateExpBar() //Updates bar to be % of way to xp Cap
    {
        expBar.fillAmount = (float)experience / experienceCap;
    }
    void UpdateLevelText()  //Updates top right text
    {
        levelText.text = "LV " + level.ToString();
    }

    public void TakeDamage(float dmg)   //helper to take damage
    {
        if (!isInvincible)  //Checks if invincible
        {
            CurrentHealth -= dmg;

            invicibilityTimer = invicibilityDuration;
            isInvincible = true;

            if (CurrentHealth <= 0) //Checks if hp < 0 then trigger kill fn
            {
                Kill();
            }
            if(CurrentHealth > 0)
            {
                if (hitSound != null)   //Else play just normal hit noise
                {
                    audioSource.PlayOneShot(hitSound);
                }
            }
            UpdateHealthBar();  //Update new hp bar 
        }
    }

    void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / characterData.MaxHealth; //% of hp from max HP
    }

    public void Kill()  //if hp <= 0 
    {
        if (!GameManager.instance.isGameOver)   //Start game over enum
        {
            if (gameOverSound != null)
            {
                audioSource.PlayOneShot(gameOverSound); //play game over
            }


            GameManager.instance.AssignLevelReachedUI(level);
            GameManager.instance.AssignWeaponsAndPassiveItemsUI(inventory.weaponUISlots, inventory.passiveItemUISlots);
            GameManager.instance.GameOver();    //Assign end game stats then trigger end game over 
        }
    }

    public void RestoreHealth(float amount) //Pick up hp fn
    {
        if(CurrentHealth < characterData.MaxHealth) //If current hp less than max
        {
            CurrentHealth += amount;                //Heal as much as u can with recovery rate
            if(CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;    //If going over max hp, just set to max hp
            }
        }
    }

    void Recover()  //Natural recovery hp fn
    {
        if(CurrentHealth < characterData.MaxHealth) //if current hp less than amx
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;  //Heal as much as u can with recovery rate

            if (CurrentHealth > characterData.MaxHealth)
            {
                CurrentHealth = characterData.MaxHealth;    //If going over max hp, just set to max hp
            }
        }
    }

    public void SpawnWeapon(GameObject weapon)  //Spawn weapon item
    {
        if(weaponIndex >= inventory.weaponSlots.Count)  //Checks if slots are full (not in my current state but just in case)
        {
            Debug.LogError("Weapon Inventory slots already full");
            return;
        }

        GameObject spawnedWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);
        inventory.AddWeapon(weaponIndex, spawnedWeapon.GetComponent<WeaponController>());   //Spawn item and add to inven, also icnrememnt index for weapon

        weaponIndex++;
    }

    public void SpawnPassiveItem(GameObject passiveItem)    //Spawn passive item 
    {
        if (passiveItemIndex >= inventory.passiveItemSlots.Count)   //Check if slots are full
        {
            Debug.LogError("Passive Inventory slots already full");
            return;
        }

        GameObject spawnedPassiveItem = Instantiate(passiveItem, transform.position, Quaternion.identity);
        spawnedPassiveItem.transform.SetParent(transform);
        inventory.AddPassiveItem(passiveItemIndex, spawnedPassiveItem.GetComponent<PassiveItem>()); //Spawn item and add to inve, increment index too

        passiveItemIndex++;
    }
}
