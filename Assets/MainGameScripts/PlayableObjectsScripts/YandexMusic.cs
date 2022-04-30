using System;
using System.Security.Cryptography;
using UnityEngine;
using Random = System.Random;

namespace MainGameScripts.PlayableObjectsScripts
{
    public class YandexMusic : PlayableObject
    {
        public AudioSource track1;
        public AudioSource track2;
        public AudioSource track3;
        public AudioSource track4;
        public AudioSource track5;
        public AudioSource track6;
        public AudioSource track7;
        private AudioSource[] tracks;
        private  AudioSource currentAudio;

        public void Start()
        {
            tracks = new[] {track1, track2, track3, track4, track5, track6, track7};
        }

        public override void Move(Vector2 direction)
        {
            if (!Input.GetKeyDown(KeyCode.Space)) return;
            currentAudio.Stop();
            currentAudio = tracks[RandomNumberGenerator.GetInt32(0, 0)];
            currentAudio.Play();
        }
    }
}
