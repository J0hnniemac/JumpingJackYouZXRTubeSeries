using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpingJackYouZXRTubeSeries;

public class Road
{
    private Texture2D _roadTexture;
    
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Color Color { get; set; }

    private SpriteBatch _spriteBatch;

    public Road(int x, int y, int width, int height, Color color, SpriteBatch spriteBatch)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Color = color;
        _spriteBatch = spriteBatch;
        
    }

    private void InitRoadTexture()
    {
        _roadTexture = new Texture2D(_spriteBatch.GraphicsDevice, 1, 1);
        _roadTexture.SetData(new [] { Color.Red });
    }

    public void Draw()
    {
        if (_roadTexture == null)
        {
            InitRoadTexture();
        }
        _spriteBatch.Draw(_roadTexture, new Rectangle(X ,Y,Width,Height),Color);
    }
    
}
