using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JumpingJackYouZXRTubeSeries;

public class Jack
{
    private Texture2D JackTexture2D;
    private int _currentIdleFrame;

    private const int IdleSpeed = 20;
    private int _idleSpeed = IdleSpeed;
    
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Color Color { get; set; }
    
    public int Direction { get; set; }
    public int Row { get; set; }

    private List<Rectangle> idleFrames;

    public Jack(int x, int y, int width, int height, Color color ,int direction, int row, Texture2D jackTexture2D)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Color = color;
        Direction = direction; //0 - left , 1 = right
        JackTexture2D = jackTexture2D;
        Row = Row;
        InitIdleFrames();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        
        spriteBatch.Draw(JackTexture2D,new Rectangle(X,Y,Width,Height),NextIdleFrame(),Color.Aqua);
        
    }

    private void InitIdleFrames()
    {
        idleFrames = new List<Rectangle>();
        idleFrames.Add(new Rectangle(0,0,16,16));
        idleFrames.Add(new Rectangle(16,0,16,16));
        idleFrames.Add(new Rectangle(32,0,16,16));
        idleFrames.Add(new Rectangle(48,0,16,16));
        _currentIdleFrame = 0;

    }

    private Rectangle NextIdleFrame()
    {
        var nextFrame = idleFrames[_currentIdleFrame];
        
        _idleSpeed--;
        if (_idleSpeed == 0)
        {
            _idleSpeed = IdleSpeed;
            _currentIdleFrame++;
            if (_currentIdleFrame >= idleFrames.Count)
            {
                //reset idle frame
                _currentIdleFrame = 0;
            }
            
            
        }
        
       

        

        return nextFrame;
    }
}