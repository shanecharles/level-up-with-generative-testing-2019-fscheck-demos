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








type Person = { Name: string; Shirt: string; Pants: string; Balance: decimal }

Gen.sample 10 10 Arb.generate<Person>




let colors = ["Orange"; "Red"; "Green"; "Blue"; "Beige"; "Tope"; "Salmon"; "Violet"; "Yellow"]
let names = ["Bob"; "Chuck"; "Kenworth"; "Shirley";
            "Susan"; "Dawn"; "Tim"; "Tammy";
            "Sir Purrs A Lot"; "Heather";
            "Hamilton"; "Kenny"]







let genPerson = 
    gen {
        let! name = Gen.elements names
        let! shirt = Gen.frequency [(3,Gen.constant "Orange"); (1,Gen.elements colors)]
        let! pants = Gen.elements colors 
        let! balance = genMoney 
        
        return {Name=name; Shirt=shirt; Pants=pants; Balance=balance}
    }


Gen.sample 10 10 genPerson








Gen.sample 1000 10 (MoneyTypes.PositiveMoney () |> Arb.toGen)
Gen.sample 1000 10 (MoneyTypes.Money () |> Arb.toGen)








let calcAmountWithTax_greater_equal_property (PositiveMoney d) =
      let result = calcAmountWithTax d
      result > d




let config = { Config.Default with
                    Arbitrary = [typeof<MoneyTypes>]
                    EndSize = 100000
                    MaxTest = 1000 }


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