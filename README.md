# TrbalhoPraticoTecnicas2

Foi feito um Endless Runner em que o objetivo é sobreviver o máximo de tempo possível enquanto que se é perseguido por uma parede de espinhos e se desvia de obstáculos saltando e deslizando.

# Content #

Os assets utilizados foram retirados, com permissão, dos seguintes links:  

Colocar aqui os links depois

# Gameplay #

O nosso jogo baseia-se num sidescroller em que o personagem tem de fugir de uma parede de espinhos que nunca para de andar, enquanto se desvia de enemigos que aprecem com uma frequencia que vai aumentando.
Para sobreviver o personagem utilizade as habilidades de correr, saltar e deslizar.
O ato de deslizar serve para desviar dos enimigos que voam, enquanto que o saltar serve para desviar dos enimigos que estao no chao.


## Sprites ##



# GameLoop #

##Game1##


----------------------------------------------------------------------

##Player##

No inicio da classe, initializam-se as variaveis todas para as animacoes e os estados dos personagens, tal como as variaveis para as hurtboxes.

Na Func "public Player" atribui-se as variaveis das animacoes inicializadas no inicio da classe os respetivos sprites/animacoes carreagdas na classe game1 e atribui-se a cada animacao a sua hurtbox.

```
   public Player(SpriteAnimation runAnimation, SpriteAnimation jumpAnimation, SpriteAnimation slideAnimation, SpriteAnimation idleAnimation, SpriteAnimation deathAnimation, Vector2 startPosition)
        {
            _runAnimation = runAnimation;
            _jumpAnimation = jumpAnimation;
            _slideAnimation = slideAnimation;
            _idleAnimation = idleAnimation;
            _currentAnimation = _idleAnimation;
            _deathAnimation = deathAnimation;
            _position = startPosition;

            _runCollisionBox = new Rectangle(0, 0, _runAnimation.FrameWidth, _runAnimation.FrameHeight);
            _jumpCollisionBox = new Rectangle(0, 0, _jumpAnimation.FrameWidth, _jumpAnimation.FrameHeight);
            _slideCollisionBox = new Rectangle(0, 0, _slideAnimation.FrameWidth, _slideAnimation.FrameHeight);
            _idleCollisionBox = new Rectangle(0, 0, _idleAnimation.FrameWidth, _idleAnimation.FrameHeight);
            
        }
```



----------------------------------------------------------------------
##KeyboardInput##

##Buildings##

##Collisions##

##Wall##

##Enemy##

##EnemiesManage##

##SpriteAnimation##

##GameState##

##GameManager##

##Camara##

