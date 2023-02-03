using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    //[CreateAssetMenu(fileName = "RampAsset", menuName = "Ramps/RampAsset", order = 2)]
    public class RampAsset : ScriptableObject
    {
        public Gradient gradient = new Gradient();
        public int size = 16;
        public bool up = false;
        public bool overwriteExisting = true;
    }
