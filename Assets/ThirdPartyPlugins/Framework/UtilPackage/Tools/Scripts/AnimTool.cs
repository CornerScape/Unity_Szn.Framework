using System.Collections.Generic;
using UnityEngine;

namespace SznFramework.UtilPackage
{
    public static class AnimTool
    {
        public static AnimationClip[] GetAnimationClipArray(this Animator InAnimator)
        {
            if (null == InAnimator) return null;

            RuntimeAnimatorController runtimeAnimatorController = InAnimator.runtimeAnimatorController;
            return null == runtimeAnimatorController ? null : runtimeAnimatorController.animationClips;
        }

        public static Dictionary<string, AnimationClip> GetAnimationClipDict(this Animator InAnimator)
        {
            if (null == InAnimator) return null;

            RuntimeAnimatorController runtimeAnimatorController = InAnimator.runtimeAnimatorController;
            if (null == runtimeAnimatorController) return null;
            AnimationClip[] clips = runtimeAnimatorController.animationClips;
            int len = clips.Length;
            Dictionary<string, AnimationClip> clipDict= new Dictionary<string, AnimationClip>(len);
            for (int i = 0; i < len; i++)
            {
                clipDict.Add(clips[i].name, clips[i]);
            }

            return clipDict;
        }
    }
}