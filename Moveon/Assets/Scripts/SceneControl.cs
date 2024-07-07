using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneControl : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private GameObject philo;
    [SerializeField] private GameObject decision;
    [SerializeField] private GameObject trigDet;
    [SerializeField] private GameObject refute;
    [SerializeField] private GameObject buttonYes;
    [SerializeField] private GameObject buttonNo;
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private GameObject player;
    public bool playerStop;
    private bool inDecision;
    private float spawnInterval;
    private float spawnPhiloInterval;
    private float philoTimer;
    private float daysCounter;
    private float timer;
    private List<GameObject> instances;
    private GameObject philosopher;
    private AudioSource audioSource;

    private void Start()
    {
        dayText.text = "";
        LoadDaysCounter();
        inDecision = false;
        playerStop = false;
        instances = new List<GameObject>();
        spawnPhiloInterval = 10.0f;
        spawnInterval = 2.0f;
        timer = 2.0f;
        philoTimer = 0.0f;
        decision.SetActive(false);
        refute.SetActive(false);
        buttonYes.SetActive(false);
        buttonNo.SetActive(false);
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (philosopher == null)
        {
            philoTimer += Time.deltaTime;
        }
        daysCounter += Time.deltaTime;
        if (daysCounter / 10 < 2)
        {
            dayText.text = (daysCounter / 10).ToString("F0") + " Day";
        }
        else
        {
            dayText.text = (daysCounter / 10).ToString("F0") + " Days";
        }

        if (IsMoving())
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();

            }
        }

        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && !inDecision)
        {
            StartAllMovement();
        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && timer >= spawnInterval && !inDecision)
        {
            SpawnPrefab();
            timer = 0f;
        }
        if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.D))
        {
            StopAllMovement();
        }
        if (philosopher != null && trigDet.GetComponent<TrigDetector>().collided)
        {
            philosopher.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StopAllMovement();
            DecisionPanel();

        }
        if ((Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) && philoTimer >= spawnPhiloInterval && !inDecision)
        {
            SpawnPrefabPhilo();
            philoTimer = 0f;
        }
    }

    private void SpawnPrefab()
    {
        int index = Random.Range(0, prefabs.Length);
        Vector3 spawnPosition;
        Vector3 spawnScale;

        if (index < 3)
        {
            spawnPosition = new Vector3(11, Random.Range(2.0f, 3.5f), 0);
            spawnScale = new Vector3(0.25f, 0.25f, 1);
        }
        else
        {
            spawnPosition = new Vector3(11, -0.25f, 0);
            spawnScale = new Vector3(0.5f, 0.5f, 1);
        }


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

    private void StopAllMovement()
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
    private void StartAllMovement()
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

    private void SpawnPrefabPhilo()
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

    private void DecisionPanel()
    {
        if (!inDecision)
        {
            playerStop = true;
            inDecision = true;
            decision.SetActive(true);
            buttonYes.SetActive(true);
            buttonNo.SetActive(true);
            philoTimer = 0f;
            PauseGame();
        }
    }

    public void OnYesClicked()
    {
        playerStop = true;
        decision.SetActive(false);
        buttonYes.SetActive(false);
        buttonNo.SetActive(false);
        refute.SetActive(true);
        Time.timeScale = 1;
        StartCoroutine(DeactivateAfterTime(2));
    }

    public void OnNoClicked()
    {
        ResumeGame();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void ResumeGame()
    {
        if (inDecision)
        {
            Time.timeScale = 1;
            decision.SetActive(false);
            buttonYes.SetActive(false);
            buttonNo.SetActive(false);
            inDecision = false;
            trigDet.GetComponent<TrigDetector>().ResetTrig();
            StartAllMovement();
            playerStop = false;
            philosopher.GetComponent<Rigidbody2D>().velocity = new Vector2(-5, 0);
        }
    }

    private IEnumerator DeactivateAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        refute.SetActive(false);
        ResumeGame();
    }
    private void SaveDaysCounter()
    {
        PlayerPrefs.SetFloat("DaysCounter", daysCounter);
        PlayerPrefs.Save();
    }
    private void LoadDaysCounter()
    {
        if (PlayerPrefs.HasKey("DaysCounter"))
        {
            daysCounter = PlayerPrefs.GetFloat("DaysCounter");
        }
        else
        {
            daysCounter = 0;
        }
    }
    public void OnExitClicked()
    {
        SaveDaysCounter();
        Application.Quit();
    }

    public void OnRestartClicked()
    {
        daysCounter = 0;
    }

    private bool IsMoving()
    {
        Animator animPlayer = player.GetComponent<Animator>();
        Rigidbody2D rbPhilo = null;
        if (philosopher != null)
        {
            rbPhilo = philosopher.GetComponent<Rigidbody2D>();
        }
        return animPlayer.GetBool("Run") || (rbPhilo != null && rbPhilo.velocity.magnitude > 0.1f);
    }
}
