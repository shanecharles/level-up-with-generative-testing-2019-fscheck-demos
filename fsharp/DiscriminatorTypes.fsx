#I "./packages/FsCheck/lib/net452/"
#r "FsCheck.dll"

open FsCheck

type PositiveMoney = PositiveMoney of decimal with
    member x.Get = match x with PositiveMoney v -> v
    override x.ToString() = x.Get.ToString()
    static member op_Explicit(PositiveMoney v) = v



type Money = Money of decimal with
    member x.Get = match x with Money v -> v
    override x.ToString() = x.Get.ToString()
    static member op_Explicit(Money v) = v

let private genChange_internal = Gen.choose (0, 99)
                                 |> Gen.map (fun x -> (decimal x / 100M))

let private genMoney_internal = Gen.zip 
                                  (Arb.generate<int>) 
                                  (genChange_internal)
                                |> Gen.map (fun (p,d) -> (decimal p) + d )

type MoneyTypes =
    static member Money () =
        genMoney_internal
        |> Arb.fromGen
        |> Arb.convert Money decimal

    static member PositiveMoney () = 
        genMoney_internal
        |> Arb.fromGen
        |> Arb.mapFilter abs (fun d -> d > 0M)
        |> Arb.convert PositiveMoney decimal


type EvenInt = EvenInt of int with
  static member op_Explicit(EvenInt i) = i

type ArbitraryModifiers =
    static member EvenInt() = 
        Arb.from<int> 
        |> Arb.filter (fun i -> i % 2 = 0) 
        |> Arb.convert EvenInt int
        
Arb.register<ArbitraryModifiers>()