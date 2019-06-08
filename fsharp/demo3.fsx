#r "./packages/FsCheck/lib/net452/FsCheck.dll"
#load "./DiscriminatorTypes.fsx"

open FsCheck
open System
open DiscriminatorTypes





let calcAmountWithTax (salesAmount : decimal) =
    Math.Round (salesAmount + (salesAmount * 0.07M) + (salesAmount * 0.05M), 2)



Gen.sample 10 10 Arb.generate<Decimal>







let genChange = Gen.choose (0, 99)
                |> Gen.map (fun x -> (decimal x / 100M))

let genMoney = Gen.zip 
                  (Arb.generate<int>) 
                  (genChange)
               |> Gen.map (fun (p,d) -> (decimal p) + d )

let genDollar = Gen.constant 1M


Gen.sample 10 10 genChange
Gen.sample 1000 10 genMoney
Gen.sample 100 1000000 genDollar





type MoneyTypes =
    static member Money () =
        genMoney
        |> Arb.fromGen
        |> Arb.convert Money decimal

    static member PositiveMoney () = 
        genMoney
        |> Arb.fromGen
        |> Arb.mapFilter abs (fun d -> d > 0M)
        |> Arb.convert PositiveMoney decimal



Gen.sample 1000 10 (MoneyTypes.PositiveMoney () |> Arb.toGen)
Gen.sample 1000 10 (MoneyTypes.Money () |> Arb.toGen)




let config = { Config.Default with
                    Arbitrary = [typeof<MoneyTypes>]
                    EndSize = 100000
                    MaxTest = 1000 }



let calcAmountWithTax_greater_equal_property (PositiveMoney d) =
      let result = calcAmountWithTax d
      result > d




Check.One("calcAmountWithTax >= property", config, calcAmountWithTax_greater_equal_property)





let calcAmountWithTax_there_and_back_property (Money m) =
    let m' = m |> calcAmountWithTax 
               |> (fun st -> st / 1.12M)
               |> (fun s -> Math.Round(s, 2))
    m = m'


Check.One("Sales tax there and back", 
        { config with Every = Config.Verbose.Every },
        calcAmountWithTax_there_and_back_property)




let calcAmountWithTax_different_paths_same_destination_property (Money m) =
    Math.Round(m * (1.12M),2) = calcAmountWithTax m


Check.One("Sales tax different paths", config, calcAmountWithTax_different_paths_same_destination_property)