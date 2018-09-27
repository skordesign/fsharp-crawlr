//these are similar to C# using statements
open OpenQA.Selenium
open Core
open System.Runtime.InteropServices.ComTypes
open System
open System.IO

type Link ={
    Parent: string
    Children: List<string>
}

Console.WriteLine("Download tool Envato for chrome")
Console.WriteLine("Step 1 :Go to [https://elements.envato.com] and login")
Console.WriteLine("Step 2 :Press Windows + R")
Console.WriteLine("Step 3 : Paste to it [%LOCALAPPDATA%\Google\Chrome\User Data]")
Console.WriteLine("Step 4: Copy directory in windows explorer and paste here")
userdataPath <- Console.ReadLine ()
let mutable path = Environment.CurrentDirectory + "\\Downloads";

let envato = "https://elements.envato.com"
    
let driver = createDriver path

let downloadButtonInPanel = """button[data-test-selector="download-without-license"]"""
let menuContainer = """ul[class="vOWd8KiH"]"""
let downloadIcon = """button[data-test-selector="item-card-download-button"]"""


let getNextButton () =
    Seq.last (elements """a[class="_36I3CjyO _635CaDn5"]""")

let getFolderName (link:string) = 
    link.Replace(envato ,"").Replace("/","\\")

let countDownload () =
    if not (Directory.Exists(path)) then
        0
    else
        let files =  (Directory.GetFiles path)
                    |> Array.filter (fun (file:string) -> file.Contains("crdownload"))
        files.Length

let downloadDisplayed e =
    (driver.FindElementByCssSelector e).Displayed

let downloadDisapear e =
    (driver.FindElementByCssSelector e) = null

let download (e : IWebElement) =
    while countDownload () >= 5 do
        printfn "Downloading : %d items" (countDownload ())
        sleep 3
    e.Click () |> ignore
    while not (displayed downloadButtonInPanel) do
       sleep 1
    let button = element downloadButtonInPanel
    button.Click () |> ignore
    sleep 1
    while displayed downloadButtonInPanel do
       sleep 1

let rec downloadAll selector =
    elements selector
    |> Seq.iter (fun element -> 
    download element
    sleep 3)
    if not (getNextButton () = null) then
        (getNextButton ()).Click ()
        downloadAll selector

start driver

navigate (envato + "/sign-in")

while (currentUrl ()).Contains (envato + "/sign-in" ) do
    sleep 1

navigate envato

let menu =  element menuContainer 
let links = 
    childrensByTag (menu, "a") 
    |> Seq.map (fun f -> f.GetAttribute ("href")) 
    |> Seq.distinct
    |> List.ofSeq

let filterChilds (list:List<string>, p:string) =
    List.filter (fun (f:string) -> f.Contains(p) && not (f = p)) list

let filterParents list =
    List.filter (fun (f:string) -> (List.findIndex (fun l -> l = f) list = List.findIndex (fun (l:string) -> f.Contains l) list)) list
 
let structObj = filterParents links 
                |> List.map (fun p -> { 
                    Parent = p
                    Children = filterChilds (links, p)
                })

quit ()



let openBrowser link =
    let customDriver = createDriver (path + (getFolderName link))
    path <- (path + (getFolderName link))
    start customDriver
    navigate link
    downloadAll downloadIcon
    while countDownload () > 0 do
        sleep 3
    quit ()

structObj |> List.filter (fun i -> not (i.Parent.Contains("all-items")))
          |> List.iter (fun i -> 
          if i.Children.Length = 0 then
             openBrowser i.Parent   
          else
             for link in i.Children do
                openBrowser link
          )
