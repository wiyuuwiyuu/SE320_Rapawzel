using System;
using UnityEngine;
using UnityEngine.UI;

public class HeartBar : MonoBehaviour
{
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;
    
    public int maxHealth = 2;
    private int currentHealth;

    private void Start()
    {
        currentHealth=maxHealth;
        UpdateHearts();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        UpdateHearts();
    }
    void UpdateHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHealth)
                hearts[i].sprite = fullHeart;
            else
                hearts[i].sprite = emptyHeart;
        }
    }
    
    public bool IsDead()
    {
        return currentHealth <= 0;
    }
}
