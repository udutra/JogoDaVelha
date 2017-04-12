using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JogoDaVelha
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;

        private SpriteBatch spriteBatch;

        private Texture2D cellEmpty;
        private Texture2D cellO;
        private Texture2D cellX;

        private Vector2 cellPos;

        private MouseState prevMouseState;

        private int Width;
        private int Height;
        private int tamanhoImagem;

        private float countDown = 3;

        private Board board;

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

            cellEmpty = Content.Load<Texture2D>("CellEmpty");
            cellO = Content.Load<Texture2D>("CellO");
            cellX = Content.Load<Texture2D>("CellX");
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            {//simple timer sample
                countDown -= dt;
                if (countDown <= 0)
                {
                    countDown = 3;
                    System.Diagnostics.Debug.WriteLine("BOOM!");
                }
            }

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
            {
                Vector2 pTeste = new Vector2(Mouse.GetState().Position.X, Mouse.GetState().Position.Y);
                for (int y = 0; y < board.GetAlturaArray(); y++)
                {
                    for (int x = 0; x < board.GetAlturaArray(); x++)
                    {
                        Vector2 pMin = new Vector2(x * tamanhoImagem + (Width - tamanhoImagem * board.GetAlturaArray()) / 2, y * tamanhoImagem + (Height - tamanhoImagem * board.GetComprimentoArray()) / 2);
                        Vector2 pMax = pMin + new Vector2(tamanhoImagem, tamanhoImagem);

                        /*Vector2 pMax = pMin;
                        pMax.X += 64;
                        pMax.Y += 64;*/

                        if (((pTeste.X > pMin.X) && (pTeste.X < pMax.X)) && ((pTeste.Y > pMin.Y) && (pTeste.Y < pMax.Y)))
                        {

                            board.AcrescentaValor(x, y); //board.cell[x,y] +=  1;


                            if (board.RetornaValorCelula(x, y) > 2) //if(board.cell[x,y] > 2)
                            {
                                board.ZeraValorCelula(x, y); //board.cell[x, y] = 0;
                            }
                        }
                    }
                }
            }

            int ganhador = board.GetVencedor();

            if (ganhador > 0)
            {
                System.Diagnostics.Debug.WriteLine("Ganhador é: " + ganhador);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("jogo empatado!");
            }

            prevMouseState = Mouse.GetState();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            for (int y = 0; y < board.GetComprimentoArray(); y++)
            {
                for (int x = 0; x < board.GetAlturaArray(); x++)
                {
                    cellPos = new Vector2(x * tamanhoImagem + (Width - tamanhoImagem * board.GetAlturaArray()) / 2, y * tamanhoImagem + (Height - tamanhoImagem * board.GetComprimentoArray()) / 2);

                    spriteBatch.Draw(cellEmpty, cellPos);
                    

                    //if(board.cell[x,y] == 1){
                    if (board.RetornaValorCelula(x, y) == 1)
                    {
                        spriteBatch.Draw(cellX, cellPos);
                    }
                    //if (board.cell[x, y] == 2){
                    if (board.RetornaValorCelula(x, y) == 2)
                    {
                        spriteBatch.Draw(cellO, cellPos);
                    }
                }
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
