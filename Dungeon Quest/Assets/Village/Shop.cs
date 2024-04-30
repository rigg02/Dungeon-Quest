using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Shop : MonoBehaviour, IInteractable
{
    public GameObject Canvas;
    public TextMeshProUGUI Message;
    public TextMeshProUGUI DisplayGold;
    public TextMeshProUGUI[] Cost;
    public GameObject Spawner;
    public GameObject[] Item;
    public int[] Gold;
    public int[] OGGold;
    public int[] Sold;
    public GameObject Player;
    public bool[] Limited;
    public int[] Inventory;
    public float[] increasePrice;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Item.Length; i++)
        {
            Sold[i] = 0;
            OGGold[i] = Gold[i];
            Cost[i].SetText(Gold[i] + " Gold");
        }
    }

    // Update is called once per frame
    void Update()
    {
        DisplayGold.SetText(Player.GetComponent<RPlayer>().gold + "");
        
    }
    public void Interact()
    {
        Canvas.SetActive(true);
        Time.timeScale = 0;
    }
    public void Exit()
    {
        Canvas.SetActive(false);
        Time.timeScale = 1;
    }
    public void Item1()
    {
        if (Limited[0] && Inventory[0] > 0)
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[0])
            {
                Player.GetComponent<RPlayer>().gold -= Gold[0];
                Inventory[0]--;
                Sold[0]++;
                Gold[0] = (int)(OGGold[0] + (float)(Sold[0]*((increasePrice[0] / 100)*OGGold[0])));
                Cost[0].SetText(Gold[0] + " Gold");
                Instantiate(Item[0], new Vector3(Spawner.transform.position.x + Random.Range(-.5f, .5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
                if (Inventory[0] == 0)
                    Cost[0].SetText("Sold Out");
            }
            else
            {
                Message.SetText("Not enough Gold");
            }
        }
        else if (!Limited[0])
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[0])
            {
                Player.GetComponent<RPlayer>().gold -= Gold[0];
                Instantiate(Item[0], new Vector3(Spawner.transform.position.x + Random.Range(-.5f, .5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
            }
            else
            {
                Message.SetText("Not enough Gold");

            }
        }
 
    }
    public void Item2()
    {
        if (Limited[1] && Inventory[1] > 0)
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[0])
            {
                Player.GetComponent<RPlayer>().gold -= Gold[0];
                Inventory[1]--;
                Sold[1]++;
                Gold[1] = (int)(OGGold[1] + (float)(Sold[1] * ((increasePrice[1] / 100) * OGGold[1])));
                Cost[1].SetText(Gold[1] + " Gold");
                Instantiate(Item[1], new Vector3(Spawner.transform.position.x + Random.Range(-.5f, .5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
                if (Inventory[1] == 0)
                    Cost[1].SetText("Sold Out");
            }
            else
            {
                Message.SetText("Not enough Gold");
            }
        }
        else if (!Limited[1])
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[1])
            {
                Player.GetComponent<RPlayer>().gold -= Gold[1];
                Instantiate(Item[1], new Vector3(Spawner.transform.position.x + Random.Range(-.5f, .5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
            }
            else
            {
                Message.SetText("Not enough Gold");
            }
        }
    }
    public void Item3()
    {
        if (Limited[2] && Inventory[2] > 0)
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[2])
            {
                Player.GetComponent<RPlayer>().gold -= Gold[2];
                Inventory[2]--;
                Sold[2]++;
                Gold[2] = (int)(OGGold[2] + (float)(Sold[2] * ((increasePrice[2] / 100) * OGGold[2])));
                Cost[2].SetText(Gold[2] + " Gold");
                Instantiate(Item[2], new Vector3(Spawner.transform.position.x + Random.Range(-.5f, .5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
                if (Inventory[2] == 0)
                    Cost[2].SetText("Sold Out");
            }
            else
            {
                Message.SetText("Not enough Gold");
            }
        }
        else if (!Limited[2])
        {
            if (Player.GetComponent<RPlayer>().gold >= Gold[2])
            {
                Player.GetComponent<RPlayer>().gold -= Gold[2];
                Instantiate(Item[2], new Vector3(Spawner.transform.position.x + Random.Range(-.5f,.5f), Spawner.transform.position.y + Random.Range(-.5f, .5f), Spawner.transform.position.z), Quaternion.identity);
            }
            else
            {
                Message.SetText("Not enough Gold");
            }
        }
    }
}