using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum CharaterState
{ 
    Normal,
    Jumping,
    Falling,
    Landing
}
public class PlayerAnimation : MonoBehaviour
{

    public Animation target;
    public Rigidbody rigidbody;
    public Transform root, spine, hub;
    public float wakingSpeed = 0.2f;
    public float runSpeed = 1.0f;
    public float rotationSpeed = 6.0f;
    public float shuffleSpeed = 7.0f;
    public float runningLandingFactor = 0.2f;
    private PlayerController controller;
    private CharaterState state = CharaterState.Falling;
    private bool canLand = true;
    private float currentRotation;
    private Vector3 lastRootForward;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool VerifySetup(Component component, string name)
    {
        if (component == null)
        {
            Debug.LogError("参数" + name + "未赋值");
            enabled = false;

            return false;
        }
        return true;
    }



}
