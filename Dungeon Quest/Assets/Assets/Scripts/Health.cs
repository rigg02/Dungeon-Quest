using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Health : MonoBehaviour
{
    private Gamepad pad;
    private Coroutine Rumble;
    Animator animator;
    public int maxHealth = 10;
    public int Armour = 0;

    //[SerializeField] private GameObject bloodParticle;
    public float currentHealth;

    //[SerializeField] private Renderer renderer;
    //[SerializeField] private float flashTime = 0.2f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        //currentHealth.Value = 1;
        currentHealth = maxHealth;
    }

    #region Part from complex video
    /*public void Reduce(int damage)
    {
        //currentHealth.Value -= damage / maxHealth;
        CreateHitFeedback();
       // if (currentHealth.Value <= 0)
        {
            Die();
        }
    }

    public void AddHealth(int healthBoost)
    {
        //int health = Mathf.RoundToInt(currentHealth.Value * maxHealth);
        //int val = health + healthBoost;
        //currentHealth.Value = (val > maxHealth ? maxHealth : val / maxHealth);
    }

    private void CreateHitFeedback()
    {
        //Instantiate(bloodParticle, transform.position, Quaternion.identity);
        StartCoroutine(FlashFeedback());
    }

    private IEnumerator FlashFeedback()
    {
        renderer.material.SetInt("_Flash", 1);
        yield return new WaitForSeconds(flashTime);
        renderer.material.SetInt("_Flash", 0);
    }

    private void Die()
    {
        Debug.Log("Died");
        //currentHealth.Value = 1;
    }*/
    #endregion

    public void TakeDamage(int amount)
    {
        currentHealth = currentHealth - (int)(amount - (float)(amount*(Armour/100)));
        RumblePulse(.5f, .5f, 1f);
        if (currentHealth <= 0)
        {
            //dead
            gameObject.GetComponent<RPlayer>().LockMovement();
            animator.SetBool("isDead", true);
            Time.timeScale = .25f;
        }
    }

    public void Increasehealth(int amount)
    {
        currentHealth += amount;
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void RumblePulse(float low,float high,float duration)
    {
        pad = Gamepad.current;
        if(pad != null)
        {
            pad.SetMotorSpeeds(low,high);
            Rumble = StartCoroutine(StopRumble(duration, pad));
        }
    }
    private IEnumerator StopRumble(float duration,Gamepad pad)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pad.SetMotorSpeeds(0, 0);
    }
}

