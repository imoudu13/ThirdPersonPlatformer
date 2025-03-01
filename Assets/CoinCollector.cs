using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public static int score = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            score++;
            Debug.Log("Score: " + score);
            Destroy(gameObject);
        }
    }
}
