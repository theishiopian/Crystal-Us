using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDComponent : MonoBehaviour, ICharacterComponent
{
    public List<GameObject> forceArrows;//10 chevron shapes on the fud for force meter
    public List<GameObject> forceLevels;//Diamond shapes on the hud for force level indicator

    public Slider HPBar; //slider bar for HP
    public Slider XPBar; //slider bar for XP

    public Text levelText; //text for level

    private CharacterHealthComponent health;
    private CharacterLevelComponent level;

    public bool hasNecklace;
    public bool hasKey;
    public GameObject necklace;
    public GameObject key;

    // Start is called before the first frame update
    void Start()
    {
        health = this.gameObject.GetComponent<CharacterHealthComponent>();
        level = this.gameObject.GetComponent<CharacterLevelComponent>();
        hasNecklace = false;
        hasKey = false;
    }

    // Update is called once per frame
    void Update()
    {
        //update HP bar
        if(HPBar.maxValue != health.maxHealth)
        {
            HPBar.maxValue = health.maxHealth;
        }
        else if(HPBar.value != health.health)
        {
            HPBar.value = health.health;
        }

        //update XP bar
        if(XPBar.maxValue != level.GetNextLevelXP())
        {
            XPBar.maxValue = level.GetNextLevelXP();
        }
        else if(XPBar.value != level.XP)
        {
            XPBar.value = level.XP;
        }

        //update level text
        levelText.text = "LV: " + level.level;
    }

    //helper method for force HUD
    private void ClearArrows()
    {
        foreach (GameObject arrow in forceArrows)
        {
            arrow.SetActive(false);
        }
    }

    //helper method for force HUD
    private void ClearLevels()
    {
        foreach (GameObject level in forceLevels)
        {
            level.SetActive(false);
        }
    }

    //force arrow HUD vars
    int attackLevel = 0; int oldPower = 0;
    int arrowLevel = 0;
    bool hasCharged = false;

    //start charge animation on HUD
    public void InitAttack(float attackPower)
    {
        if (attackPower < 2)
        {
            arrowLevel += Mathf.FloorToInt(attackPower * 10 / 2);
            arrowLevel *= 2;
            if (arrowLevel > oldPower && oldPower < 30 && attackLevel < 3)
            {
                forceArrows[oldPower / 3].SetActive(true);
                oldPower++;
            }
            else if (oldPower >= 15 && attackLevel > 0 && attackLevel < 2 && !hasCharged)
            {
                oldPower = 0;
                attackLevel++;
                arrowLevel = 0;
                ClearArrows();
                //Debug.Log("called");
                hasCharged = true;
            }
        }
    }

    //reset charge HUD animation
    public void ResetAttack()
    {
        oldPower = 0;
        arrowLevel = 0;
        hasCharged = false;
        ClearArrows();
    }

    //set level indicator for force HUD
    public void SetAttackLevel(float attackPower)
    {
        if (attackPower >= 2)
        {
            attackLevel = 2;
            forceLevels[1].SetActive(true);
        }
        else if (attackPower >= 1)
        {
            attackLevel = 1;
            forceLevels[0].SetActive(true);
        }
        else
        {
            attackLevel = 0;
            ClearLevels();
        }
    }

    public void getNecklace()
    {
        hasNecklace = true;
        necklace.SetActive(true);
    }

    public void getKey()
    {
        hasNecklace = false;
        necklace.SetActive(false);
        hasKey = true;
        key.SetActive(true);
    }
}
