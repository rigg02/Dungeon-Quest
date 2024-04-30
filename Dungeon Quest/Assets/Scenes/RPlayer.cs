using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

interface IInteractable
{
    void Interact();
}

public class RPlayer : MonoBehaviour
{
    public int gold = 0;
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public int mana = 100;
    private int Maxmana = 100;
    Vector2 movementInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public int healthPotion=0, manaPotion=0;
    bool canMove = true;
    public TextMeshProUGUI HealthText, ManaText, GoldText;
    [Header("Aimimg Part")]
    private Camera _cam;
    public ThrowAttack throwAttack;
    public GameObject throwSwordPos;
    

    //Dash Control
    [SerializeField] float dashSpeed = 10f;
    [SerializeField] float dashDuration = 1f;
    [SerializeField] float dashCooldown = 1f;
    bool isDashing;
    bool canDash = true;
    public float DashCollisionOffset = 1.5f;

    public Image fillHealth,fillMana;
    public Slider sliderHealth,sliderMana;
    public static RPlayer Instance;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        GoldText.text = gold.ToString();
        manaRecover();
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);

    }

    private void Update()
    {
        GoldText.text = gold.ToString();
        sliderHealth.value=gameObject.GetComponent<Health>().currentHealth/ gameObject.GetComponent<Health>().maxHealth;
        sliderMana.value = (float)mana/Maxmana;
        sliderHealth.value = (float)gameObject.GetComponent<Health>().currentHealth / gameObject.GetComponent<Health>().maxHealth;
        if (isDashing)
            return;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                    if (!success)
                    {

                        success = TryMove(new Vector2(0, movementInput.y));
                    }
                }

                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            //Direction to sprite
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;

            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;

            }
        }

    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {

                /*if(Input.GetKey(KeyCode.T) && canDash)
                {
                    print("Dashing");
                    StartCoroutine(Dash(direction));
                }*/

                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else { return false; }

    }
    void OnLook()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        /*Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;*/
        throwSwordPos.transform.up = mousePosition - rb.position;
    }
    void OnAimPad(InputValue aimInput)
    {
        Vector2 aim = aimInput.Get<Vector2>();
        throwSwordPos.transform.up = aim;
    }

    void OnSecondary_Fire()
    {
        animator.SetTrigger("LongswordAttack");
        if (mana >= 10)
        {
            mana -= 10;
            Instantiate(throwAttack, throwSwordPos.transform.position, Quaternion.identity).Init(throwSwordPos.transform.up);
        }

    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>().normalized;
    }
    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }
    #region Dash Control
    void OnDash()
    {
        if (canDash)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryDash(movementInput);

                if (!success)
                {
                    success = TryDash(new Vector2(movementInput.x, 0));
                    if (!success)
                    {

                        success = TryDash(new Vector2(0, movementInput.y));
                    }
                }

                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            //Direction to sprite
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;

            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;

            }
        }
    }

    private bool TryDash(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + DashCollisionOffset);
            if (count == 0)
            {


                StartCoroutine(Dash(direction));


                //rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else { return false; }

    }
    #endregion

    #region SwordAttack
    public void SwordAttack()
    {
        print("SwordAttt");
        LockMovement();
        if (spriteRenderer.flipX == true)
            swordAttack.AttackLeft();
        else
            swordAttack.AttackRight();
    }

    public void EndSwordAttack()
    {
        UnLockMovememt();
        swordAttack.StopAttack();
    }
    public void LockMovement()
    {
        canMove = false;
    }

    public void UnLockMovememt()
    {
        canMove = true;
    }
    #endregion
    private IEnumerator Dash(Vector2 direction)
    {
        isDashing = true;
        canDash = false;
        // dash krne se obstacles ke aar paar ja rha hai
        if (canMove)
            rb.velocity = new Vector2(direction.x * dashSpeed, direction.y * dashSpeed);
        else
            rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    private void manaRecover()
    {
        if (mana < Maxmana)
        {
            mana += 2;
        }
        Invoke("manaRecover", 1);

    }
    public void Death()
    {
        transform.position = new Vector3(0 ,0, 0);
        SceneManager.LoadScene("Village");
        SceneManager.MoveGameObjectToScene(gameObject,SceneManager.GetSceneByName("Village"));
        Time.timeScale = 1;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("HealthPick"))
        {
            healthPotion++;
            HealthText.text = healthPotion.ToString();
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.CompareTag("ManaPick"))
        {
            manaPotion++;
            ManaText.text = manaPotion.ToString();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("GoldPick"))
        {
            gold += collision.gameObject.GetComponent<GoldDrop>().gold; 
            GoldText.text = gold.ToString();
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("MaxHealthPick"))
        {
            gameObject.GetComponent<Health>().maxHealth += 10;
            sliderHealth.maxValue = (float)(gameObject.GetComponent<Health>().maxHealth/100);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("MaxManaPick"))
        {
            Maxmana += 10;
            sliderMana.maxValue = (float)(Maxmana/100);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("FireballPick"))
        {
            throwAttack.damage += 5;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("ArmourPick"))
        {
            gameObject.GetComponent<Health>().Armour += 10;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("SpeedPick"))
        {
            moveSpeed++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("SwordPick"))
        {
            swordAttack.damage += 5;
            Destroy(collision.gameObject);
        }

    }
    void OnHealUp()
    {
        if(healthPotion>0 && gameObject.GetComponent<Health>().maxHealth > gameObject.GetComponent<Health>().currentHealth)
        {  
            healthPotion--;
            HealthText.text = healthPotion.ToString();
            gameObject.GetComponent<Health>().Increasehealth(40);
        }
        
    }
    void OnManaUp()
    {
        if (manaPotion > 0 && mana != Maxmana)
        {
            manaPotion--;
            ManaText.text = manaPotion.ToString();
            if (mana >= Maxmana-50)
            {
                mana = Maxmana;
            }
            else
            {
                mana += 50;
            }
        }
    }

    void OnInteract()
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector3(transform.position.x+2,transform.position.y,transform.position.z), Vector2.up,.1f);
        if (hit.collider!=null)
        {
            print(hit.collider.gameObject.name);
            if (hit.collider.gameObject.TryGetComponent(out IInteractable interactObj))
            {
                interactObj.Interact();
            }

        }
    }
 }

