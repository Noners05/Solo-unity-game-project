using UnityEngine;

public class KeyItemController : MonoBehaviour
{
    [SerializeField] private bool firstGate = false;
    [SerializeField] private bool fGatekey = false;

    [SerializeField] private KeyInventory _keyInventory = null;

    private KeyDoorController doorObject;

    private void Start()
    {
        if(firstGate)
        {
            doorObject = GetComponent<KeyDoorController>();
        }
    }

    public void ObjectInteraction()
    {
        if(firstGate)
        {
            doorObject.PlayAnimation();
        }
        else if(fGatekey)
        {
            _keyInventory.hasfGatekey = true;
            gameObject.SetActive(false);
        }
    }
}
