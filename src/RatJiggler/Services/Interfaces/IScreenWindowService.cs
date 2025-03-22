using System.Drawing;
using System.Threading.Tasks;

namespace RatJiggler.Services.Interfaces;

public interface IScreenWindowService
{
    Task<Rectangle> GetScreenBoundsAsync();
}