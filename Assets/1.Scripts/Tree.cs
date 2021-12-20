using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    [SerializeField] private Transform fullForm;
    [SerializeField] private Transform cutForm;
    [SerializeField] private Rigidbody topRB;
    [SerializeField] private ParticleSystem treeSlash;
    
    public void Slash()
    {
        GetComponent<Collider>().enabled = false;
        fullForm.gameObject.SetActive(false);
        cutForm.gameObject.SetActive(true);
        topRB.AddForce(Vector3.right * 3, ForceMode.Impulse);
        treeSlash.Play();
    }
}
