using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void IrAJugar()
    {
        SceneManager.LoadScene("Escena2_Bosque");
    }

    public void IrALaboratorio()
    {
        SceneManager.LoadScene("Escena3_Laboratorio");
    }

    public void IrAMenu()
    {
        GameManager.Instance.inventario.Clear();
        GameManager.Instance.recetaActualIndex = 0;
        SceneManager.LoadScene("Escena1_Menu");
    }

    public void MostrarInstrucciones()
    {
        // Lo maneja el UIManager
    }

    public void Salir()
    {
        Application.Quit();
    }
}