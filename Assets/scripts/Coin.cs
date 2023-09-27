using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public float turnSpeed = 90f;
   // public Text coinsText;
    private void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.name == "Player")
        {
           
           // numberOfCoins += 1;
            PlayerManager.numberOfCoins += 1;
            Debug.Log("Coins:" +PlayerManager.numberOfCoins);
           // Debug.Log("Coins:" + numberOfCoins);
        }
        Destroy(gameObject);

    }
    // Start is called before the first frame update
   private void Start()
    {
        
    }

    // Update is called once per frame
   private void Update()
    {
        transform.Rotate(turnSpeed * Time.deltaTime, 0, 0);
    }
}
