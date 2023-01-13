using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleDamageColliders : MonoBehaviour
{
    public GameObject[] damageCollidersLeft;
    public GameObject[] damageCollidersRight;

    public enum DamageType
    {
        light,
        heavy
    }

    public enum DCtype
    {
        bottom,
        up
    }

    StateManager states;

    // Start is called before the first frame update
    void Start()
    {
        states = GetComponent<StateManager>();
        CloseColliders();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
