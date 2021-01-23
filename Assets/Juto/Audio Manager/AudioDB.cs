using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Juto.Audio
{
    public class AudioDB : MonoBehaviour
    {

        public List<AudioClipHolder> AudioClips;

        private static List<AudioClipHolder> clips;

        private void Start()
        {
            clips = AudioClips;
        }

        /// <summary>
        /// Finds a AudioClip in the clip database.
        /// </summary>
        /// <param name="name">the name of the clip, in the list.</param>
        /// <returns></returns>
        public static AudioClip FindClip(string name)
        {
            foreach (AudioClipHolder clip in clips)
            {
                if (clip.name == name)
                    return clip.clip;
            }

            throw new System.Exception("Couldn't find any clip with the name " + name);
        }

        [System.Serializable]
        public struct AudioClipHolder
        {
            public string name;
            public AudioClip clip;
        }
    }

}
