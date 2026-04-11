using System.Collections.Generic;

[System.Serializable]
public class Ingrediente
{
    public string nombre;
    public int valor;
    public string iconoId;
}

[System.Serializable]
public class ObjetivoReceta
{
    public string ingrediente;
    public int cantidad;
}

[System.Serializable]
public class Receta
{
    public int id;
    public string nombre;
    public List<ObjetivoReceta> objetivos;
}

[System.Serializable]
public class GameData
{
    public List<Ingrediente> ingredientes;
    public List<Receta> recetas;
}