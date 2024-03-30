using BruTile.Wmts.Generated;
using GeoAPI.Geometries;
using Mapsui.Layers;
using Mapsui.Providers;
using Mapsui.Rendering.Skia.SkiaStyles;
using Mapsui.Rendering;
using Mapsui.Styles;
using Mapsui.Tiling;
using Mapsui.UI;
using SkiaSharp;
using System;
using System.Collections.Generic;

namespace AvionicsEditor.MTypes.Map.Render
{
    public class Default : IStyle
    {
        public double MinVisible { get; set; } = 0;
        public double MaxVisible { get; set; } = double.MaxValue;
        public bool Enabled { get; set; } = true;
        public float Opacity { get; set; } = 0.7f;
    }

    public class SmallInfo : IStyle
    {
        public double MinVisible { get; set; } = 0;
        public double MaxVisible { get; set; } = double.MaxValue;
        public bool Enabled { get; set; } = true;
        public float Opacity { get; set; } = 0.5f;
    }
}
