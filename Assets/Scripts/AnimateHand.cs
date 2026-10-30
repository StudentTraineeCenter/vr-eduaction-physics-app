using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHand : MonoBehaviour
{
    public InputActionProperty trigger_value;
    public InputActionProperty grip_value;

    public Animator hand_animator; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float trigger = trigger_value.action.ReadValue<float>();
        float grip = grip_value.action.ReadValue<float>();

        hand_animator.SetFloat("Trigger", trigger);
        hand_animator.SetFloat("Grip", grip);

    }
}
