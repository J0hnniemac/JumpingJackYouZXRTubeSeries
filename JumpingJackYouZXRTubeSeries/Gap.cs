using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace JumpingJackYouZXRTubeSeries;

public class Gap
{
    private Texture2D _gapTexture;
    public int X { get; set; }

    public int Y { get; set; }

    public int Width { get; set; }

    public int Height { get; set; }

    public Color Color { get; set; }

    public int Direction { get; set; }
    public int Row { get; set; }

    private SpriteBatch _spriteBatch;

    public Gap(int x, int y, Color color, int direction, int row, SpriteBatch spriteBatch)
    {
        X = x;
        Y = y;
        Width = 24;
        Height = 2;
        Color = color;
        Direction = direction;
        Row = row;
        _spriteBatch = spriteBatch;
        
    }
    private void InitGapTexture()
    {
        _gapTexture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
        _gapTexture.SetData(new[] { Color });
    }
    public void Draw()
    {
        if (_gapTexture == null)
        {
            InitGapTexture();
        }
        _spriteBatch.Draw(_gapTexture, new Rectangle(X, Y, Width, Height), Color);
    }
}