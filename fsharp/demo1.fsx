#I "./packages/FsCheck/lib/net452/"
#r "FsCheck.dll"
#load "./DiscriminatorTypes.fsx"

open FsCheck
open DiscriminatorTypes

let ``Equality equals property`` s =
    s = s


Check.Verbose ``Equality equals property``


let ``Multiplication even property`` (EvenInt x, y: int) =
        (0 = (x * y) % 2 )

Check.Verbose ``Multiplication even property``




let evilMultiplication x y =
    if y % 2 = 0 
    then x * (y + 1)
    else x * y



// This property will hold true.
let ``evilMultiplication even property`` (EvenInt x, y) =
    let result = evilMultiplication x y
    0 = result % 2


Check.Verbose ``evilMultiplication even property``


// This property catches the issue.
let ``evilMultiplication commutative property`` (x, y) =
    let r1 = evilMultiplication x y
    let r2 = evilMultiplication y x
    r1 = r2

Check.Verbose ``evilMultiplication commutative property``