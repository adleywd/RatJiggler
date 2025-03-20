using System.Drawing;
using System.Threading.Tasks;

namespace RatJiggler.Services.Interfaces;

public interface IWindowService
{
    Task<Rectangle> GetScreenBoundsAsync();
}