#r "./packages/FsCheck/lib/net452/FsCheck.dll"

open FsCheck
open System




let calculateWithTax (salesAmount : decimal) =
    Math.Round (salesAmount + (salesAmount * 0.07M) + (salesAmount * 0.05M), 2)



let ``Calculate with tax is greater than or equal to sales amount property`` (saleAmount : decimal) = 
    saleAmount <= calculateWithTax saleAmount 




Check.Verbose ``Calculate with tax is greater than or equal to sales amount property``





let ``Calculate with tax is greater than or equal to sales amount property 2`` (PositiveInt saleAmount) = 
    let amount = decimal saleAmount
    amount <= calculateWithTax amount 




Check.Verbose ``Calculate with tax is greater than or equal to sales amount property 2``


