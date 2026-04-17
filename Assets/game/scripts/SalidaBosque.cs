using UnityEngine;
using UnityEngine.SceneManagement;

public class SalidaBosque : MonoBehaviour
{
    public int minimoIngredientes = 3;
    public GameObject mensajeInsuficiente; // UI Text opcional

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        int total = 0;
        foreach (var entry in GameManager.Instance.inventario)
            total += entry.Value;

        if (total >= minimoIngredientes)
        {
            SceneManager.LoadScene("Escena3_Laboratorio");
        }
        else
        {
            Debug.Log($"Necesitas al menos {minimoIngredientes} ingredientes. Tienes: {total}");
            if (mensajeInsuficiente != null)
                mensajeInsuficiente.SetActive(true);
        }
    }
}