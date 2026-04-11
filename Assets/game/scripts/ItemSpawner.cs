using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    public GameObject ingredientePrefab;
    public List<Sprite> sprites; // asignar en Inspector en el mismo orden que el JSON

    // Define los límites del mapa donde pueden aparecer ingredientes
    public float xMin = -8f, xMax = 8f;
    public float yMin = -4f, yMax = 4f;

    public int cantidadASpawnear = 8;

    void Start()
    {
        if (GameManager.Instance == null || GameManager.Instance.datos == null)
        {
            Debug.LogError("GameManager o datos no disponibles");
            return;
        }

        SpawnIngredientes();
    }

    void SpawnIngredientes()
    {
        List<Ingrediente> lista = GameManager.Instance.datos.ingredientes;

        for (int i = 0; i < cantidadASpawnear; i++)
        {
            // Elegir ingrediente aleatorio de la lista
            Ingrediente datosIngrediente = lista[Random.Range(0, lista.Count)];

            // Posición aleatoria en el mapa
            Vector3 pos = new Vector3(
                Random.Range(xMin, xMax),
                Random.Range(yMin, yMax),
                0f
            );

            GameObject obj = Instantiate(ingredientePrefab, pos, Quaternion.identity);

            // Asignar datos al script
            ItemRecolectable item = obj.GetComponent<ItemRecolectable>();
            item.datos = datosIngrediente;

            // Cambiar sprite dinámicamente según iconoId
            AsignarSprite(obj, datosIngrediente.iconoId);
        }
    }

    void AsignarSprite(GameObject obj, string iconoId)
    {
        // Buscar sprite cuyo nombre coincida con el iconoId del JSON
        Sprite sprite = sprites.Find(s => s.name == iconoId);

        if (sprite != null)
            obj.GetComponent<SpriteRenderer>().sprite = sprite;
        else
            Debug.LogWarning($"No se encontró sprite para iconoId: {iconoId}");
    }
}