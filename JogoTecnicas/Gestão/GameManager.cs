using Microsoft.Xna.Framework;
using JogoTecnicas.Graficos;
using JogoTecnicas.Inimigos;
using JogoTecnicas.Objetos;

namespace JogoTecnicas.Gestão
{
    public class GameManager
    {
        public void RestartGame(Game1 game)
        {
            // Reinicializa o jogador
            var runAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun,
                193,
                game.FrameWidth,
                game.FrameHeight,
                game.TotalFrames,
                game.TimePerFrame
            );

            var jumpAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun,
                129,
                game.FrameWidth,
                game.FrameHeight,
                game.TotalFrames,
                game.TimePerFrame
            );

            var slideAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun,
                257,
                game.FrameWidth,
                game.FrameHeight,
                game.TotalFrames,
                game.TimePerFrame
            );

            var idleAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun,
                65,
                game.FrameWidth,
                game.FrameHeight,
                4,
                game.TimePerFrame
            );

            var deathAnimation = new SpriteAnimation(
                game.SpriteSheetTextureRun,
                0,
                game.FrameWidth,
                game.FrameHeight,
                game.TotalFrames,
                game.TimePerFrame
            );

            game.Player = new Player(
                runAnimation,
                jumpAnimation,
                slideAnimation,
                idleAnimation,
                deathAnimation,
                new Vector2(180, game.FloorY - game.FrameHeight)
            );


            // Redefine as caixas de colisão do jogador
            game.Player.SetRunCollisionBox(new Rectangle(15, 20, 40, 45)); // Colisão para corrida
            game.Player.SetJumpCollisionBox(new Rectangle(15, 5, 35, 50)); // Colisão para salto
            game.Player.SetSlideCollisionBox(new Rectangle(10, 50, 45, 15)); // Colisão para deslizar
            game.Player.SetIdleCollisionBox(new Rectangle(20, 15, 20, 50)); // Colisão para idle

            // Reinicializa os inimigos
            var voador = new SpriteAnimation(game.Voador, 65, 64, 64, 6, 0.1f);
            var caveira = new SpriteAnimation(game.Chao, 65, 64, 64, 8, 0.2f);
            var caveiramorre = new SpriteAnimation(game.Chao, 0, 64, 64, 6, 0.2f);
            var voadormorre = new SpriteAnimation(game.Voador, 0, 64, 64, 6, 0.1f);
            caveiramorre.Loop = false;
            voadormorre.Loop = false;
            game.Enemies = new EnemiesManage(voador, caveira,voadormorre, caveiramorre);
            game.Enemies.Reset();

            // Reinicializa o placar
            game._score.Reset();

            game.ResetCamera();
            // Reinicializa a parede
            game.Wall.Reset();

            // Reinicializa os prédios e o chão
            game.Buildings = new Buildings(
                game.BackgroundTexture,
                game.FloorTexture,
                game.ScreenWidth,
                game.ScreenHeight
            );


            // Reinicia o estado de Game Over
            game.IsGameOver = false;
        }


    }

}
