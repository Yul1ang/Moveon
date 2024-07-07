using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject philo;
    [SerializeField] private GameObject decisionPanel;
    [SerializeField] private GameObject trigDet;
    private bool stop;
    private float spawnInterval;
    private float spawnPhiloInterval;
    private float days;
    private float timer;
    private List<GameObject> instances;
    private GameObject philosopher;
    void Start()
    {
        stop = false;
        instances = new List<GameObject>();
        spawnPhiloInterval = 9.0f;
        spawnInterval = 2.0f;
        timer = 2.0f;
        days = 0.0f;
        decisionPanel.SetActive(false);
    }

    void Update()
    {
        timer += Time.deltaTime;
        days += Time.deltaTime;
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !stop)
        {
            StartAllMovement();
        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && timer >= spawnInterval && !stop)
        {
            SpawnPrefab();
            timer = 0f;
        }
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D))
        {
            StopAllMovement();
        }
        if (trigDet.GetComponent<TrigDetector>().collided)
        {
            philosopher.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StopAllMovement();
            StartCoroutine(DecisionPanel());
            // StartAllMovement();
            // philosopher.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, 0);
        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && days >= spawnPhiloInterval && !stop)
        {
            SpawnPrefabPhilo();
            days = 0f;
        }
    }

    void SpawnPrefab()
    {
        int index = Random.Range(0, prefabs.Length);

        Vector3 spawnPosition = new Vector3(11, Random.Range(2.0f, 3.5f), 0);
        Vector3 spawnScale = new Vector3(0.25f, 0.25f, 1);

        GameObject instance = Instantiate(prefabs[index], spawnPosition, Quaternion.identity);
        instance.transform.localScale = spawnScale;
        instance.AddComponent<PrefabDestroy>();

        Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = instance.AddComponent<Rigidbody2D>();
        }
        rb.velocity = new Vector2(-5, 0);
        instances.Add(instance);
    }

    void StopAllMovement()
    {
        instances.RemoveAll(instance => instance == null);
        foreach (var instance in instances)
        {
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }
    void StartAllMovement()
    {
        instances.RemoveAll(instance => instance == null);
        foreach (var instance in instances)
        {
            Rigidbody2D rb = instance.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(-5, 0);
            }
        }
    }

    void SpawnPrefabPhilo()
    {
        philosopher = Instantiate(philo, philo.transform.position, Quaternion.identity);
        philosopher.AddComponent<PrefabDestroy>();

        Rigidbody2D rb = philosopher.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = philosopher.AddComponent<Rigidbody2D>();
        }
        rb.velocity = new Vector2(-5, 0);
    }

    IEnumerator DecisionPanel()
    {
        stop = true;
        decisionPanel.SetActive(true);
        yield return new WaitForSeconds(10f);
        decisionPanel.SetActive(false);
        stop = false;
    }

    public void OnYesClicked()
    {
        Debug.Log("Sí fue seleccionado. Mostrar frase.");
        decisionPanel.SetActive(false);
    }

    public void OnNoClicked()
    {
        Debug.Log("No fue seleccionado. Continuar juego.");
        decisionPanel.SetActive(false);
    }
}
