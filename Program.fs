//these are similar to C# using statements
open canopy.runner.classic
open canopy.configuration
open canopy.classic
open OpenQA.Selenium.Chrome
open System.Threading.Tasks;
open System.Threading
open System.Collections.ObjectModel

//start an instance of chrome
chromeDir <- "./"

start chrome

let ctrlClickAll selector =
    elements selector
    |> List.iter (fun element -> ctrlClick element)

let seq (readonlyCollection: ReadOnlyCollection<string>) = 
    Seq.cast readonlyCollection |> Seq.mapi (fun f -> fun v -> f)
//this is how you define a test
"taking canopy for a spin" &&& fun _ ->
    //this is an F# function body, it's whitespace enforced

    //go to url
    url "https://elements.envato.com/sign-in"

    while currentUrl () = "https://elements.envato.com/sign-in" do
        Thread.Sleep(3000)
    
    url "https://elements.envato.com/stock-video"

    ctrlClickAll """a[class="_1wL0Kx7A"]"""
    
    seq browser.WindowHandles |> Seq.iter (fun i ->
        i+2 |> printfn "%d"
            |> ignore
        i+2 |> switchToTab
            |> ignore

        Thread.Sleep(3000)
    )
run()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()