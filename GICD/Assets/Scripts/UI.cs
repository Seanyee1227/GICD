using UnityEngine.UI;
using UnityEngine;

public class UI : MonoBehaviour
{
    public int curHealth;
    public int maxHealth;

    public Sprite emptyHeart;
    public Sprite fullHeart;
    public Image[] hearts;

    public Player player;

    private void Update()
    {
        curHealth = player.curHealth;
        maxHealth = player.maxHealth;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < curHealth)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if (i < maxHealth)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled=false;
            }
        }
    }
}
