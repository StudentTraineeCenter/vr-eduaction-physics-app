using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class TeleportationActivator : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public XRRayInteractor teleport_interactor;
    public InputActionProperty teleport_activation_action;
    void Start()
    {
        teleport_interactor.gameObject.SetActive(false);

        teleport_activation_action.action.performed += Action_performed;
    }
    private void Action_performed (InputAction.CallbackContext obj) 
    {
        teleport_interactor.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(teleport_activation_action.action.WasReleasedThisFrame())
        {
            teleport_interactor.gameObject.SetActive(false);
        }
    }
}
