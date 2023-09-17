using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] private KeyCode _actionKey;

    [SerializeField] private GameObject _inventoryHolder;

    [SerializeField] private float _throwForce;

    private GameObject _currentObject;
    private Transform _parent;
    private GameObject _holdedItem;

    private bool _holding;
    void Start()
    {
        _holding = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _holding)
        {
            TryThrowItem();
        }
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo))
        {
            var hitObject = hitInfo.collider.gameObject;
            if (hitObject != _currentObject && _currentObject != null)
            {
                TryRemoveFocus();
            }

            var interactableItem = hitObject.GetComponent<InteractableItem>();
            if (interactableItem != null)
            {
                TryFocus(interactableItem, hitObject);
            }

            if (Input.GetKeyDown(_actionKey))
            {
                if (interactableItem != null)
                {
                    Debug.Log("Interactable");
                    if (!_holding)
                    {
                        PickUp(hitObject);
                    }
                    else
                    {
                        DropItem();
                        PickUp(hitObject);
                    }
                }
            }
        }
    }

    private void DropItem()
    {
        _holdedItem.transform.SetParent(_parent);
        _holdedItem.GetComponent<Rigidbody>().isKinematic = false;
        _holdedItem = null;
    }

    private void PickUp(GameObject pickup)
    {
        _holding = true;
        _parent = pickup.transform.parent;
        pickup.transform.SetParent(_inventoryHolder.transform, false);
        pickup.transform.localPosition = new Vector3(0,0,0);
        pickup.GetComponent<Rigidbody>().isKinematic = true;
        _holdedItem = pickup;
   
    }

    private void TryFocus(InteractableItem item, GameObject hitObject)
    {
         _currentObject = hitObject;
        item.SetFocus();
    }

    private void TryRemoveFocus()
    {
        _currentObject.GetComponent<InteractableItem>().RemoveFocus();
        _currentObject = null;
    }

    private void TryThrowItem()
    {
        _holding = false;
        _holdedItem.GetComponent<Rigidbody>().isKinematic = false;
        _holdedItem.GetComponent<Rigidbody>().AddForce(transform.forward * _throwForce);
        _holdedItem.transform.SetParent(_parent);
        _holdedItem = null;          
     }
}
