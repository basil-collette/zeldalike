using UnityEngine;
using UnityEngine.UI;

public class HeartManager : MonoBehaviour
{
    public Image[] hearts;
    public FloatValue playerHealth;

    public Sprite fullHeart;
    public Sprite halfFullHeart;
    public Sprite halfHeart;
    public Sprite halfEmptyHeart;
    public Sprite emptyHeart;
    
    void Start()
    {
        InitHearts();
    }

    public void InitHearts()
    {
        for (int i = 0; i < playerHealth.initialValue; i++)
        {
            hearts[i].gameObject.SetActive(true);
            hearts[i].sprite = fullHeart;
        }
    }

    public void UpdateHearts()
    {
        for (int i = 0; i < playerHealth.initialValue; i++)
        {
            float temp = playerHealth.RuntimeValue - i;
            
            if (temp < 0.25)
            {
                hearts[i].sprite = emptyHeart;
            }
            else if (temp < 0.5)
            {
                hearts[i].sprite = halfEmptyHeart;
            }
            else if (temp < 0.75)
            {
                hearts[i].sprite = halfHeart;
            }
            else if (temp < 1)
            {
                hearts[i].sprite = halfFullHeart;
            }
            else
            {
                hearts[i].sprite = fullHeart;
            }
        }
    }

}
