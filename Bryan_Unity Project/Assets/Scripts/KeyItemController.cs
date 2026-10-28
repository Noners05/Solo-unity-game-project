using UnityEngine;

public class KeyItemController : MonoBehaviour
{
    [SerializeField] private bool firstGate = false;
    [SerializeField] private bool fGatekey = false;

    [SerializeField] private KeyInventory _keyInventory = null;

    private keyDoorController doorObject;
}
