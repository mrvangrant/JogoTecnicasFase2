using Microsoft.Xna.Framework;
using JogoTecnicas.Graficos;

namespace JogoTecnicas
{
    public class GameManager
    {
        public void RestartGame(Game1 game)
        {
            // Reinicializa as animações do player
            var runAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun, 320, game.FrameWidth, game.FrameHeight, game.TotalFrames, game.TimePerFrame);
            var jumpAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun, 448, game.FrameWidth, game.FrameHeight, game.TotalFrames, game.TimePerFrame);
            var slideAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun, 384, game.FrameWidth, game.FrameHeight, game.TotalFrames + 1, game.TimePerFrame);

            game.Player = new Player(runAnimation, jumpAnimation, slideAnimation, new Vector2(180, game.FloorY - game.FrameHeight));
            game.Obstacles = new Obstacles(game.ObstacleTexture);
            game.Buildings = new Buildings(game.BackgroundTexture, game.FloorTexture, game.ScreenWidth, game.ScreenHeight);
            game.IsGameOver = false;
        }

    }
}
