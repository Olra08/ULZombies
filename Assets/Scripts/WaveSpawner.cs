using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public WaveSO wave;

    [SerializeField]
    private Transform[] spawnPoints;
    private int waveInterval = 5;
    private bool firstWave = false;
    public Text nextWave;

    private float timeRemaining;
    private float timeBtwnSpawns;

    private void Awake()
    {
        timeBtwnSpawns = wave.TimeBeforeThisWave;
        timeRemaining = timeBtwnSpawns + 1;
    }

    private void Update()
    {
        if (!firstWave)
        {
            SpawnWave();
            firstWave = true;
        }
        
        timeRemaining -= Time.deltaTime;
        nextWave.text = "Next Wave: " + (int)timeRemaining;

        if (Time.time >= timeBtwnSpawns)
        {
            SpawnWave();
            if (wave.TimeBeforeThisWave - waveInterval >= 10)
            {
                timeBtwnSpawns = Time.time + (wave.TimeBeforeThisWave - waveInterval);
                timeRemaining = wave.TimeBeforeThisWave - waveInterval + 1;
                waveInterval += 5;
            }

            if (wave.TimeBeforeThisWave <= 10 || wave.TimeBeforeThisWave - waveInterval < 10)
            {
                timeBtwnSpawns = Time.time + 10;
                timeRemaining = 11;
            }
        }
        Debug.Log(((int)Time.time));
    }

    private void SpawnWave()
    {
        for (int i = 0; i < wave.NumberToSpawn; i++)
        {
            int num = Random.Range(0, wave.EnemiesInWave.Length);
            int num2 = Random.Range(0, spawnPoints.Length);

            Instantiate(wave.EnemiesInWave[num], spawnPoints[num2].position, spawnPoints[num2].rotation);
        }
    }
    /*
    private void NextWaveUpdate()
    {
        if (wave.TimeBeforeThisWave - waveInterval < 10 || waveInterval)
        {
            timeRemaining = 10;
        }
        else
        {
            timeRemaining = wave.TimeBeforeThisWave - waveInterval;
        }
    }*/
}