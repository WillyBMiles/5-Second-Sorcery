using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using static UnityEngine.Rendering.DebugUI;

public static class Extensions
{
    public static T RandomValue<T>(this IEnumerable<T> list)
    {
        var ordered = list.OrderBy(_ => Random.value);
        return ordered.FirstOrDefault();
    }

    public static Sequence Delay(this System.Action action, float time)
    {
        Sequence sequence = DOTween.Sequence();
        sequence.AppendInterval(time);
        sequence.AppendCallback(() => action());
        return sequence;
    }

    public static RectTransform rectTransform(this MonoBehaviour mb) => mb.transform as RectTransform;

    public static Vector2 RandomWithin(this RectTransform rt)
    {
        return new Vector3(Random.Range(rt.rect.xMin, rt.rect.xMax),
                  Random.Range(rt.rect.yMin, rt.rect.yMax), 0) + rt.transform.position;
    }

    static Vector2 Center(this Rect rect)
    {
        return new Vector2(rect.xMin + Random.Range(0f, rect.xMax - rect.xMin), rect.yMin + Random.Range(0f, rect.yMax - rect.yMin));
    }
}
