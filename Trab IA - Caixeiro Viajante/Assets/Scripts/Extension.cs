using System.Collections.Generic;

public static class Extensions
{
    //Codigo de extensoes que podem ser chamados em qualquer momento do codigo

    public static void ShuffleList<T>(this List<T> list)//Mistura elementos de uma lista automaticamente
    {

        int n = list.Count;

        while (n > 1)
        {
            int k = (UnityEngine.Random.Range(0, n) % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
