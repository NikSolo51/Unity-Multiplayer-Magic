﻿using CodeBase.SoundManager;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "StaticData/Level")]
    public class LevelStaticData : ScriptableObject
    {
        public bool InitGameWorld = true;
        public string LevelKey;

        public SoundManagerData SoundManagerData;
        public Vector3[] SpawnPoints;
    }
}