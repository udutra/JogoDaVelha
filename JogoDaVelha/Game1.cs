using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace JogoDaVelha
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;

        private Texture2D cellEmpty, cellO, cellX, rbSelecionado, rbNaoSelecionado, grafico, btCima, btBaixo, volMais, volMenos, mute;

        private SoundEffect sndGood, sndElephant, sndMistery, sndSong;
        private SoundEffectInstance playSound;

        private SpriteFont fonteNormal, fonteRadioButton;

        private Vector2 cellPos;

        private MouseState prevMouseState;

        private int Width, Height, tamanhoImagem, nivel;
        private Boolean boolMute;

        private float stateTimer, volume;

        private Board board;

        private UIButton btSair, btStart, btAumentar, btDiminuir, btMute, btVolMais, btVolMenos;
        private UIRadioButton rbComputador, rbUsuario;

        private enum GameState { Null, MainMenu, PlayerTurn, ComputerTurn, ShowResults};

        private GameState currGameState = GameState.Null;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Width = 640;
            Height = 400;
            tamanhoImagem = 64;
            nivel = 1;
            boolMute = false;
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

            cellEmpty           = Content.Load<Texture2D>("Sprites/CellEmpty");
            cellO               = Content.Load<Texture2D>("Sprites/CellO");
            cellX               = Content.Load<Texture2D>("Sprites/CellX");
            rbSelecionado       = Content.Load<Texture2D>("Sprites/rb32_32");
            rbNaoSelecionado    = Content.Load<Texture2D>("Sprites/rb32_32_ns");
            grafico             = Content.Load<Texture2D>("Sprites/grafico1_sf");
            mute                = Content.Load<Texture2D>("Sprites/red-listen-32");
            volMais             = Content.Load<Texture2D>("Sprites/red-volume-up-32");
            volMenos            = Content.Load<Texture2D>("Sprites/red-volume-down-32");
            btCima              = Content.Load<Texture2D>("Sprites/botaoCima_sf");
            btBaixo             = Content.Load<Texture2D>("Sprites/botaoBaixo_sf");
            fonteNormal         = Content.Load<SpriteFont>("Fonts/Normal");
            fonteRadioButton    = Content.Load<SpriteFont>("Fonts/RadioButton");
            sndGood             = Content.Load<SoundEffect>("Sounds/ir_begin");
            sndElephant         = Content.Load<SoundEffect>("Sounds/ir_end");
            sndMistery          = Content.Load<SoundEffect>("Sounds/ir_inter");
            sndSong             = Content.Load<SoundEffect>("Sounds/Speech_Off");
            
            playSound           = sndSong.CreateInstance();
            playSound.IsLooped  = true;
            playSound.Volume = 0.5f;
            playSound.Play();

            btSair          = new UIButton(new Vector2(10, 340), new Vector2(128, 50), cellEmpty, "Sair", fonteNormal);
            btStart         = new UIButton(new Vector2(10, 80), new Vector2(128, 50), cellEmpty, "Iniciar", fonteNormal);
            btAumentar      = new UIButton(new Vector2(450, 280), new Vector2(18, 18), btCima, "", fonteNormal);
            btDiminuir      = new UIButton(new Vector2(450, 300), new Vector2(18, 18), btBaixo, "", fonteNormal);
            btMute          = new UIButton(new Vector2(500, 358), new Vector2(32, 32), mute, "", fonteNormal);
            btVolMais       = new UIButton(new Vector2(545, 358), new Vector2(32, 32), volMais, "", fonteNormal);
            btVolMenos      = new UIButton(new Vector2(590, 358), new Vector2(32, 32), volMenos, "", fonteNormal);
            rbComputador    = new UIRadioButton(new Vector2(10, 200), new Vector2(20, 20), rbSelecionado, "", fonteNormal, true);
            rbUsuario       = new UIRadioButton(new Vector2(10, 237), new Vector2(20, 20), rbNaoSelecionado, "", fonteNormal, false);
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

                            int bestScore = -1;
                            Board bestBoard = null;

                            foreach (Board p in possibilidades)
                            {
                                int aux = board.Minimax(p, 9, 2);

                                System.Console.WriteLine("Aux = " + aux);

                                if (aux > bestScore)
                                {
                                    bestScore = aux;
                                    bestBoard = p;
                                }

                            }

                            
                            board = bestBoard;
                        
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
                        sndMistery.Play();
                    }
                    break;
            }
        }

        public void LeaveGameState()
        {
            switch (currGameState)
            {
                case GameState.MainMenu: { playSound.Stop(); } break;
                case GameState.PlayerTurn: { } break;
                case GameState.ComputerTurn: { } break;
                case GameState.ShowResults: { playSound.Play(); } break;
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
                                sndGood.Play();
                                EnterGameState(GameState.PlayerTurn);
                            }

                            if (btSair.TesteClick(pTeste))
                            {
                                Exit();
                            }

                            if (rbUsuario.TesteClick(pTeste))
                            {
                                if (rbUsuario.getCheck() == false && rbComputador.getCheck() == true)
                                    {
                                    rbUsuario.setBackground(rbSelecionado);
                                    rbUsuario.setCheck(true);
                                    rbComputador.setBackground(rbNaoSelecionado);
                                    rbComputador.setCheck(false);
                                }
                                
                            }

                            if (rbComputador.TesteClick(pTeste))
                            {
                                if (rbUsuario.getCheck() == true && rbComputador.getCheck() == false)
                                {
                                    rbUsuario.setBackground(rbNaoSelecionado);
                                    rbUsuario.setCheck(false);
                                    rbComputador.setBackground(rbSelecionado);
                                    rbComputador.setCheck(true);
                                }
                            }

                            if (btAumentar.TesteClick(pTeste))
                            {
                                if (nivel == 1)
                                {
                                    grafico = Content.Load<Texture2D>("Sprites/grafico2_sf");
                                    nivel++;
                                }
                                else if(nivel == 2)
                                {
                                    grafico = Content.Load<Texture2D>("Sprites/grafico3_sf");
                                    nivel++;
                                }
                                else
                                {

                                }
                            }
                            if (btDiminuir.TesteClick(pTeste))
                            {
                                if (nivel == 3)
                                {
                                    grafico = Content.Load<Texture2D>("Sprites/grafico2_sf");
                                    nivel--;
                                }
                                else if (nivel == 2)
                                {
                                    grafico = Content.Load<Texture2D>("Sprites/grafico1_sf");
                                    nivel--;
                                }
                                else
                                {
                                    
                                }
                            }

                            if (btMute.TesteClick(pTeste))
                            {
                                if (boolMute == false)
                                {
                                    mute = Content.Load<Texture2D>("Sprites/red-not-listen-32");
                                    btMute.setBackground(mute);
                                    playSound.Pause();
                                    boolMute = true;
                                }
                                else
                                {
                                    mute = Content.Load<Texture2D>("Sprites/red-listen-32");
                                    btMute.setBackground(mute);
                                    playSound.Play();
                                    boolMute = false;
                                    if (playSound.Volume == 0){
                                        playSound.Volume = playSound.Volume + 0.1f;
                                    }
                                }
                            }

                            if (btVolMais.TesteClick(pTeste))
                            {
                                if (playSound.Volume > 0.9)
                                {
                                    playSound.Volume = 1;
                                }

                                else if (playSound.Volume < 1 )
                                {
                                    if (playSound.Volume == 0)
                                    {
                                        mute = Content.Load<Texture2D>("Sprites/red-listen-32");
                                        btMute.setBackground(mute);
                                        playSound.Play();
                                        boolMute = false;
                                        playSound.Volume = playSound.Volume + 0.1f;
                                    }
                                    else
                                    {
                                        playSound.Volume = playSound.Volume + 0.1f;
                                    }

                                }

                            }

                            if (btVolMenos.TesteClick(pTeste))
                            {
                                if (playSound.Volume > 0 && playSound.Volume < 0.2)
                                {
                                    playSound.Volume = 0;
                                    mute = Content.Load<Texture2D>("Sprites/red-not-listen-32");
                                    btMute.setBackground(mute);
                                    playSound.Pause();
                                    boolMute = true;
                                }

                                else if (playSound.Volume > 0)
                                {
                                    playSound.Volume = playSound.Volume - 0.1f;
                                }

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

                                    if (((pTeste.X > pMin.X) && (pTeste.X < pMax.X)) && ((pTeste.Y > pMin.Y) && (pTeste.Y < pMax.Y)) && board.RetornaValorCelula(x,y) == 0)
                                    {
                                        board.AtribuiValorCelula(x, y, 2);
                                        sndElephant.Play(1.0f, -1.0f, 0.0f);

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

                        float t = (float)(gameTime.TotalGameTime.TotalSeconds * MathHelper.Pi * 1);

                        float alpha = (float)Math.Abs(Math.Sin(t));

                        DrawStringT("Jogo da Velha", new Vector2(320, 40), new Color(1.0f, 1.0f, 1.0f, alpha));

                        DrawString("Computador começa jogando", new Vector2(50, 205), new Color(1.0f, 1.0f, 1.0f, 1));
                        DrawString("Jogagor começa jogando",    new Vector2(50, 241), new Color(1.0f, 1.0f, 1.0f, 1));
                        

                        btSair.Draw(spriteBatch);
                        btStart.Draw(spriteBatch);
                        btAumentar.Draw(spriteBatch);
                        btDiminuir.Draw(spriteBatch);
                        btMute.Draw(spriteBatch);
                        btVolMais.Draw(spriteBatch);
                        btVolMenos.Draw(spriteBatch);
                        rbComputador.Draw(spriteBatch);
                        rbUsuario.Draw(spriteBatch);

                        DrawString("Quem começa jogando", new Vector2(10, 155), new Color(1.0f, 1.0f, 1.0f, 1));
                        DrawString("Dificuldade", new Vector2(500, 155), new Color(1.0f, 1.0f, 1.0f, 1));

                        spriteBatch.Draw(grafico, new Vector2(490,200), Color.White);
                        

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
                        else if(board.GetVencedor() == 2)
                        {
                            titulo = titulo + " - O ganhador é o jogador";
                        }
                        else
                        {
                            titulo = titulo + " - Jogo empatado";
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

        public void DrawBoard()
        {
            for (int x = 0; x < board.GetAlturaArray(); x++)
            {
                for (int y = 0; y < board.GetComprimentoArray(); y++)
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

        private void DrawStringT(string text, Vector2 pos, Color color)
        {
            Vector2 textSize = fonteNormal.MeasureString(text);

            spriteBatch.DrawString(
                fonteNormal,
                text,
                pos,                //position 
                color,              //color
                0.0f,               //rotation
                textSize / 2.0f,    //origin (pivot)
                Vector2.One,        //scale
                SpriteEffects.None,
                0.0f
            );
        }

        private void DrawString(string text, Vector2 pos, Color color)
        {
            Vector2 textSize = fonteNormal.MeasureString(text);

            spriteBatch.DrawString(
                fonteRadioButton,
                text,
                pos,                //position 
                color,              //color
                0.0f,               //rotation
                new  Vector2(0, 0),    //origin (pivot)
                Vector2.One,        //scale
                SpriteEffects.None,
                0.0f
            );
        }

    }
}
