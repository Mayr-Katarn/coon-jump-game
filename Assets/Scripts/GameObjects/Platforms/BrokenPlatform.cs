using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenPlatform : StaticPlatform
{
    #region FIELDS
    [SerializeField] private Transform _leftPart;
    [SerializeField] private Transform _rightPart;
    #endregion

    #region METHODS
    private void Broke()
    {
        _collider.enabled = false;
        Vector3 rotation = new(0, 0, 20);
        Vector3 scale = new(0.4f, 0.6f, 1);

        _leftPart.localEulerAngles = rotation * -1;
        _rightPart.localEulerAngles = rotation;
        _leftPart.localScale = _rightPart.localScale = scale;

        StartCoroutine(AlphaLerpAndDestroy());
    }

    IEnumerator AlphaLerpAndDestroy()
    {
        float timeElapsed = 0;

        while (timeElapsed < 1)
        {
            Color partsColor = _leftPart.GetComponentInChildren<SpriteRenderer>().color;
            Color newColor = Color.Lerp(partsColor, new Color(partsColor.r, partsColor.g, partsColor.b, 0), timeElapsed / 1);
            _leftPart.GetComponentInChildren<SpriteRenderer>().color = newColor;
            _rightPart.GetComponentInChildren<SpriteRenderer>().color = newColor;
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        Destroy();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Rigidbody2D playerRigidBody = collision.gameObject.CompareTag("Player") ? collision.gameObject.GetComponent<PlayerController>().GetRigidbody() : null;
        if (collision.gameObject.CompareTag("Player")) Broke();
    }
    #endregion
}
