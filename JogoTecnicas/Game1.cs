using JogoTecnicas.Graficos;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace JogoTecnicas
{
    public class Game1 : Game
    {
        private const string ASSET_NAME_SPRITESHEET = "shaolin_running_strip";


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _spriteSheetTexture;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            _spriteSheetTexture = Content.Load<Texture2D>(ASSET_NAME_SPRITESHEET);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();


            Sprite Correr1 = new Sprite(_spriteSheetTexture, 6, 0, 42, 61);

            Correr1.Draw(_spriteBatch, new Vector2(10, 10));

           // _spriteBatch.Draw(_spriteSheetTexture, new Vector2(10, 10),new Rectangle(6,0,42,61), Color.White);


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
