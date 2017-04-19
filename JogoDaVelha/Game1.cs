using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace JogoDaVelha
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;

        private Texture2D cellEmpty;
        private Texture2D cellO;
        private Texture2D cellX;

        private SpriteFont fonteNormal;

        private Vector2 cellPos;

        private MouseState prevMouseState;

        private int Width;
        private int Height;
        private int tamanhoImagem;

        private float countDown = 3, stateTimer;

        private Board board;

        private UIButton btSair, btStart;

        private enum GameState { Null, MainMenu, PlayerTurn, ComputerTurn, ShowResults};

        private GameState currGameState = GameState.Null;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Width = 640;
            Height = 400;
            tamanhoImagem = 64;

            board = new Board();

            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            cellEmpty   = Content.Load<Texture2D>("Sprites/CellEmpty");
            cellO       = Content.Load<Texture2D>("Sprites/CellO");
            cellX       = Content.Load<Texture2D>("Sprites/CellX");
            fonteNormal = Content.Load<SpriteFont>("Fonts/Normal");

            btSair = new UIButton(new Vector2(10, 70), new Vector2(128, 50), cellEmpty, "Sair", fonteNormal);
            btStart = new UIButton(new Vector2(10, 10), new Vector2(128, 50), cellEmpty, "Iniciar", fonteNormal);
            EnterGameState(GameState.MainMenu);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            /*float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            {//simple timer sample
                countDown -= dt;
                if (countDown <= 0)
                {
                    countDown = 3;
                    System.Diagnostics.Debug.WriteLine("BOOM!");
                }
            }*/

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            

            UpdateGameState(gameTime);

            prevMouseState = Mouse.GetState();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.NonPremultiplied);

            DrawGameState(gameTime);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void EnterGameState(GameState newState)
        {
            LeaveGameState();
            currGameState = newState;
            switch (currGameState)
            {
                case GameState.MainMenu:
                    {
                        board.Limpar();
                    }
                    break;

                case GameState.PlayerTurn:
                    {
                    }
                    break;

                case GameState.ComputerTurn:
                    {
                        List<Board> possibilidades = board.GetPossibilidades(1);

                        board = possibilidades[0];

                        if (board.EhFimDeJogo())
                        {
                            EnterGameState(GameState.ShowResults);
                        }
                        else
                        {
                            EnterGameState(GameState.PlayerTurn);
                        }

                    }
                    break;

                case GameState.ShowResults:
                    {
                        stateTimer = 3;
                    }
                    break;
            }
        }

        public void LeaveGameState()
        {
            switch (currGameState)
            {
                case GameState.MainMenu: { } break;
                case GameState.PlayerTurn: { } break;
                case GameState.ComputerTurn: { } break;
                case GameState.ShowResults: { } break;
            }
        }

        public void UpdateGameState(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            switch (currGameState)
            {
                case GameState.MainMenu: {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

                            if (btStart.TesteClick(pTeste))
                            {
                                EnterGameState(GameState.PlayerTurn);
                            }

                            if (btSair.TesteClick(pTeste))
                            {
                                Exit();
                            }
                        }
                    }
                    break;

                case GameState.PlayerTurn:
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                        {
                            Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);

                            for (int y = 0; y < board.GetAlturaArray(); y++)
                            {
                                for (int x = 0; x < board.GetAlturaArray(); x++)
                                {
                                    Vector2 pMin = new Vector2(x * tamanhoImagem + (Width - tamanhoImagem * board.GetAlturaArray()) / 2, y * tamanhoImagem + (Height - tamanhoImagem * board.GetComprimentoArray()) / 2);
                                    Vector2 pMax = pMin + new Vector2(tamanhoImagem, tamanhoImagem);

                                    if (((pTeste.X > pMin.X) && (pTeste.X < pMax.X)) && ((pTeste.Y > pMin.Y) && (pTeste.Y < pMax.Y)))
                                    {
                                        board.AtribuiValorCelula(x, y, 2);

                                        if (board.EhFimDeJogo())
                                        {
                                            EnterGameState(GameState.ShowResults);
                                        }
                                        else
                                        {
                                            EnterGameState(GameState.ComputerTurn);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case GameState.ComputerTurn: { } break;
                case GameState.ShowResults:
                    {
                        stateTimer -= dt;
                        if (stateTimer <= 0)
                        {
                            EnterGameState(GameState.MainMenu);
                        }
                    }
                    break;
            }
        }

        public void DrawGameState(GameTime gameTime)
        {
            switch (currGameState)
            {
                case GameState.MainMenu:
                    {
                        btSair.Draw(spriteBatch);
                        btStart.Draw(spriteBatch);
                        /*//string titulo = "Tic-Tac-Toe";
                        string titulo = "Tempo: " + (int)gameTime.TotalGameTime.TotalSeconds + " segundos";
                        Vector2 textSize = fonteNormal.MeasureString(titulo);
                        
                        spriteBatch.DrawString(
                            fonteNormal,
                            titulo,
                            new Vector2(Width * 0.5f, 40),    //position 
                            Color.White,            //color
                            0.0f,                   //rotation
                            textSize * 0.5f,        //origin (pivot)
                            Vector2.One,            //scale
                            SpriteEffects.None,
                            0.0f
                            );*/
                    }
                    break;

                case GameState.PlayerTurn:
                    {
                        DrawBoard();
                    }
                    break;

                case GameState.ComputerTurn:
                    {
                        DrawBoard();
                    }
                    break;

                case GameState.ShowResults: {

                        string titulo = "Fim do jogo";

                        if (board.GetVencedor() == 1)
                        {
                            titulo = titulo + " - O ganhador é o computador";
                        }
                        else
                        {
                            titulo = titulo + " - O ganhador é o jogador";
                        }

                        
                        Vector2 textSize = fonteNormal.MeasureString(titulo);
                        spriteBatch.DrawString(
                            fonteNormal,
                            titulo,
                            new Vector2(Width * 0.5f, 40),    //position 
                            Color.White,            //color
                            0.0f,                   //rotation
                            textSize * 0.5f,        //origin (pivot)
                            Vector2.One,            //scale
                            SpriteEffects.None,
                            0.0f
                            );
                        DrawBoard();
                    }
                    break;
            }
        }

        void DrawBoard()
        {
            for (int y = 0; y < board.GetComprimentoArray(); y++)
            {
                for (int x = 0; x < board.GetAlturaArray(); x++)
                {
                    cellPos = new Vector2(x * tamanhoImagem + (Width - tamanhoImagem * board.GetAlturaArray()) / 2, y * tamanhoImagem + (Height - tamanhoImagem * board.GetComprimentoArray()) / 2);

                    spriteBatch.Draw(cellEmpty, cellPos, Color.White);


                    //if(board.cell[x,y] == 1){
                    if (board.RetornaValorCelula(x, y) == 1)
                    {
                        spriteBatch.Draw(cellX, cellPos, Color.Red);
                    }
                    //if (board.cell[x, y] == 2){
                    if (board.RetornaValorCelula(x, y) == 2)
                    {
                        spriteBatch.Draw(cellO, cellPos, Color.Yellow);
                    }
                }
            }
        }

    }
}
