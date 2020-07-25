using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
    // Can edit in unity
    public int inventoryCap = 2;

    // Local Variables
    private Flora[] floraList = new Flora[5];
    private int[] floraAmounts = new int[5];
    private bool canAdd;
    private Image image;
    private Color tempColor;
    private Text text;

    // Gets FloraList
    public Flora[] getFloraList() {
        return floraList;
    }

    // Gets Flora Amounts
    public int[] getFloraAmounts() {
        return floraAmounts;
    }

    public void setFloraList(Flora[] oldFloraList) {
        floraList = oldFloraList;
        refreshSprites();
    }

    public void setFloraAmounts(int[] oldFloraAmounts) {
        floraAmounts = oldFloraAmounts;
        refreshSprites();
    }

    // Adds multiple of one item
    public void addItems(Flora flora, int amount) {
        for(int y = 0; y < amount; y++) {
            addItem(flora);
        }
    }

    // Removes multiple of one item
    public void removeItems(Flora flora, int amount) {
        for(int y = 0; y < amount; y++) {
            removeItem(flora);
        }
    }

    // adds an item to the inventory
    public void addItem(Flora flora) {
        checkAdd();        
        if (floraList.Contains(flora)) {
            floraAmounts[Array.IndexOf(floraList, flora)] += 1;
        } else if (canAdd) {
            for (int i = 0; i < 5; i++) {
                if (floraAmounts[i] == 0) {
                    floraList[i] = flora;
                    floraAmounts[i] = 1;
                    break;
                } else {
                    continue;
                }
            }
        }
        refreshSprites();
    }

    // Removes item from inventory
    public void removeItem(Flora flora) {
        if (floraList.Contains(flora) && floraAmounts[Array.IndexOf(floraList, flora)] > 0) {
            floraAmounts[Array.IndexOf(floraList, flora)] -= 1;
        }
        refreshSprites();
    }

    // Sets the canAdd variable to false if inventory array is full
    public void checkAdd() {
        int fullCount = 0;
        for (int i = 0; i < 5; i++) {
            if (floraAmounts[i] == 0) {
                canAdd = true;
                break;
            } else {
                fullCount += 1;
            }
        }
        if (fullCount == 5) {
            canAdd = false;
        }
    }

    // Gets image
    private Image getImage(int child) {
        return this.transform.GetChild(child).gameObject.GetComponent<Image>();
    }

    // Refreshes the image and text when collecting an item or losing an item
    private void refreshImage(Image thisImage, int iNum) {
        thisImage.sprite = floraList[iNum].inventorySprite;
        tempColor = thisImage.color;
        tempColor.a = 1f;
        thisImage.color = tempColor;
        text = thisImage.transform.GetChild(0).gameObject.GetComponent<Text>();
        text.text = floraAmounts[iNum].ToString();
    }

    // Refreshes the image when you lose an item
    private void refreshImageLoss(Image thisImage, int iNum) {
        if (thisImage.sprite != null) {
            thisImage.sprite = null;
            tempColor = thisImage.color;
            tempColor.a = 0f;
            image.color = tempColor;
            text = thisImage.transform.GetChild(0).gameObject.GetComponent<Text>();
            text.text = floraAmounts[iNum].ToString();
        }
    }

    // Refreshes sprites in inventory bag
    public void refreshSprites() {
        for (int i = 0; i < 5; i++) {
            image = getImage(i);
            if (floraAmounts[i] > 0) {
                refreshImage(image, i);
            } else if (floraAmounts[i] < 1) {
                refreshImageLoss(image, i);
            }
        }
        Debug.Log("Blue Flowers: " + floraAmounts[0] + "\nPink Flowers: " + floraAmounts[1]);
    }

    // Checks to see if you can pick another one of the item
    public bool checkCap(Flora flora) {
        if (floraList.Contains(flora) && floraAmounts[Array.IndexOf(floraList, flora)] > (inventoryCap - 1)) {
            return false;
        } else {
            return true;
        }
    }
}
