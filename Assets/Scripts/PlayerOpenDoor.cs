using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOpenDoor : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    // Update is called once per frame
    void Update()
    {
        var ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hitInfo, 50f) && Input.GetKeyDown(KeyCode.E))
        {
            var hitObject = hitInfo.collider.gameObject;
            var component = hitObject.GetComponent<Door>();
            if (component != null)
            {
                component.SwitchDoorState();
            }
        }
    }
}
