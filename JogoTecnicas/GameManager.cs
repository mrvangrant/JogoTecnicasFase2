using Microsoft.Xna.Framework;
using JogoTecnicas.Graficos;

namespace JogoTecnicas
{
    public class GameManager
    {
        public void RestartGame(Game1 game)
        {
            // Reinicializa o jogador
            var runAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun,
                320,
                game.FrameWidth,
                game.FrameHeight,
                game.TotalFrames,
                game.TimePerFrame
            );

            var jumpAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun,
                448,
                game.FrameWidth,
                game.FrameHeight,
                game.TotalFrames,
                game.TimePerFrame
            );

            var slideAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun,
                384,
                game.FrameWidth,
                game.FrameHeight,
                game.TotalFrames + 1,
                game.TimePerFrame
            );

            var idleAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun,
                0,
                game.FrameWidth,
                game.FrameHeight,
                4,
                game.TimePerFrame
            );

            game.Player = new Player(
                runAnimation,
                jumpAnimation,
                slideAnimation,
                idleAnimation,
                new Vector2(180, game.FloorY - game.FrameHeight)
            );

            // Redefine as caixas de colisão
            game.Player.SetRunCollisionBox(new Rectangle(15, 20, 40, 45)); // Colisão para corrida
            game.Player.SetJumpCollisionBox(new Rectangle(15, 5, 35, 50)); // Colisão para salto
            game.Player.SetSlideCollisionBox(new Rectangle(10, 30, 45, 35)); // Colisão para deslizar
            game.Player.SetIdleCollisionBox(new Rectangle(20, 15, 20, 50)); // Colisão para deslizar

            // Reinicializa os obstáculos
            game.Obstacles = new Obstacles(game.ObstacleTexture);

            // Reinicializa o placar
            game._score.Reset();
            game.Wall.Reset();



            // Reinicia o estado de Game Over
            game.IsGameOver = false;
        }
    }

}
