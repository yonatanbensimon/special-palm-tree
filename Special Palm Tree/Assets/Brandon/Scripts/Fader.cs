using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Fader : MonoBehaviour
{
    bool isFaded = false;
    ISet<GameObject> hiddenObjects;
    Vector3Int pos;
    public Tilemap wallTilemap;
    Color color = Color.white;
    public float alpha = 0.5f;
    const float switchSpeed = 4.0f;

    Coroutine _fadeCoroutine;

    private void Start()
    {
        hiddenObjects = new HashSet<GameObject>();
    }

    private void SetGridVariable()
    {
        GameObject wallGO = GameObject.FindGameObjectWithTag("Walls");
        if (wallGO == null)
        {
            Debug.LogError("No 'walls' tag item.");
            enabled = false;
            return;
        }

        wallTilemap = wallGO.GetComponent<Tilemap>();
        pos = wallTilemap.WorldToCell(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isFaded) FadeOut();
        if (collision.gameObject.GetComponent<ShowBehindWalls>() != null)
        {
            hiddenObjects.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (hiddenObjects.Contains(collision.gameObject))
        {
            hiddenObjects.Remove(collision.gameObject);
        }

        if (hiddenObjects.Count == 0)
        {
            FadeIn();
        }
    }

    void FadeOut()
    {
        if (wallTilemap == null) SetGridVariable();

        if (!wallTilemap.HasTile(pos))
        {
            Debug.LogError("Mismatch tiles");
        }

        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(ColorSwitch(false));
    }

    void FadeIn()
    {
        if (wallTilemap == null) SetGridVariable();

        if (!wallTilemap.HasTile(pos))
        {
            Debug.LogError("Mismatch tiles");
        }

        if (_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);
        _fadeCoroutine = StartCoroutine(ColorSwitch(true));
    }

    IEnumerator ColorSwitch(bool visible)
    {
        float target = visible ? 1.0f : alpha;
        while (!Mathf.Approximately(0, color.a - target))
        {
            color.a = Mathf.MoveTowards(color.a, target, switchSpeed * Time.fixedDeltaTime);
            wallTilemap.SetColor(pos, color);
            yield return new WaitForFixedUpdate();
        }
    }
}
