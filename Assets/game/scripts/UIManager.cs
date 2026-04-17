using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Escena 2 - Bosque")]
    public TextMeshProUGUI textoInventarioBosque;

    [Header("Escena 3 - Laboratorio")]
    public TextMeshProUGUI textoRecetaNombre;
    public TextMeshProUGUI textoObjetivos;
    public TextMeshProUGUI textoInventarioLab;
    public TextMeshProUGUI textoMensaje;
    public GameObject panelVictoria;

    // Botones de entrega generados dinámicamente
    public Transform contenedorBotones;
    public GameObject botonIngredientePrefab;

    // --- Escena 2 ---
    public void ActualizarInventarioBosque(Dictionary<string, int> inventario)
    {
        if (textoInventarioBosque == null) return;

        string texto = "Ingredientes:\n";
        foreach (var entry in inventario)
            texto += $"• {entry.Key}: {entry.Value}\n";

        textoInventarioBosque.text = texto;
    }

    // --- Escena 3 ---
    public void ActualizarReceta(Receta receta)
    {
        textoRecetaNombre.text = $"Receta: {receta.nombre}";

        string objetivos = "Necesitas:\n";
        foreach (var obj in receta.objetivos)
            objetivos += $"• {obj.ingrediente} x{obj.cantidad}\n";

        textoObjetivos.text = objetivos;

        GenerarBotonesEntrega();
    }

    public void ActualizarInventario(Dictionary<string, int> inventario)
    {
        string texto = "Tu inventario:\n";
        foreach (var entry in inventario)
            if (entry.Value > 0)
                texto += $"• {entry.Key}: {entry.Value}\n";

        textoInventarioLab.text = texto;
    }

    public void MostrarMensaje(string msg)
    {
        textoMensaje.text = msg;
        CancelInvoke(nameof(LimpiarMensaje));
        Invoke(nameof(LimpiarMensaje), 2f);
    }

    void LimpiarMensaje()
    {
        textoMensaje.text = "";
    }

    public void MostrarVictoria()
    {
        if (panelVictoria != null)
            panelVictoria.SetActive(true);
    }

    void GenerarBotonesEntrega()
    {
        // Limpiar botones anteriores
        foreach (Transform hijo in contenedorBotones)
            Destroy(hijo.gameObject);

        // Crear un botón por cada tipo de ingrediente en inventario
        foreach (var entry in GameManager.Instance.inventario)
        {
            if (entry.Value <= 0) continue;

            GameObject btn = Instantiate(botonIngredientePrefab, contenedorBotones);
            btn.GetComponentInChildren<TextMeshProUGUI>().text = $"{entry.Key} ({entry.Value})";

            string nombre = entry.Key; // captura para el lambda
            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                RecipeManager.Instance.EntregarIngrediente(nombre);
                ActualizarInventario(GameManager.Instance.inventario);
                GenerarBotonesEntrega();
            });
        }
    }
}