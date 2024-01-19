using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    private ÏnventoryScript inv;
    public int X;
    public int Y;
    public bool taken = false;
    public bool container;

    private void Start()
    {
        inv = FindObjectOfType<ÏnventoryScript>();
    }
    private void Update()
    {
        if (taken)
        {
            transform.GetComponent<Image>().color = Color.red;
            transform.GetComponent<Image>().raycastTarget = false;
        }
        else
        {
            transform.GetComponent<Image>().color = Color.grey;
            transform.GetComponent<Image>().raycastTarget = true;
        }
    }
    public void DropDrag()
    {
        inv.CurrentSlotX = X;
        inv.CurrentSlotY = Y;
    }

}