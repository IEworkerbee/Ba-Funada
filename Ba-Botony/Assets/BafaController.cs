using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;

/* 
    Current Issues:
*/

public class BafaController : MonoBehaviour {
    // Object References
    public Rigidbody2D rb;
    public Animator animator;
    public Camera cam;
    public GameObject Panel;
    public GameObject Text;
    public GameObject Inventory;
    public GameObject PauseScreenUI;
    public Grid grid;
    public Tilemap tMap;

    // Editable by UI
    public float moveSpeed = 4f;
    public string[] approvedFlora;

    // Setup
    private bool isPaused = false;
    private Vector2 movement;
    private Vector3 position;
    private bool isTouching;
    private Collision2D col;
    private bool isInputEnabled;
    private Renderer rend;
    private float playedTime; 
    private Sprite currentSprite;
    private bool inventoryActive = true;

    // Runs once
    void Start() {
        if (File.Exists(Application.persistentDataPath + "/bafaInfo.dat")) {
            loadBafaOnStart();
        }
        playedTime = 0.0f;
        rend = GetComponent<Renderer>();
        isInputEnabled = true;
        Panel.SetActive(false);
        position = this.transform.position;
        this.transform.position = new Vector3(position.x, position.y, -0.01f);
    }

    void Update() {
        // Updating Variables
        position = this.transform.position;
        playedTime += Time.fixedDeltaTime;

        cam.transform.position = new Vector3(position.x, position.y, -4);
        // Get Player input axis
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (isInputEnabled) {
            animator.SetFloat("Speed", movement.sqrMagnitude);
        }

        // Checks if moving
        if (movement.sqrMagnitude > 0.01 && isInputEnabled) {
            // Change animation based on input
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
        }
        
        // Checks if you are interacting with a character
        if (Input.GetButtonDown("Interact") && isTouching && isInputEnabled) {
            if (col.gameObject.tag == "character") {
                showDialogue(col.gameObject.GetComponent<CharacterInfo>().dialogue);
                StartCoroutine(waitDialogueExit());
            } else if (col.gameObject.name == "DoorObjects") {
                this.transform.position = new Vector3(-7.5f, -4.3f, 0);
                SaveGame.SaveBafa(this, Inventory.GetComponent<InventoryManager>().getFloraList(), Inventory.GetComponent<InventoryManager>().getFloraAmounts());
                SceneManager.LoadScene("BafaHouse");
            }
        }

        // Controls Flora picking 
        if (Input.GetButtonDown("PickFlowers") && isInputEnabled) {
            TileBase tileName = TileFinder.findTileData(position, grid, tMap);
            string tileNameReal = "";
            bool isValid = false;
            if (tileName != null) {
                tileNameReal = tileName.ToString();
                isValid = true;
            }
            if (approvedFlora.Contains(tileNameReal) && isValid == true) {
                StartCoroutine(pickFlowers(tileNameReal, 2));
            }
        }

        // Removes and adds inventory visual
        if (Input.GetButtonDown("RemoveInventory") && isInputEnabled) {
            inventoryActive = !inventoryActive;
            Inventory.SetActive(inventoryActive);
        }

        // Brings up pause menu
        if (Input.GetButtonDown("Pause") && isInputEnabled) {
            PauseScreenUI.SetActive(!isPaused);
            isPaused = !isPaused;
            if (isPaused) {
                Time.timeScale = 0f;
            } else {
                Time.timeScale = 1f;
            }
        }

        if (Input.GetKeyDown("i") && isInputEnabled) {
            Flora floraTest = new Flora("overworldtilesheet_blueFlowers (UnityEngine.Tilemaps.Tile)");
            floraTest.changeState("chopped");
            Inventory.GetComponent<InventoryManager>().addItem(floraTest);
        }
    }

    // Stores what you are touching
    void OnCollisionEnter2D(Collision2D other) {
        isTouching = true;
        col = other;
    }

    // Stores that you aren't touching anything
    void OnCollisionExit2D(Collision2D other) {
        isTouching = false;
        col = other;
    }

    // Disables other input when reading dialogue
    private IEnumerator waitDialogueExit() {
        isInputEnabled = false;
        while(!isInputEnabled) {
            if(Input.GetKeyDown(KeyCode.Mouse0)) {
                isInputEnabled = true;
            }
            yield return null;
        }
        showDialogue("nothing");
    }
    
    // Pick flowers animations (Sprite and player)
    private IEnumerator pickFlowers(string tileNameReal, int timeDur) {
        Flora flora = new Flora(tileNameReal);
        if (Inventory.GetComponent<InventoryManager>().checkCap(flora)) {
            isInputEnabled = false;
            animator.SetBool("IsPickingFlowers", true);
            FloraAnimation floraAnim = Resources.Load<FloraAnimation>("floraAnimations/" + tileNameReal + "PickAnim");
            Sprite[] currentSprites = floraAnim.getSprites();
            Tile currentTile = TileFinder.findTile(position, grid, tMap);
            float timeWait = floraAnim.getTimeBetweenFrames(timeDur);
            for (int i = 0; i < floraAnim.getSpriteAmount(); i++) {
                currentSprite = currentSprites[i];
                currentTile.sprite = currentSprite;
                TileFinder.refreshTile(position, grid, tMap);
                yield return new WaitForSeconds(timeWait);
            }
            TileFinder.destroyTile(position, grid, tMap);
            SaveGame.UpdateMap(position, true, flora);
            currentTile.sprite = currentSprites[0];
            Inventory.GetComponent<InventoryManager>().addItem(flora);
            animator.SetBool("IsPickingFlowers", false);
            isInputEnabled = true;
        } else {
            showDialogue("You have the max amount of this item.");
            StartCoroutine(waitDialogueExit());
        }
    }

    // Shows and removes dialogue
    void showDialogue(string dialogue) {
        bool isActive = Panel.activeSelf;
        Panel.SetActive(!isActive);
        Text.GetComponent<Text>().text = dialogue;
    }

    // do physics changes here
    void FixedUpdate() {
        // moves player
        if (isInputEnabled) {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // Runs after input
    private void LateUpdate() {
        // Changes sorting layer based off position
        rend.sortingOrder = -(int)(GetComponent<Collider2D>().bounds.min.y * 1000);
    }

    // Saves Player
    public void saveBafa() {
        SaveGame.SaveBafa(this, Inventory.GetComponent<InventoryManager>().getFloraList(), Inventory.GetComponent<InventoryManager>().getFloraAmounts());
    }

    // Deletes Save
    public void deleteBafa() {
        SaveGame.DeleteSave();
    }
    // Loads player data
    private void loadBafaOnStart() {
        PlayerData data = SaveGame.loadBafa();
        if(data != null) {
            Flora currentFlora;
            Flora[] tempFloraList = new Flora[5];
            int[] tempFloraAmounts = data.floraInventoryAmounts;
            for (int i = 0; i < data.floraInventory.Length; i++) {
                for (int y = 0; y < 5; y++) {
                    if (tempFloraAmounts[y] > 0) {
                        Debug.Log(data.floraStatesReal[i]);
                        currentFlora = new Flora(data.floraInventory[i]);
                        currentFlora.changeState(data.floraStatesReal[i]);
                        tempFloraList[y] = currentFlora;
                        i += 1;
                    }
                }
            }
            Inventory.GetComponent<InventoryManager>().setFloraList(tempFloraList);
            Inventory.GetComponent<InventoryManager>().setFloraAmounts(data.floraInventoryAmounts);
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            this.transform.position = position;
            SaveGame.LoadMap(grid, tMap, data);
        }
    }
} 