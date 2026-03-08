using UnityEngine;

namespace CursWork.Utils
{
    public static class CursWorkUtils
    {
        public static Vector3 GetRandomDir()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }
    }
}
