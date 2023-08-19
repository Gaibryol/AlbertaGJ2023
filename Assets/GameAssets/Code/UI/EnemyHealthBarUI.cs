using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarUI : MonoBehaviour
{
    [SerializeField] private Slider healthBarSliderPrefab;
    [SerializeField] private Vector2 healthBarOffset;
    private Slider healthBar;
    private GameObject worldCanvas;

    void Start()
    {
        // For now... Prob get from GameBase or GameState later...
        worldCanvas = GameObject.FindGameObjectWithTag("WorldCanvas");
        healthBar = Instantiate(healthBarSliderPrefab, transform.position, Quaternion.identity, worldCanvas.transform);
        healthBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.position = transform.position + (Vector3)healthBarOffset;
    }

    public void SetHealth(float value)
    {
        if (!healthBar.gameObject.activeSelf) 
        {
            healthBar.gameObject.SetActive(true);    
        }
        healthBar.value = value;
    }

    private void OnDestroy()
    {
        Destroy(healthBar.gameObject);
    }
}
