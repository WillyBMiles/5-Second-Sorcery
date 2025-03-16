using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class AudienceMember : MonoBehaviour
{
    [SerializeField] int sortingLayer;
    [SerializeField] Gradient gradient;
    [SerializeField] float bounceHeight;
    [SerializeField] float minBouncyPhase;
    [SerializeField] float maxBouncyPhase;
    [SerializeField] float moveSpeed;
    float currentPhase;

    SpriteRenderer sr;

    Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {
        currentPhase = Random.value;
        startingPosition = transform.position;
        moveSpeed = Random.Range(.75f, 1.3f);
    }

    // Update is called once per frame
    void Update()
    {
        currentPhase += Random.Range(minBouncyPhase, maxBouncyPhase) * Time.deltaTime * moveSpeed;
        transform.position = startingPosition + bounceHeight * Mathf.Sin(currentPhase) * Vector3.up;
    }

    [Button]
    public void SetColor()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
        sr.color = gradient.Evaluate(Random.value);
        sr.sortingOrder = sortingLayer;
    }

    [Button]
    public void Jiggle()
    {
        transform.position += (Vector3) Random.insideUnitCircle * .01f;
        transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z + (Random.value - .5f) * .1f);
    }
}
