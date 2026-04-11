using UnityEngine;

public class ItemRecolectable : MonoBehaviour
{
    public Ingrediente datos; // se asigna dinámicamente desde el spawner

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameManager.Instance.AgregarIngrediente(datos.nombre);

        // Mostrar en pantalla (el UIManager lo puede tomar después)
        Debug.Log($"Recogiste: {datos.nombre} (valor: {datos.valor})");

        Destroy(gameObject);
    }
}