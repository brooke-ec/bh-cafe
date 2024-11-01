using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HeartsUI : MonoBehaviour
{
    public GameObject heartPrefab;
    public Sprite greyHeartSprite;
    public Sprite redHeartSprite;

    private int currentHealth;
    private int maxHealth;
    // Start is called before the first frame update
    public void SetInitialHearts(int numOfHearts)
    {
        for(int i=0; i< numOfHearts; i++)
        {
            Instantiate(heartPrefab, transform);
        }
        currentHealth = numOfHearts;
        maxHealth = numOfHearts;
    }

    public void LoseHeart()
    {
        if(currentHealth!=0) {
            currentHealth--;
            God.instance.levelUIManager.ModifyScore(-God.instance.levelUIManager.lvlSettings.scoreReductionForHealth);
        }
        transform.GetChild(currentHealth).GetComponent<Image>().sprite = greyHeartSprite;

        if(currentHealth<=0)
        {
            God.instance.levelUIManager.EndLevel();
        }
    }

    public void GainHeart()
    {
        if (currentHealth < maxHealth)
        {
            transform.GetChild(currentHealth).GetComponent<Image>().sprite = redHeartSprite;
            currentHealth++;
        }
    }

}
