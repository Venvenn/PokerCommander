using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Siren
{
    public static class ColourUtility 
    {
        public static void SetA(this ref Color colour, float a)
        {
            colour.a = a;
        }
    }
}

