using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticles;
    [SerializeField] private Transform wick;

    private void Start()
    {
        StartCoroutine(Explode());
    }

    IEnumerator Explode()
    {
        wick.DOLocalMoveY(0.01f, 2f);
        yield return new WaitForSeconds(2f);
        GetComponent<CinemachineImpulseSource>().GenerateImpulse();
        GameObject explosion = Instantiate(explosionParticles.gameObject, transform.GetChild(0).transform.position, Quaternion.identity);
        Destroy(explosion, 2f);
        Destroy(gameObject);
    }
}
