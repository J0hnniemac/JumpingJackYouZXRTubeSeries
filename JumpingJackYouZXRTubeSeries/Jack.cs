using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumpingJackYouZXRTubeSeries;

public class Jack
{
    private Texture2D JackTexture2D;
    private int _currentIdleFrame;
    private int _currentRunFrame;
    private int _currentJumpFrame;

    
    private const int IdleSpeed = 20;
    private int _idleSpeed = IdleSpeed;
    
    private const int RunSpeed = 5;
    private int _runSpeed = RunSpeed;
    
    private const int JumpSpeed = 5;
    private int _jumpSpeed = JumpSpeed;
    
    
    private readonly int SideBorder = 32;
    private readonly int NativeWidth = 320;

    private int _animationState = 1; //0 - idle , 1 - run, 2 - jump

    private int _targetY;
    private const int GapBetweenRoads = 24;
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Color Color { get; set; }
    
    public int Direction { get; set; }
    public int Row { get; set; }

    private List<Rectangle> idleFrames;
    private List<Rectangle> runFrames;
    private List<Rectangle> jumpFrames;

    public bool IsJumping { get; set; }
    
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
        IsJumping = false;
        InitIdleFrames();
        InitRunFrames();
        InitJumpFrames();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var frame = new Rectangle(); 
        if (_animationState == 0)
        {
            frame = NextIdleFrame();
        }
        if (_animationState == 1)
        {
            frame = NextRunFrame();
        }

        if (_animationState == 2)
        {
            frame = NextJumpFrame();
        }
        
        if (_animationState == 1 && Direction == 0)
        {
            spriteBatch.Draw(JackTexture2D,new Rectangle(X,Y,Width,Height),frame,Color.Aqua,0,Vector2.Zero,SpriteEffects.FlipHorizontally,0);
            
        }
        else
        {
            spriteBatch.Draw(JackTexture2D,new Rectangle(X,Y,Width,Height),frame,Color.Aqua);

        }

        
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

    private void InitRunFrames()
    {
        runFrames = new List<Rectangle>();
        runFrames.Add(new Rectangle(64,0,16,16));
        runFrames.Add(new Rectangle(80,0,16,16));
        runFrames.Add(new Rectangle(96,0,16,16));
        _currentRunFrame = 0;
        
    }
    private void InitJumpFrames()
    {
        jumpFrames = new List<Rectangle>();
        
        
        jumpFrames.Add(new Rectangle(128,0,16,16));
        jumpFrames.Add(new Rectangle(128,0,16,16));
        jumpFrames.Add(new Rectangle(144,0,16,16));
        jumpFrames.Add(new Rectangle(144,0,16,16));
        jumpFrames.Add(new Rectangle(144,0,16,16));
        jumpFrames.Add(new Rectangle(144,0,16,16));
        jumpFrames.Add(new Rectangle(144,0,16,16));
        jumpFrames.Add(new Rectangle(144,0,16,16));
        jumpFrames.Add(new Rectangle(144,0,16,16));
        jumpFrames.Add(new Rectangle(144,0,16,16));
        jumpFrames.Add(new Rectangle(160,0,16,16));
        _currentJumpFrame = 0;
        
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

    private Rectangle NextRunFrame()
    {
        var nextFrame = runFrames[_currentRunFrame];
        _runSpeed--;
        if (_runSpeed == 0)
        {
            _runSpeed = RunSpeed;
            _currentRunFrame++;
            if (_currentRunFrame >= runFrames.Count)
            {
                //reset idle frame
                _currentRunFrame = 0;
            }
        }
        return nextFrame;
    }
    
    private Rectangle NextJumpFrame()
    {
        var nextFrame = jumpFrames[_currentJumpFrame];
        _jumpSpeed--;
        if (_jumpSpeed == 0)
        {
            _jumpSpeed = JumpSpeed;
            _currentJumpFrame++;
            if (_currentJumpFrame >= jumpFrames.Count)
            {
                //reset idle frame
                _currentJumpFrame = 0;
                //stop jumping
            }
        }
        return nextFrame;
    }



    public void MoveLeft()
    {
        if (_animationState != 1)
        {
            _animationState = 1;
            _currentRunFrame = 0;
        }
        X -= 1;

        if (X < SideBorder - 16)
        {
            X = NativeWidth - SideBorder;
        }
        Direction = 0;
    }

    public void MoveRight()
    {
        if (_animationState != 1)
        {
            _animationState = 1;
            _currentRunFrame = 0;
        }

        X += 1;
        
        if (X > NativeWidth - SideBorder)
        {
            X =  SideBorder -16;
        }

        Direction = 1;

    }

    public void idle()
    {
        if (_animationState != 0)
        {
            _animationState = 0;
            _currentIdleFrame = 0;
        }
    }
    public void Jump()
    {
        if (_animationState != 2)
        {
            _animationState = 2;
            _currentJumpFrame = 0;
        }
        
        //set target y
        _targetY= Y - GapBetweenRoads;
        
    }

    public void jumping()
    {
        if (!IsJumping) return;
        //move player to next row
        Y -= 1;
        if (Y <= _targetY)
        {
            idle();
            IsJumping = false;
        }
        
        var mod = Y-_targetY % 4;
        if (mod == 0)
        {
            nextJumpFrame();
        }
        
        
    }

    public void Update()
    {
        jumping();
    }

    private void nextJumpFrame()
    {
        
        
        
    }
}