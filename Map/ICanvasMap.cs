using System.Threading.Tasks;
using System.Windows.Controls;
using Mapsui;
using Mapsui.Extensions;
using Mapsui.Tiling;
using Mapsui.Widgets;
using Mapsui.Widgets.ButtonWidget;

namespace AvionicsEditor.Map
{
    public interface ICanvasMap
    {
        Image MImage { get; }
        MTypes.Math.Vector2 GetGlobalPos(MTypes.Math.Vector2 pos);
        bool IsPointOnScreen(MTypes.Math.Vector2 point);
    }
}
