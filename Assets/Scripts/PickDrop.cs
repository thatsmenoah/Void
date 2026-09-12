using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickDrop : MonoBehaviour, Interface
{
    [SerializeField] private string diskName = "ZeroFloppy";

    public string GetDescription(){
        return "Взять "+diskName;
    }
    public void Interact(){
        Debug.Log("Вы подобрали предмет: "+diskName);
        Destroy(gameObject);
    }
}
