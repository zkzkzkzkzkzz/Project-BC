using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 무한 루프 검사 및 방지
/// </summary>
public static class InfiniteLoopDetector
{
    private static string prevPoint = "";
    private static int detectionCount = 0;
    private const int DetectionThreshold = 10000;

    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Run(
        [System.Runtime.CompilerServices.CallerMemberName] string mn = "",
        [System.Runtime.CompilerServices.CallerFilePath] string fp = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int ln = 0)
    {
        string curPoint = $"{fp}:{ln}, {mn}()";

        if (prevPoint == curPoint)
            ++detectionCount;
        else
            detectionCount = 0;

        if (detectionCount > DetectionThreshold)
            throw new Exception($"Infinite Loop Detected : \n{curPoint}\n\n");

        prevPoint = curPoint;
    }

#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoadMethod]
    private static void Init()
    {
        UnityEditor.EditorApplication.update += () =>
        {
            detectionCount = 0;
        };
    }
#endif
}
