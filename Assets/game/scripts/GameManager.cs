using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameData datos;
    public Dictionary<string, int> inventario = new Dictionary<string, int>();
    public int recetaActualIndex = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        datos = GameDataLoader.CargarDatos();

        if (datos != null)
            Debug.Log($"Datos cargados: {datos.ingredientes.Count} ingredientes, {datos.recetas.Count} recetas");
    }

    public void AgregarIngrediente(string nombre)
    {
        if (inventario.ContainsKey(nombre))
            inventario[nombre]++;
        else
            inventario[nombre] = 1;

        Debug.Log($"Inventario: {nombre} x{inventario[nombre]}");
    }

    public int CantidadIngrediente(string nombre)
    {
        return inventario.ContainsKey(nombre) ? inventario[nombre] : 0;
    }

    public Receta GetRecetaActual()
    {
        if (datos == null || recetaActualIndex >= datos.recetas.Count)
            return null;
        return datos.recetas[recetaActualIndex];
    }

    public bool AvanzarReceta()
    {
        recetaActualIndex++;
        return recetaActualIndex < datos.recetas.Count;
    }
}