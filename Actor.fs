namespace ZongziCatcher.FSharpGame

open Microsoft.Xna.Framework
open Microsoft.Xna.Framework.Graphics

type ActorType =
    | DragonBoat = 0
    | Zongzi = 1
    | Scorpion = 2

type Actor(position: Vector2, speed: float32, actorType: ActorType) =
    let mutable _position = position
    let mutable _speed = speed
    let _actorType = actorType

    member val Id = System.Guid.NewGuid()

    member _.Position
        with get () = _position
        and set (value) = _position <- value

    member _.Speed
        with get () = _speed
        and set (value) = _speed <- value

    member _.Type = _actorType

    member _.SpriteSize =
        match _actorType with
        | ActorType.DragonBoat -> Vector2(130f, 80f)
        | _ -> Vector2(50f, 50f)

    member _.Update(gameTime: GameTime) =
        let dt = float32 gameTime.ElapsedGameTime.TotalSeconds
        _position <- Vector2(_position.X, _position.Y + _speed * dt)

    member this.Draw(spriteBatch: SpriteBatch, texture: Texture2D) =
        spriteBatch.Draw(
            texture,
            Rectangle(int _position.X, int _position.Y, int this.SpriteSize.X, int this.SpriteSize.Y),
            Color.White
        )

type Player(position: Vector2) =
    inherit Actor(position, 0.f, ActorType.DragonBoat)

    let mutable _lives = 3
    let mutable _score = 0

    member _.Score
        with get () = _score
        and set (value: int) = _score <- value

    member _.Lives
        with get () = _lives
        and set (value: int) = _lives <- value

    member this.MoveLeft(speed: float32, dt: float32, screenWidth: int) =
        let newX = max 0.f (this.Position.X - speed * dt)
        this.Position <- Vector2(newX, this.Position.Y)

    member this.MoveRight(speed: float32, dt: float32, screenWidth: int) =
        let maxX = float32 screenWidth - this.SpriteSize.X
        let newX = min maxX (this.Position.X + speed * dt)
        this.Position <- Vector2(newX, this.Position.Y)

type FallingItem(position: Vector2, speed: float32, itemType: ActorType) =
    inherit Actor(position, speed, itemType)

    member this.IsOffScreen(screenHeight: int) = this.Position.Y > float32 screenHeight

type WaterSplash(position: Vector2) =
    let mutable _position = position
    let mutable _lifeTime = 0.5f

    member _.Position = _position
    member _.LifeTime = _lifeTime

    member this.Update(gameTime: GameTime) =
        let dt = float32 gameTime.ElapsedGameTime.TotalSeconds
        _lifeTime <- _lifeTime - dt

    member _.IsDead = _lifeTime <= 0.f

    member _.Draw(spriteBatch: SpriteBatch, texture: Texture2D) =
        let alpha = int (_lifeTime / 0.5f * 255.f)

        spriteBatch.Draw(
            texture,
            Rectangle(int _position.X - 25, int _position.Y - 25, 50, 50),
            Color(255, 255, 255, alpha)
        )

type Caption(text: string, position: Vector2) =
    let mutable _text = text
    let mutable _position = position
    let mutable _lifeTime = 1.0f

    member _.Text = _text
    member _.Position = _position
    member _.LifeTime = _lifeTime

    member this.Update(gameTime: GameTime) =
        let dt = float32 gameTime.ElapsedGameTime.TotalSeconds
        _lifeTime <- _lifeTime - dt
        _position <- Vector2(_position.X, _position.Y - 20.f * dt)

    member this.IsDead = _lifeTime <= 0.f
