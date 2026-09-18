using UnityEngine;

public class NumbGen : MonoBehaviour, IInteractable { 


    public void Interact()
    {
        Debug.Log(Random.Range(0, 100));
    }

}


  
