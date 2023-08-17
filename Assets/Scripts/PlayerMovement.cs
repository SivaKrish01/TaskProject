using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float rotationSpeed = 100f;

    private CharacterController controller;

    public Inventory inventoy = new Inventory();


    public Text coinCountText;
    public Text booleanText;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        coinCountText.text = "coins: " + inventoy.coins;
        booleanText.text = "GotMoney: " + inventoy.isGot;
    }

    private void Update()
    {
        // Player Movement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 movement = transform.TransformDirection(moveDirection) * movementSpeed * Time.deltaTime;

        controller.Move(movement);

        // Player Rotation
        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);

        coinCountText.text = "coins: " + inventoy.coins;
        booleanText.text = "GotMoney: " + inventoy.isGot;
    }

    public void SaveToJson()
    {
        string inventoryData = JsonUtility.ToJson(inventoy);
        string filePath = Application.persistentDataPath + "/InventoryData.json";
        Debug.Log(filePath);
        File.WriteAllText(filePath, inventoryData);
        Debug.Log("The file has saved");
    }

    public void LoadToJson()
    {
        string filePath = Application.persistentDataPath + "/InventoryData.json";
        string inventoryData = File.ReadAllText(filePath);

        inventoy = JsonUtility.FromJson<Inventory>(inventoryData);
        Debug.Log("The file has loaded");

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            inventoy.coins++;
            UpdateCoinCountText();
            //SaveCoinCount();
        }
    }

    private void UpdateCoinCountText()
    {
        coinCountText.text = "Coins: " + inventoy.coins++;
    }



    [System.Serializable]
    public class Inventory
    {
        public int coins;
        public bool isGot;
        public List<Item> items = new List<Item>();
    }

    [System.Serializable]
    public class Item
    {
        public string name;
    }
}
