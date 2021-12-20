using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brush : MonoBehaviour
{
    public Animator Animator => animator;
    public Renderer[] Renderers => renderers;
    
    [SerializeField] private Animator animator;
    [SerializeField] private Renderer[] renderers;
}
