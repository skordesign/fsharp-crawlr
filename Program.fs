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

let path = Environment.CurrentDirectory + "Downloads";

let envato = "https://elements.envato.com"
    
let driver = createDriver path

let downloadButtonInPanel = """button[data-test-selector="download-without-license"]"""
let menuContainer = """ul[class="vOWd8KiH"]"""
let downloadIcon = """button[data-test-selector="item-card-download-button"]"""

let getFolderName (link:string) = 
    link.Replace(envato ,"").Replace("/","\\")

let countDownload () =
    let files =  (Directory.GetFiles path)
                |> Array.filter (fun (file:string) -> file.Contains("crdownload"))
    files.Length

let downloadDisplayed e =
    (driver.FindElementByCssSelector e).Displayed

let downloadDisapear e =
    (driver.FindElementByCssSelector e) = null

let download (e : IWebElement) =
    e.Click () |> ignore
    let button = element downloadButtonInPanel
    button.Click () |> ignore
    let btn2 = element downloadButtonInPanel
    while btn2 = null do
       sleep 1

let downloadAll selector =
    elements selector
    |> Seq.iter (fun element -> 
    download element
    sleep 3)

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
    start customDriver
    navigate link
    downloadAll downloadIcon
    quit ()

structObj |> List.filter (fun i -> not (i.Parent.Contains("all-items")))
          |> List.iter (fun i -> 
          if i.Children.Length = 0 then
             openBrowser i.Parent   
          else
             for link in i.Children do
                openBrowser link
          )
