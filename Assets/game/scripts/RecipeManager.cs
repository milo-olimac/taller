using UnityEngine;
using System.Collections.Generic;

public class RecipeManager : MonoBehaviour
{
    public static RecipeManager Instance { get; private set; }

    // Ingredientes que el jugador ha entregado para la receta actual
    private Dictionary<string, int> entregados = new Dictionary<string, int>();

    public UIManager uiManager;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        MostrarRecetaActual();
    }

    public void MostrarRecetaActual()
    {
        Receta receta = GameManager.Instance.GetRecetaActual();
        if (receta == null)
        {
            uiManager.MostrarVictoria();
            return;
        }
        uiManager.ActualizarReceta(receta);
        uiManager.ActualizarInventario(GameManager.Instance.inventario);
    }

    // Llamado cuando el jugador presiona un botón de entregar ingrediente
    public void EntregarIngrediente(string nombre)
    {
        int disponible = GameManager.Instance.CantidadIngrediente(nombre);

        if (disponible <= 0)
        {
            uiManager.MostrarMensaje($"No tienes {nombre} en el inventario.");
            return;
        }

        // Descontar del inventario
        GameManager.Instance.inventario[nombre]--;

        // Registrar entrega
        if (entregados.ContainsKey(nombre))
            entregados[nombre]++;
        else
            entregados[nombre] = 1;

        uiManager.ActualizarInventario(GameManager.Instance.inventario);
        uiManager.MostrarMensaje($"Entregaste: {nombre}");

        VerificarReceta();
    }

    void VerificarReceta()
    {
        Receta receta = GameManager.Instance.GetRecetaActual();

        foreach (var objetivo in receta.objetivos)
        {
            int entregado = entregados.ContainsKey(objetivo.ingrediente)
                ? entregados[objetivo.ingrediente] : 0;

            if (entregado < objetivo.cantidad)
                return; // Aún no está completa
        }

        // Receta completa
        uiManager.MostrarMensaje($"¡{receta.nombre} completada!");
        entregados.Clear();

        bool hayMas = GameManager.Instance.AvanzarReceta();

        if (hayMas)
        {
            Invoke(nameof(MostrarRecetaActual), 1.5f);
        }
        else
        {
            Invoke(nameof(MostrarFin), 1.5f);
        }
    }

    void MostrarFin()
    {
        uiManager.MostrarVictoria();
    }
}