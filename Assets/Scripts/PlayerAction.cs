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
    //private Joint _joint;

    private bool _holding;
    void Start()
    {
        _holding = false;
        //_joint = _inventoryHolder.GetComponent<FixedJoint>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _holding)
        {
            _holding = false;
            _holdedItem.GetComponent<Rigidbody>().isKinematic = false;
            _holdedItem.GetComponent<Rigidbody>().AddForce(transform.forward * _throwForce);
            _holdedItem.transform.SetParent(_parent);
            //_joint.connectedBody = null;
            _holdedItem = null;
        }
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out var hitInfo))
        {
            var hitObject = hitInfo.collider.gameObject;
            if (hitObject != _currentObject && _currentObject != null)
            {
                _currentObject.GetComponent<InteractableItem>().RemoveFocus();
                _currentObject = null;
            }
            
            var interactableItem = hitObject.GetComponent<InteractableItem>();
            if (interactableItem != null)
            {
                _currentObject = hitObject;
                interactableItem.SetFocus();
            }

            
            if (Input.GetKeyDown(_actionKey))
            {
                Debug.Log("pushed");
                var component = hitObject.GetComponent<Door>();
                if (component != null)
                {
                    component.SwitchDoorState();
                }
                
                if (interactableItem != null)
                {
                    Debug.Log("Interactable");
                    if (!_holding)
                    {
                        _holding = true;
                        _parent = hitObject.transform.parent;
                        hitObject.transform.SetParent(_inventoryHolder.transform, false);
                        hitObject.transform.localPosition = new Vector3(0,0,0);
                        hitObject.GetComponent<Rigidbody>().isKinematic = true;
                        _holdedItem = hitObject;
                        //_joint.connectedBody = hitObject.GetComponent<Rigidbody>();
                    }
                    else
                    {
                        _holdedItem.transform.SetParent(_parent);
                        _holdedItem.GetComponent<Rigidbody>().isKinematic = false;
                        _holdedItem = null;
                        _parent = hitObject.transform.parent;
                        hitObject.transform.SetParent(_inventoryHolder.transform, false);
                        hitObject.transform.localPosition = new Vector3(0,0,0);
                        hitObject.GetComponent<Rigidbody>().isKinematic = true;
                        _holdedItem = hitObject;
                        //_joint.connectedBody = null;
                    }
                }
            }
        }
    }
}
