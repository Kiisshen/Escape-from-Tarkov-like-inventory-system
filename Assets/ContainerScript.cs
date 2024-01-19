using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerScript : MonoBehaviour
{
    public List<ContainerItem> loot;
    private ÏnventoryScript inv;

    void Start()
    {
        for (int i = 0; i < loot.Count; i++)
        {
            loot[i].item = null;
        }
        inv = FindObjectOfType<ÏnventoryScript>();
    }
    public void RayCastOpenContainer()
    {
        inv.OpenContainer(10, 10);
        inv.cs = transform.GetComponent<ContainerScript>();
        if (inv.CheckForSlots(new Vector2(inv.inventoryWidth, inv.inventoryHeight), new Vector2(4, 2)))
        {
            for (int i = 0; i < loot.Count; i++)
            {
                if (loot[i].item != null)
                {
                    try
                    {
                        inv.AddItem(new Vector2(loot[i].posX, loot[i].posY), new Vector2(loot[i].sizeX, loot[i].sizeY), loot[i].ItemRead());
                    }
                    catch (System.Exception)
                    {

                        print("Missing item information");
                        throw;
                    }
                }
            }
        }
    }
}
[System.Serializable]
public class ContainerItem
{
    public GameObject item;
    public int posX;
    public int posY;
    public int sizeX;
    public int sizeY;

    public void setPositon(int x, int y)
    {
        posX = x;
        posY = y;
    }
    public GameObject ItemRead()
    {
        return item;
    }
}