using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public static class PortfolioTimelineBuilder
{
    private const string TimelinePath = "Assets/UpdateDirectorTimeline.playable";
    private const string GeneratedFolderPath = "Assets/TimelineGenerated";

    [MenuItem("Tools/Portfolio Demo/Rebuild UpdateDirector Timeline")]
    public static void RebuildUpdateDirectorTimeline()
    {
        var timeline = AssetDatabase.LoadAssetAtPath<TimelineAsset>(TimelinePath);
        if (timeline == null)
        {
            Debug.LogError($"Timeline asset not found: {TimelinePath}");
            return;
        }

        Undo.RegisterCompleteObjectUndo(timeline, "Rebuild portfolio demo timeline");
        ClearTimeline(timeline);

        timeline.editorSettings.fps = 60;
        timeline.durationMode = TimelineAsset.DurationMode.FixedLength;
        timeline.fixedDuration = 39.5;

        BuildGlobalVolumeSection(timeline);
        BuildStartKeySection(timeline);
        BuildPuzzle2Section(timeline);
        BuildPuzzle4Section(timeline);

        EditorUtility.SetDirty(timeline);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Selection.activeObject = timeline;
        Debug.Log("UpdateDirectorTimeline rebuilt for the four GitHub demo clips.");
    }

    [MenuItem("Tools/Portfolio Demo/Create Or Select UpdateDirector In Scene")]
    public static void CreateOrSelectUpdateDirectorInScene()
    {
        var timeline = AssetDatabase.LoadAssetAtPath<TimelineAsset>(TimelinePath);
        if (timeline == null)
        {
            Debug.LogError($"Timeline asset not found: {TimelinePath}");
            return;
        }

        var director = UnityEngine.Object.FindObjectsByType<PlayableDirector>(FindObjectsSortMode.None)
            .FirstOrDefault(item => item.playableAsset == timeline || item.name == "UpdateDirector");

        if (director == null)
        {
            var gameObject = new GameObject("UpdateDirector");
            Undo.RegisterCreatedObjectUndo(gameObject, "Create UpdateDirector");
            director = gameObject.AddComponent<PlayableDirector>();
        }

        Undo.RecordObject(director, "Assign UpdateDirector Timeline");
        director.playableAsset = timeline;
        director.playOnAwake = false;
        EditorUtility.SetDirty(director);

        Selection.activeGameObject = director.gameObject;
        EditorGUIUtility.PingObject(director.gameObject);
        Debug.Log("UpdateDirector selected. Open the Timeline window with this GameObject selected to edit track bindings.");
    }

    private static void ClearTimeline(TimelineAsset timeline)
    {
        foreach (var track in timeline.GetRootTracks().ToArray())
        {
            timeline.DeleteTrack(track);
        }
    }

    private static void BuildGlobalVolumeSection(TimelineAsset timeline)
    {
        var group = timeline.CreateTrack<GroupTrack>(null, "01 Global Volume Horror Mood - slow move (0.0-9.5)");
        var cameraTrack = CreateCameraTrack(timeline, group, "Slow directed camera - same path before/after");
        CreateClip(cameraTrack, "Before slow move: original brighter mood", 0.0, 3.5);
        CreateClip(cameraTrack, "After slow move: darker exposure and cold color", 3.5, 6.0);

        var beforeMoveTrack = timeline.CreateTrack<AnimationTrack>(group, "VCam Before slow move animation");
        CreateAnimationClip(beforeMoveTrack, "VCam_Before_SlowMove", 0.0, 3.5, 1.2f);

        var afterMoveTrack = timeline.CreateTrack<AnimationTrack>(group, "VCam After slow move animation");
        CreateAnimationClip(afterMoveTrack, "VCam_After_SlowMove", 3.5, 5.5, 1.8f);
        CreateHoldAnimationClip(afterMoveTrack, "VCam_After_FinalHold", 9.0, 0.5f, 1.8f);

        var beforeTrack = timeline.CreateTrack<ActivationTrack>(group, "Before lighting / volume state");
        CreateClip(beforeTrack, "Before state active", 0.0, 3.5);

        var afterTrack = timeline.CreateTrack<ActivationTrack>(group, "After Global Volume active");
        CreateClip(afterTrack, "After state active", 3.5, 6.0);

        var beforeLabelTrack = timeline.CreateTrack<ActivationTrack>(group, "Before label UI - bottom right");
        CreateClip(beforeLabelTrack, "Show BEFORE label", 0.0, 3.5);

        var afterLabelTrack = timeline.CreateTrack<ActivationTrack>(group, "After label UI - bottom right");
        CreateClip(afterLabelTrack, "Show AFTER label", 3.5, 6.0);

        var notes = timeline.CreateTrack<ActivationTrack>(group, "Timing notes - no binding required");
        CreateClip(notes, "Use matching slow camera paths before/after", 0.0, 3.5);
        CreateClip(notes, "CUT at 3.5 sec, continue same slow move, hold final 0.5 sec", 3.5, 6.0);
    }

    private static void BuildStartKeySection(TimelineAsset timeline)
    {
        var group = timeline.CreateTrack<GroupTrack>(null, "02 Start Key UI (12.0-22.0)");
        var cameraTrack = CreateCameraTrack(timeline, group, "Player POV camera");
        CreateClip(cameraTrack, "Play button -> UI appears", 12.0, 2.0);
        CreateClip(cameraTrack, "Hold readable UI", 14.0, 7.5);
        CreateClip(cameraTrack, "Capture fade-out", 21.5, 0.5);

        var notes = timeline.CreateTrack<ActivationTrack>(group, "Timing notes - no binding required");
        CreateClip(notes, "Trigger play/start around 12.5 sec", 12.5, 0.5);
        CreateClip(notes, "End after StartKey UI fully disappears", 21.5, 0.5);
    }

    private static void BuildPuzzle2Section(TimelineAsset timeline)
    {
        var group = timeline.CreateTrack<GroupTrack>(null, "03 Puzzle 2 Key UI (24.0-30.5)");
        var cameraTrack = CreateCameraTrack(timeline, group, "Player POV camera");
        CreateClip(cameraTrack, "Approach Puzzle 2 trigger", 24.0, 1.2);
        CreateClip(cameraTrack, "Enter zone, UI appears", 25.2, 0.8);
        CreateClip(cameraTrack, "Hold readable UI", 26.0, 2.2);
        CreateClip(cameraTrack, "Exit trigger zone", 28.2, 1.2);
        CreateClip(cameraTrack, "UI disappears", 29.4, 1.1);

        var notes = timeline.CreateTrack<ActivationTrack>(group, "Timing notes - no binding required");
        CreateClip(notes, "Enter trigger at 25.2 sec", 25.2, 0.4);
        CreateClip(notes, "Exit trigger at 28.2 sec", 28.2, 0.4);
    }

    private static void BuildPuzzle4Section(TimelineAsset timeline)
    {
        var group = timeline.CreateTrack<GroupTrack>(null, "04 Puzzle 4 UI (32.0-39.5)");
        var cameraTrack = CreateCameraTrack(timeline, group, "Player POV camera");
        CreateClip(cameraTrack, "Approach Puzzle 4 route", 32.0, 1.0);
        CreateClip(cameraTrack, "Pass display trigger, UI appears", 33.0, 1.0);
        CreateClip(cameraTrack, "Hold added UI", 34.0, 2.5);
        CreateClip(cameraTrack, "Move to hide trigger", 36.5, 1.7);
        CreateClip(cameraTrack, "UI disappears", 38.2, 1.3);

        var notes = timeline.CreateTrack<ActivationTrack>(group, "Timing notes - no binding required");
        CreateClip(notes, "Display trigger at 33.0 sec", 33.0, 0.4);
        CreateClip(notes, "Hide trigger at 38.2 sec", 38.2, 0.4);
    }

    private static TrackAsset CreateCameraTrack(TimelineAsset timeline, TrackAsset parent, string name)
    {
        var cinemachineTrackType =
            Type.GetType("Unity.Cinemachine.CinemachineTrack, Unity.Cinemachine") ??
            Type.GetType("Cinemachine.Timeline.CinemachineTrack, Cinemachine") ??
            Type.GetType("Cinemachine.Timeline.CinemachineTrack, CinemachineTimeline") ??
            Type.GetType("Unity.Cinemachine.Timeline.CinemachineTrack, Unity.Cinemachine");

        if (cinemachineTrackType != null)
        {
            return timeline.CreateTrack(cinemachineTrackType, parent, name);
        }

        return timeline.CreateTrack<ActivationTrack>(parent, $"{name} (Cinemachine not found - replace with camera track)");
    }

    private static TimelineClip CreateClip(TrackAsset track, string name, double start, double duration)
    {
        var clip = track.CreateDefaultClip();
        clip.displayName = name;
        clip.start = start;
        clip.duration = duration;
        clip.easeInDuration = 0;
        clip.easeOutDuration = 0;
        return clip;
    }

    private static TimelineClip CreateAnimationClip(
        AnimationTrack track,
        string clipName,
        double start,
        double duration,
        float forwardDistance)
    {
        var animationClip = GetOrCreateForwardMoveClip(clipName, (float)duration, forwardDistance);
        var timelineClip = track.CreateClip(animationClip);
        timelineClip.displayName = clipName;
        timelineClip.start = start;
        timelineClip.duration = duration;
        timelineClip.easeInDuration = 0;
        timelineClip.easeOutDuration = 0;
        return timelineClip;
    }

    private static TimelineClip CreateHoldAnimationClip(
        AnimationTrack track,
        string clipName,
        double start,
        double duration,
        float holdZ)
    {
        var animationClip = GetOrCreateConstantPositionClip(clipName, (float)duration, holdZ);
        var timelineClip = track.CreateClip(animationClip);
        timelineClip.displayName = clipName;
        timelineClip.start = start;
        timelineClip.duration = duration;
        timelineClip.easeInDuration = 0;
        timelineClip.easeOutDuration = 0;
        return timelineClip;
    }

    private static AnimationClip GetOrCreateForwardMoveClip(string clipName, float duration, float forwardDistance)
    {
        EnsureGeneratedFolder();

        var path = $"{GeneratedFolderPath}/{clipName}.anim";
        var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
        if (clip == null)
        {
            clip = new AnimationClip
            {
                name = clipName,
                frameRate = 60f
            };

            AssetDatabase.CreateAsset(clip, path);
        }

        var xCurve = AnimationCurve.Constant(0f, duration, 0f);
        var yCurve = AnimationCurve.Constant(0f, duration, 0f);
        var zCurve = AnimationCurve.EaseInOut(0f, 0f, duration, forwardDistance);
        AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "m_LocalPosition.x"), xCurve);
        AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "m_LocalPosition.y"), yCurve);
        AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "m_LocalPosition.z"), zCurve);

        EditorUtility.SetDirty(clip);
        return clip;
    }

    private static AnimationClip GetOrCreateConstantPositionClip(string clipName, float duration, float z)
    {
        EnsureGeneratedFolder();

        var path = $"{GeneratedFolderPath}/{clipName}.anim";
        var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(path);
        if (clip == null)
        {
            clip = new AnimationClip
            {
                name = clipName,
                frameRate = 60f
            };

            AssetDatabase.CreateAsset(clip, path);
        }

        var xCurve = AnimationCurve.Constant(0f, duration, 0f);
        var yCurve = AnimationCurve.Constant(0f, duration, 0f);
        var zCurve = AnimationCurve.Constant(0f, duration, z);
        AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "m_LocalPosition.x"), xCurve);
        AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "m_LocalPosition.y"), yCurve);
        AnimationUtility.SetEditorCurve(clip, EditorCurveBinding.FloatCurve(string.Empty, typeof(Transform), "m_LocalPosition.z"), zCurve);

        EditorUtility.SetDirty(clip);
        return clip;
    }

    private static void EnsureGeneratedFolder()
    {
        if (AssetDatabase.IsValidFolder(GeneratedFolderPath))
        {
            return;
        }

        AssetDatabase.CreateFolder("Assets", "TimelineGenerated");
    }
}
