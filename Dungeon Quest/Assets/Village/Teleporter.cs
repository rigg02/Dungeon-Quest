using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour,IInteractable
{
    Animator animator;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Interact()
    {
        player.GetComponent<RPlayer>().LockMovement();
        animator.SetTrigger("Teleport");
    }
    public void TP()
    {
        player.GetComponent<RPlayer>().UnLockMovememt();
        animator.ResetTrigger("Teleport");
        SceneManager.LoadScene("Diff Player");
        SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("Diff Player"));
    }
}
