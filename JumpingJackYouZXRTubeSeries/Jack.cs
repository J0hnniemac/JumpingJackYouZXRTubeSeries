using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using System.Xml;
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
    private int _fallTargetY;
    private const int GapBetweenRoads = 24;
    public int X { get; set; }
    public int Y { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public Color Color { get; set; }
    
    public int Direction { get; set; }
    public int Row { get; set; }

    public List<Gap> Gaps;
    public Road[] Roads;

    private List<Rectangle> idleFrames;
    private List<Rectangle> runFrames;
    private List<Rectangle> jumpFrames;

    public bool IsJumping { get; set; }
    public bool IsJumpingReady { get; set; }

    public bool IsFalling { get; set; }
    public bool JumpThroughGap { get; set; }
    
    public Jack(int x, int y, int width, int height, Color color ,
        int direction, int row, Texture2D jackTexture2D, List<Gap> gaps, Road[] roads)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Color = color;
        Direction = direction; //0 - left , 1 = right
        JackTexture2D = jackTexture2D;
        Row = row;
        IsJumping = false;
        IsJumpingReady = false;
        IsFalling = false;
        JumpThroughGap = false;
        Gaps = gaps;
        Roads = roads;
        
        
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
        CheckForGapAbove();
        if (Y <= _targetY)
        {
            idle();
            IsJumping = false;
            JumpThroughGap = false;
            Row--;
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
        CheckForGapBelow();
        if(IsFalling) FallJack();
        if(Row==0) Console.WriteLine("NEXTLEVEL");
    }

    private void nextJumpFrame()
    {
        
        
        
    }

    private void CheckForGapBelow()
    {
        foreach (var gap in Gaps)
        {
            if (gap.Row == Row)
            {
                if (X > gap.X && X + Width < gap.X + gap.Width)
                {
                    Console.WriteLine("need to fall");
                    Fall();
                }

            }
        }
        
        
    }

    private  void Fall(int offset = 0)
    {
        var Gap = 24;
        if (IsFalling) return;
        IsJumping = false;
        IsJumpingReady = false;
        _fallTargetY = Y + Gap + offset;
        IsFalling = true;
    }

    private void FallJack()
    {
        Y = Y + 1;
        if (Y >= _fallTargetY)
        {
            Y = _fallTargetY;
            //Console.WriteLine("falling ended");
            IsFalling = false;
            IsJumping = false;
            Row++;
        }
    }

    void CheckForGapAbove()
    {
        if(!IsJumping) return;
        if (JumpThroughGap) return;
        if(Row == 0) return;
        foreach (var gap in Gaps)
        {
            if (gap.Row == Row - 1)
            {
                if (X > gap.X && X + Width < gap.X + gap.Width)
                {
                    JumpThroughGap = true;
                }    
            }
            else
            {
                if (Row > 0)
                {
                    if (Y < Roads[Row - 1].Y )
                    {
                        IsJumping = false;
                        IsJumpingReady = false;
                        JumpThroughGap = false;
                        Row--;
                        Fall(-Height + 1);
                    }    
                }
                else
                {
                    Console.WriteLine("Next Level");
                }



            }


        }
        

    }

    
}