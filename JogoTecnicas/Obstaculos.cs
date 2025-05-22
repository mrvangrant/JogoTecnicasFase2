using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace JogoTecnicas
{
    public class Obstaculos
    {
        public Vector2 Position;
        public Texture2D Texture;


        public Obstaculos(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public Rectangle BoundingBox =>
        new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        public void Update(float speed, float deltaTime)
        {
            // Move o obstáculo para a esquerda
            Position.X -= speed * deltaTime;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }


    }
}
