
using UnityEngine;


public abstract class Equippables : MonoBehaviour
{
    public bool needMouseAim;
    public Animator anim;
    public string equipmentName;
    public Sprite equipmentUiSprite;
    public GameObject droppedObject;
    public string description;
    public int value;
}
