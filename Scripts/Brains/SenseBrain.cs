using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Brains
{
    public class SenseBrain : Brain
    {
        public List<Sense> senses;

        public override short? Behave(BehaveParam param)
        {
            throw new NotImplementedException();
        }

        public override Vector3? Think(ThinkParam param)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class Sense
    {
        public SenseEnum type;
        public float distance;
        public AngleDegree[] schema;
    }

    [Serializable]
    public class AngleDegree
    {
        public float degree;
        public float angle;
    }

}
