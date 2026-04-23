using UnityEngine;

public class PontoScript : MonoBehaviour
{
    public GameObject ponto;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        ponto.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        ponto.SetActive(false);

    }
}
