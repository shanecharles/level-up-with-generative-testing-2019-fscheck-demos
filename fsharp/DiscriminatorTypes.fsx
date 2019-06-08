#r "../packages/FsCheck/lib/net452/FsCheck.dll"

open FsCheck

type PositiveMoney = PositiveMoney of decimal with
    member x.Get = match x with PositiveMoney v -> v
    override x.ToString() = x.Get.ToString()
    static member op_Explicit(PositiveMoney v) = v



type Money = Money of decimal with
    member x.Get = match x with Money v -> v
    override x.ToString() = x.Get.ToString()
    static member op_Explicit(Money v) = v



type EvenInt = EvenInt of int with
  static member op_Explicit(EvenInt i) = i

type ArbitraryModifiers =
    static member EvenInt() = 
        Arb.from<int> 
        |> Arb.filter (fun i -> i % 2 = 0) 
        |> Arb.convert EvenInt int
        
Arb.register<ArbitraryModifiers>()