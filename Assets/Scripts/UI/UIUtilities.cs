using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public readonly struct UIUtilities
{
    /// <summary>
    /// Tweens a UI bar that contains a <see cref="RectTransform"/> and its pivoted to the left.
    /// Use this to tween for example, the percentage of a character's health. <para/>
    /// For example, if you wish to animate the health from 100% to 75%, call
    /// <c>TweenUIBarsX(HealthBar, 0.75f)</c>. <br/>The code will automatically tween from it's current
    /// position.
    /// </summary>
    /// <param name="bar">The UI panel/bar that contains a <see cref="RectTransform"/> that
    /// MUST be pivoted to the left.</param>
    /// <param name="percentage">The percentage of the bar to tween to.</param>
    public static IEnumerator TweenUIBarsX(RectTransform bar, float percentage)
    {
        // Get fixed FPS / 2.
        var fps = Mathf.FloorToInt(1.0f / Time.fixedDeltaTime) / 2;
        var sizeDelta = bar.sizeDelta;

        var maxwidth = bar.rect.width - bar.sizeDelta.x; // start from width pos.
        float rawpercentage = percentage;
        float curPercentage = bar.rect.width / maxwidth;
        percentage = curPercentage - percentage;

        var diff = Mathf.Abs(curPercentage - rawpercentage);
        // Don't waste time drawing if the percentage delta is too small.
        if (diff <= 0.005) yield break;

        // Calculate the size delta for (FPS / 2) frames.
        var inc = (maxwidth - (maxwidth * (1 - percentage))) / fps;

        for (uint i = 0; i < fps; i++)
        {
            sizeDelta.x -= inc;
            bar.sizeDelta = sizeDelta;
            yield return new WaitForFixedUpdate();
        }
    }
    /// <summary>
    /// Tweens a UI bar that contains a <see cref="RectTransform"/> and its pivoted to the bottom left.
    /// Use this to tween for example, the percentage of a character's health. <para/>
    /// For example, if you wish to animate the health from 100% to 75%, call
    /// <c>TweenUIBarsX(HealthBar, 0.75f)</c>. <br/>The code will automatically tween from it's current
    /// position.
    /// </summary>
    /// <param name="bar">The UI panel/bar that contains a <see cref="RectTransform"/> that
    /// MUST be pivoted to the left.</param>
    /// <param name="percentage">The percentage of the bar to tween to.</param>
    public static IEnumerator TweenUIBarsY(RectTransform bar, float percentage)
    {
        // Get fixed FPS / 2.
        var fps = Mathf.FloorToInt(1.0f / Time.fixedDeltaTime) / 2;
        var sizeDelta = bar.sizeDelta;

        var maxHeight = bar.rect.height - bar.sizeDelta.y; // start from height pos.
        float rawpercentage = percentage;
        float curPercentage = bar.rect.height / maxHeight;
        percentage = curPercentage - percentage;

        var diff = Mathf.Abs(curPercentage - rawpercentage);
        // Don't waste time drawing if the percentage delta is too small.
        if (diff <= 0.005) yield break;

        // Calculate the size delta for (FPS / 2) frames.
        var inc = (maxHeight - (maxHeight * (1 - percentage))) / fps;

        for (uint i = 0; i < fps; i++)
        {
            sizeDelta.y -= inc;
            bar.sizeDelta = sizeDelta;
            yield return new WaitForFixedUpdate();
        }
    }
    public static IEnumerator TweenUIAlpha(Image panel, float seconds)
    {
        // Get fixed FPS / 2.
        var new_color = panel.color;
        float duration = 0;
        while (duration < seconds)
        {
            duration += Time.deltaTime;
            new_color.a = duration / seconds;
            panel.color = new_color;
            yield return new WaitForEndOfFrame();
        }
        new_color.a = 255;
        panel.color = new_color;
    }

    public static bool IsApproximatelyEqual(float a, float b, float threshold = 0.005f) => Mathf.Abs(a - b) <= threshold;
}
