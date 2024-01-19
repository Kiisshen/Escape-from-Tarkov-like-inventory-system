using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemScript : MonoBehaviour
{
    public GameObject prefab;
    private ContainerScript cs;
    public GameObject hoverImage;
    GameObject hover;
    ÏnventoryScript inv;
    bool isHolding;
    public int sizeX = 4;
    public int sizeY = 2;
    public int posX;
    public int posY;

    void Start()
    {
        prefab = Resources.Load<GameObject>("RK62");
        transform.GetComponent<Image>().raycastTarget = true;
        inv = FindObjectOfType<ÏnventoryScript>();
        cs = inv.cs;
    }
    public void Drag()
    {
        if (inv.holdingObject == null && isHolding == false)
        {
            isHolding = true;
            hover = Instantiate(hoverImage, inv.transform.parent.parent);
            inv.holdingObject = hover;
            transform.GetComponent<Image>().raycastTarget = false;
            inv.UnclaimSlots(new Vector2(posX, posY), new Vector2(sizeX, sizeY));
        }
    }
    public void EndDrag()
    {
        if (inv.holdingObject == hover && isHolding == true)
        {
            transform.GetComponent<Image>().raycastTarget = true;
            isHolding = false;
            Destroy(hover);
            inv.holdingObject = null;
            if (inv.CheckForSlots(new Vector2(inv.CurrentSlotX, inv.CurrentSlotY), new Vector2(sizeX, sizeY)))
            {
                inv.MoveItem(new Vector2(posX, posY), new Vector2(inv.CurrentSlotX, inv.CurrentSlotY), new Vector2(sizeX, sizeY), transform.gameObject);
            }
            else
            {
                inv.ReClaimSlots(new Vector2(posX, posY), new Vector2(sizeX, sizeY));
            }
        }
    }
}