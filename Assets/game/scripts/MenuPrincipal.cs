using UnityEngine;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject panelInstrucciones;
    private SceneController sceneController;

    void Start()
    {
        sceneController = GetComponent<SceneController>();
        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(false);
    }

    public void Jugar()
    {
        sceneController.IrAJugar();
    }

    public void ToggleInstrucciones()
    {
        if (panelInstrucciones != null)
            panelInstrucciones.SetActive(!panelInstrucciones.activeSelf);
    }

    public void Salir()
    {
        sceneController.Salir();
    }
}