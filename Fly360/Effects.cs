using System;

using Xamarin.Forms;

namespace Fly360
{
    public class EntryEffect : RoutingEffect
    {
        public EntryEffect()
            : base("Fly360.EntryEffect")
        {
        }
    }

    public class ShadowEffect : RoutingEffect
    {
        public float Radius { get; set; }

        public Color Color { get; set; }

        public float DistanceX { get; set; }

        public float DistanceY { get; set; }

        public ShadowEffect() : base("Fly360.LabelShadowEffect")
        {
        }
    }
}

