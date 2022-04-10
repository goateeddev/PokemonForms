using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Presentation.WPF.Service
{
    public sealed class LandscapeService
    {
        public static IEnumerable<Image> ConfigureObstructions(Canvas canvas)
        {
            foreach (var child in canvas.Children.Cast<UIElement>().Where(element => element is Image && element.Uid == "obstruction"))
            {
                yield return (Image)child;
            }
        }
    }
}