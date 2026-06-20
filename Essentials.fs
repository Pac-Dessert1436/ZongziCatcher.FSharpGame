module ZongziCatcher.FSharpGame.Essentials

open FSharpEventAddons
open Microsoft.Xna.Framework.Audio
open Microsoft.Xna.Framework.Media

[<Literal>]
let SCREEN_WIDTH = 800

[<Literal>]
let SCREEN_HEIGHT = 600

[<Literal>]
let PLAYER_SPEED = 300.f

[<Literal>]
let SPAWN_INTERVAL = 1.5f

[<Literal>]
let BASE_ITEM_SPEED = 150.f

[<Literal>]
let POINTS_PER_COLLECT = 10

[<Literal>]
let POINTS_PER_DROP = -5

[<Literal>]
let INITIAL_LIVES = 3

let pes: PriorityEventScheduler = PriorityEventScheduler()

type GameEvents() =
    let mutable _zongziCollected = Event<unit>()
    let mutable _zongziDropped = Event<unit>()
    let mutable _playerHit = Event<unit>()
    let mutable _gameStarted = Event<unit>()
    let mutable _gameEnded = Event<unit>()

    [<CLIEvent>]
    member _.ZongziCollected = _zongziCollected.Publish

    [<CLIEvent>]
    member _.ZongziDropped = _zongziDropped.Publish

    [<CLIEvent>]
    member _.PlayerHit = _playerHit.Publish

    [<CLIEvent>]
    member _.GameStarted = _gameStarted.Publish

    [<CLIEvent>]
    member _.GameEnded = _gameEnded.Publish

    member _.TriggerZongziCollected() = _zongziCollected.Trigger()

    member _.TriggerZongziDropped() = _zongziDropped.Trigger()

    member _.TriggerPlayerHit() = _playerHit.Trigger()

    member _.TriggerGameStarted() = _gameStarted.Trigger()

    member _.TriggerGameEnded() = _gameEnded.Trigger()

type SoundManager() =
    let mutable _initialized = false
    let mutable _itemCollectedSound: SoundEffect option = None
    let mutable _dropSound: SoundEffect option = None
    let mutable _playerHitSound: SoundEffect option = None
    let mutable _gameOverSound: SoundEffect option = None
    let mutable _bgm: Song option = None

    member _.Initialized = _initialized

    member _.LoadSounds(content: Microsoft.Xna.Framework.Content.ContentManager) =
        if not _initialized then
            try
                _itemCollectedSound <- Some(content.Load<SoundEffect> "Sounds/item_collected")
            with _ ->
                ()

            try
                _dropSound <- Some(content.Load<SoundEffect> "Sounds/drop_into_water")
            with _ ->
                ()

            try
                _playerHitSound <- Some(content.Load<SoundEffect> "Sounds/player_hit")
            with _ ->
                ()

            try
                _gameOverSound <- Some(content.Load<SoundEffect> "Sounds/game_over")
            with _ ->
                ()

            try
                _bgm <- Some(content.Load<Song> "Sounds/BGM/main_theme")
            with _ ->
                ()

            _initialized <- true

    member _.PlayItemCollected() =
        _itemCollectedSound |> Option.iter (fun s -> s.Play() |> ignore)

    member _.PlayDrop() =
        _dropSound |> Option.iter (fun s -> s.Play() |> ignore)

    member _.PlayPlayerHit() =
        _playerHitSound |> Option.iter (fun s -> s.Play() |> ignore)

    member _.PlayGameOver() =
        _gameOverSound |> Option.iter (fun s -> s.Play() |> ignore)

    member _.PlayBGM() =
        if MediaPlayer.State <> MediaState.Playing then
            _bgm
            |> Option.iter (fun s ->
                MediaPlayer.Play(s)
                MediaPlayer.IsRepeating <- true)

    member _.StopBGM() = MediaPlayer.Stop()

let gameEvents: GameEvents = GameEvents()
let soundManager: SoundManager = SoundManager()
