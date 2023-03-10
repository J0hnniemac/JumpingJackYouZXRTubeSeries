using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JumpingJackYouZXRTubeSeries;

public class JumpingJackGame : Game
{

    private readonly int NativeWidth = 320;
    private readonly int NativeHeight = 240;
    private readonly int SideBorder = 32;
    private readonly int TopBorder = 16;
    private readonly int DisplayMultiplier = 2;

    private static readonly Color BackgroundColor = Color.White;

    private RenderTarget2D _nativeRenderTarget2D;
    
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    
    //ROAD Info
    private Road[] _roads;
    private const int GapBetweenRoads = 24;
    private const int InitRoadGap = 3;

    //Gaps info
    
    private List<Gap> _gaps;
    
    //Jack Infor
    private Texture2D _jackTexture2D;
    private Jack _jack;
    
    //Border Texture
    private Texture2D _borderTexture2D;
    
    
    
    public JumpingJackGame()
    {
       
        
        
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _graphics.PreferredBackBufferWidth = NativeWidth * DisplayMultiplier;
        _graphics.PreferredBackBufferHeight = NativeHeight * DisplayMultiplier;

    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();

        _nativeRenderTarget2D = new RenderTarget2D(GraphicsDevice, NativeWidth, NativeHeight);
        InitRoads();
        
        InitFirst2Gaps();
        InitJack();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here
        _jackTexture2D = Content.Load<Texture2D>("JumpingJackSprites");
        // init empty border texture
        _borderTexture2D = new Texture2D(GraphicsDevice,1,1);
        _borderTexture2D.SetData(new[] {Color.White});
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        ReadJackInput();

        // TODO: Add your update logic here
        MoveGaps();
        _jack.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.SetRenderTarget(_nativeRenderTarget2D);
        GraphicsDevice.Clear(BackgroundColor);

        // TODO: Add your drawing code here
        _spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone);
        // Draw Stuff
        DrawRoads();
        DrawGaps();
        DrawJack();
        DrawBorders();
        
        _spriteBatch.End();
        
        
        // Copy to Screen Buffer
        GraphicsDevice.SetRenderTarget(null);
        
        _spriteBatch.Begin(SpriteSortMode.Immediate,BlendState.AlphaBlend,SamplerState.PointClamp,DepthStencilState.Default,RasterizerState.CullNone);

        _spriteBatch.Draw(_nativeRenderTarget2D, new Rectangle(0,0, NativeWidth * DisplayMultiplier,NativeHeight * DisplayMultiplier), BackgroundColor);
        
        _spriteBatch.End();
        base.Draw(gameTime);
    }

    void InitRoads()
    {
        _roads = new Road[8];
        for (int i = 0; i <_roads.Length; i++)
        {
            _roads[i] = new Road(SideBorder, 
                TopBorder + InitRoadGap + (GapBetweenRoads * i)
                , 256, 
                2, 
                Color.Red,_spriteBatch);

        }
    }

    void DrawRoads()
    {
        foreach (var r in _roads)
        {
            r.Draw();
        }
        
        
    }

    private void InitFirst2Gaps()
    {
        _gaps = new List<Gap>();
        var rowID = 1;
        var rowY = _roads[rowID].Y;
        _gaps.Add(new Gap(SideBorder,rowY,BackgroundColor,1,rowID,_spriteBatch));
        _gaps.Add(new Gap(NativeWidth - SideBorder - 16,rowY,BackgroundColor,0,rowID,_spriteBatch));

        
    }

    private void MoveGaps()
    {
        foreach (var gap in _gaps)
        {
            if (gap.Direction == 0)
            {
                gap.X -= 1;
            }
            else
            {
                gap.X += 1;
            }
            
            //check if gap is out of bounds
            if (gap.X < SideBorder - gap.Width)
            {
                gap.Row -= 1;
                if (gap.Row < 0) gap.Row = _roads.Length - 1;

                gap.Y = _roads[gap.Row].Y;
                gap.X = NativeWidth - SideBorder;
            }

            if (gap.X > NativeWidth - SideBorder)
            {
                //change row
                gap.Row += 1;
                if (gap.Row > _roads.Length - 1) gap.Row = 0;
                gap.Y = _roads[gap.Row].Y;
                gap.X = SideBorder - gap.Width;
            }


        }
        
        
    }

    void DrawGaps()
    {
        foreach (var g in _gaps)
        {
            g.Draw();
        }
    }
    


    void InitJack()
    {
        _jack = new Jack(SideBorder + 256 / 2 - 8, TopBorder + 192 - 13, 16, 16, Color.Aqua, 0, 8, _jackTexture2D, _gaps, _roads);
    }

    void DrawJack()
    {
     _jack.Draw(_spriteBatch);   
    }


    private void ReadJackInput()
    {
        if (_jack.IsJumping) return;
        if (_jack.IsFalling) return;
        var dirKeyPressed = false;
        //read keyboard left and right  
        if (Keyboard.GetState().IsKeyDown(Keys.Left))
        {
            _jack.MoveLeft();
            dirKeyPressed = true;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Right))
        {
            _jack.MoveRight();
            dirKeyPressed = true;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            _jack.Jump();
            dirKeyPressed = true;
            _jack.IsJumping = true;
        }
        
        if (!dirKeyPressed)
        {
            _jack.idle();
        }
    }

    void DrawBorders()
    {
        //Draw border to spritebuffer
        _spriteBatch.Draw(_borderTexture2D,new Rectangle(0,0,SideBorder,NativeHeight),BackgroundColor);
        _spriteBatch.Draw(_borderTexture2D,new Rectangle(NativeWidth - SideBorder ,0,SideBorder,NativeHeight),BackgroundColor);

        
    }
}