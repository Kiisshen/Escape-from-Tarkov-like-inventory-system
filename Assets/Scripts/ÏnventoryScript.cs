using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ÏnventoryScript : MonoBehaviour
{
    public GameObject rk;
    public GameObject gridSlot;
    public int inventoryWidth = 15;
    public int inventoryHeight = 25;
    float gridOffset;
    GameObject[,] inventory;
    bool[,] inventoryData;
    public GameObject holdingObject;
    public int CurrentSlotX;
    public int CurrentSlotY;
    public GameObject containerParent;
    int containerWidth = 0;
    int containerHeight = 0;
    public ContainerScript cs;

    void Start()
    {
        inventoryData = new bool[inventoryWidth + 10, inventoryHeight + 10];
        inventory = new GameObject[inventoryWidth + 10, inventoryHeight + 10];
        gridOffset = gridSlot.GetComponent<RectTransform>().rect.width * transform.parent.parent.GetComponent<RectTransform>().localScale.x;
        PlaceGrid(inventoryWidth, inventoryHeight, gridOffset);
        //Next 3 if statemetns are to place 3 testing items in the inventory. In order for it to work gameobject rk needs to be assigned.
        if(CheckForSlots(new Vector2(0, 0), new Vector2(4, 2)))
            AddItem(new Vector2(0, 0), new Vector2(4, 2), rk);
        if(CheckForSlots(new Vector2(0, 2), new Vector2(4, 2)))
            AddItem(new Vector2(0, 2), new Vector2(4, 2), rk);
        if(CheckForSlots(new Vector2(0, 4), new Vector2(4, 2)))
            AddItem(new Vector2(0, 4), new Vector2(4, 2), rk);
    }
    private void Update()
    {
        if (holdingObject != null)
        {
            HoldingItemCursor();
        }
    }
    void HoldingItemCursor()
    {
        holdingObject.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.1f));
    }
    public void OpenContainer(int width, int height)
    {
        containerWidth = width;
        containerHeight = height;
        for (int i = inventoryWidth; i < inventoryWidth + width; i++)
        {
            for (int k = inventoryHeight; k < inventoryHeight + height; k++)
            {
                Vector3 pos = new Vector3((i-inventoryWidth) * 35f, (k-inventoryHeight) * 35f, 0);
                GameObject currentSlot = Instantiate(gridSlot, containerParent.transform);
                currentSlot.transform.localPosition += pos;
                currentSlot.GetComponent<SlotScript>().X = i;
                currentSlot.GetComponent<SlotScript>().Y = k;
                inventory[i, k] = currentSlot;
                inventory[i, k].GetComponent<SlotScript>().container = true;
            }
        }
    }
    public void CloseContainer()
    {
        for (int i = inventoryWidth; i < inventoryWidth + containerWidth; i++)
        {
            for (int k = inventoryHeight; k < inventoryHeight + containerHeight; k++)
            {
                inventoryData[i, k] = false;
                inventory[i, k].GetComponent<SlotScript>().taken = false;
            }
        }
        cs.loot = null;
        cs.loot = new List<ContainerItem>();
        foreach (Transform g in containerParent.transform)
        {
            if (g.GetComponent<ItemScript>() != null)
            {
                ContainerItem ci = new ContainerItem();
                ci.item = g.GetComponent<ItemScript>().prefab;
                ci.posX = g.GetComponent<ItemScript>().posX;
                ci.posY = g.GetComponent<ItemScript>().posY;
                ci.sizeX = g.GetComponent<ItemScript>().sizeX;
                ci.sizeY = g.GetComponent<ItemScript>().sizeY;
                cs.loot.Add(ci);
            }
            Destroy(g.gameObject);
        }
        containerWidth = 0;
        containerHeight = 0;
        cs = null;
    }
    void PlaceGrid(int width, int height, float offset)
    {
        for (int i = 0; i < width; i++)
        {
            for (int k = 0; k < height; k++)
            {
                Vector3 pos = new Vector3(i*offset, k*offset, 0);
                GameObject currentSlot = Instantiate(gridSlot, transform);
                currentSlot.transform.position += pos;
                currentSlot.GetComponent<SlotScript>().X = i;
                currentSlot.GetComponent<SlotScript>().Y = k;
                inventory[i, k] = currentSlot;
                inventory[i, k].GetComponent<SlotScript>().container = false;
            }
        }
    }
   
    public void MoveItem(Vector2 oldPos, Vector2 newPos, Vector2 size, GameObject item)
    {
        for (int i = (int)oldPos.x; i < oldPos.x + size.x; i++)
        {
            for (int k = (int)oldPos.y; k < oldPos.y + size.y; k++)
            {
                inventoryData[i, k] = false;
                inventory[i, k].GetComponent<SlotScript>().taken = false;
            }
        }
        for (int i = (int)newPos.x; i < newPos.x + size.x; i++)
        {
            for (int k = (int)newPos.y; k < newPos.y + size.y; k++)
            {
                inventoryData[i, k] = true;
                inventory[i, k].GetComponent<SlotScript>().taken = true;
            }
        }
        item.transform.position = inventory[(int)newPos.x, (int)newPos.y].transform.position;
        item.GetComponent<ItemScript>().posX = (int)newPos.x;
        item.GetComponent<ItemScript>().posY = (int)newPos.y;
        if (newPos.x >= inventoryWidth && newPos.y >= inventoryHeight)
        {
            item.transform.SetParent(containerParent.transform);
        }
        else
        {
            item.transform.SetParent(transform);
        }
    }
    public void UnclaimSlots(Vector2 pos, Vector2 size)
    {
        for (int i = (int)pos.x; i < pos.x + size.x; i++)
        {
            for (int k = (int)pos.y; k < pos.y + size.y; k++)
            {
                inventoryData[i, k] = false;
                inventory[i, k].GetComponent<SlotScript>().taken = false;
            }
        }
    }
    public void AddItem(Vector2 pos, Vector2 size, GameObject item)
    {
        int itemWidth = (int)size.x;
        int itemHeight = (int)size.y;
        int itemPosX = (int)pos.x;
        int itemPosY = (int)pos.y;

        for (int i = itemPosX; i < itemPosX + itemWidth; i++)
        {
            for (int k = itemPosY; k < itemPosY + itemHeight; k++)
            {
                inventoryData[i, k] = true;
                inventory[i, k].GetComponent<SlotScript>().taken = true;
            }
        }
        GameObject Item;
        Item = Instantiate(item, transform);
        Item.transform.position = inventory[(int)pos.x, (int)pos.y].transform.position;
        Item.GetComponent<ItemScript>().posX = (int)pos.x;
        Item.GetComponent<ItemScript>().posY = (int)pos.y;
        if (itemPosX >= inventoryWidth && itemPosY >= inventoryHeight)
        {
            Item.transform.SetParent(containerParent.transform);
        }
        else
        {
            Item.transform.SetParent(transform);
        }
    }
    public void ReClaimSlots(Vector2 pos, Vector2 size)
    {
        int itemWidth = (int)size.x;
        int itemHeight = (int)size.y;
        int itemPosX = (int)pos.x;
        int itemPosY = (int)pos.y;

        for (int i = itemPosX; i < itemPosX + itemWidth; i++)
        {
            for (int k = itemPosY; k < itemPosY + itemHeight; k++)
            {
                inventoryData[i, k] = true;
                inventory[i, k].GetComponent<SlotScript>().taken = true;
            }
        }
    }
    public bool CheckForSlots(Vector2 pos, Vector2 size)
    {
        int itemWidth = (int)size.x;
        int itemHeight = (int)size.y;
        int itemPosX = (int)pos.x;
        int itemPosY = (int)pos.y;

        if (pos.x + size.x > inventoryWidth && pos.y + size.y > inventoryHeight && pos.x + size.x <= inventoryWidth + containerWidth && pos.y + size.y <= inventoryHeight + containerHeight)
        {
            for (int i = itemPosX; i < itemPosX + itemWidth; i++)
            {
                for (int k = itemPosY; k < itemPosY + itemHeight; k++)
                {
                    if (inventoryData[i, k])
                    {
                        print("Can not place item in " + pos + ", slots taken in Container.");
                        return false;
                    }
                }
            }
            return true;
        }
        if (pos.x + size.x <= inventoryWidth)
        {
            if (pos.y + size.y <= inventoryHeight)
            {
                for (int i = itemPosX; i < itemPosX + itemWidth; i++)
                {
                    for (int k = itemPosY; k < itemPosY + itemHeight; k++)
                    {
                        if (inventoryData[i, k])
                        {
                            print("Can not place item in " + pos + ", slots taken.");
                            return false;
                        }
                    }
                }
                return true;
            }
        }
        print("Can not place item in " + pos + ", past inventory boundaries.");
        return false;
    }
}
