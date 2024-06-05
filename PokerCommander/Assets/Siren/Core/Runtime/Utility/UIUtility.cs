using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;

namespace Siren
{
    public static class UIUtility
    {
        public static void DestroyChildren(Transform transform)
        {
            foreach (Transform child in transform) Object.Destroy(child.gameObject);
        }
        
        public static async Task FadeAlpha(CanvasGroup canvasGroup, float startAlpha, float targetAlpha, float time)
        {
            canvasGroup.alpha = startAlpha;
            await Task.Yield();
        
            float currentTime = 0;
            while (canvasGroup.alpha != targetAlpha)
            {
                currentTime += Time.deltaTime;
                
                if (startAlpha > targetAlpha)
                {
                    canvasGroup.alpha = math.max(targetAlpha, math.lerp(startAlpha, targetAlpha, currentTime / time));
                }
                else
                {
                    canvasGroup.alpha = math.min(targetAlpha, math.lerp(startAlpha, targetAlpha, currentTime / time));
                }
                
                await Task.Yield();
            }
        }
    }
}