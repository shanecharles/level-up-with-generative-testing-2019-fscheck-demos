#r "./packages/FsCheck/lib/net452/FsCheck.dll"

open FsCheck


// The power of shrinking!!


let calculateSum xs =
    if xs |> Seq.map abs |> Seq.windowed 2 |> Seq.exists ((=) [|23;17|])
    then 0
    else xs |> Seq.sum




let ``Shrinking for calculateSum`` (xs : int list) =
    let expected = (0,xs) ||> Seq.fold (+)
    expected = calculateSum xs




let config = { Config.Default with
                EndSize = 10000
                MaxTest = 1000 }




Check.One("Shrinking", config, ``Shrinking for calculateSum``)



let failConfig = { Config.Default with
                    EndSize = 10000
                    MaxTest = 1000
                    Replay = Some <| Random.StdGen (1240726311,296594816)
                    EveryShrink = Config.Verbose.EveryShrink }

Check.One("Shrinking", failConfig, ``Shrinking for calculateSum``)