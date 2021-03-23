using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    //Codigo de extensoes que podem ser chamados em qualquer momento do codigo

    public static void Shuffle<T>(this IList<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

        public static void ShuffleList<T>(this List<T> list)//Mistura elementos de uma lista automaticamente
    {

        int n = list.Count;

        while (n > 1)
        { 
            int k = (Random.Range(0, n) % n);
            n--;
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
