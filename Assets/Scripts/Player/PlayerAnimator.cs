using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    //References
    PlayerMovement pm;
    SpriteRenderer sr;
    PlayerStats player;

    public Sprite[] level1Sprites; // Indexes: N, NE, E, SE, S, SW, W, NW
    public Sprite[] level2Sprites;
    public Sprite[] level3Sprites;

    private Sprite[] currentLevelSprites;
    private int lastWeaponIndex;

    /*public Sprite northSprite;
    public Sprite northEastSprite;
    public Sprite eastSprite;
    public Sprite southEastSprite;
    public Sprite southSprite;
    public Sprite southWestSprite;
    public Sprite westSprite;
    public Sprite northWestSprite;*/


    //Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerStats>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();

        lastWeaponIndex = player.weaponIndex; //Initialize lastWeaponIndex
        UpdateCurrentLevelSprites(); //Set initial sprites
    }

    //Update is called once per frame
    void Update()
    {
        if (player.weaponIndex != lastWeaponIndex) //Check if weaponIndex has changed
        {
            lastWeaponIndex = player.weaponIndex;
            UpdateCurrentLevelSprites(); //Update sprites if weapon level changed
        }

        if (pm.moveDir.x != 0 || pm.moveDir.y != 0)
        {
            SpriteDirectionChecker();
        }
    }


    void UpdateCurrentLevelSprites()    //Changes sprite depending on weapon level
    {
        if (player.weaponIndex == 0 || player.weaponIndex == 1)
            currentLevelSprites = level1Sprites;
        else if (player.weaponIndex == 2)
            currentLevelSprites = level2Sprites;
        else if (player.weaponIndex >= 3)
            currentLevelSprites = level3Sprites;
    }


    void SpriteDirectionChecker()
    {
        Vector2 moveDir = pm.moveDir.normalized;
        if (moveDir != Vector2.zero)
        {
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;

            if (angle >= 67.5f && angle < 112.5f)
                sr.sprite = currentLevelSprites[0]; // North
            else if (angle >= 22.5f && angle < 67.5f)
                sr.sprite = currentLevelSprites[1]; // North-East
            else if (angle >= -22.5f && angle < 22.5f)
                sr.sprite = currentLevelSprites[2]; // East
            else if (angle >= -67.5f && angle < -22.5f)
                sr.sprite = currentLevelSprites[3]; // South-East
            else if (angle >= -112.5f && angle < -67.5f)
                sr.sprite = currentLevelSprites[4]; // South
            else if (angle >= -157.5f && angle < -112.5f)
                sr.sprite = currentLevelSprites[5]; // South-West
            else if (angle >= 112.5f && angle < 157.5f)
                sr.sprite = currentLevelSprites[6]; // North-West
            else
                sr.sprite = currentLevelSprites[7]; // West
        }
    }
}
