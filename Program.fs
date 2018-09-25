//these are similar to C# using statements
open canopy.runner.classic
open canopy.configuration
open canopy.classic
open canopy
open OpenQA.Selenium.Chrome
open OpenQA.Selenium.Support.UI
open System
open OpenQA.Selenium

//start an instance of chrome
chromeDir <- "."

let chromeOptions = OpenQA.Selenium.Chrome.ChromeOptions()
chromeOptions.AddUserProfilePreference ("download.default_directory", @"C:\Workspace\Downloads")


let driver = new ChromeDriver(".",chromeOptions)


let downloadButtonInPanel = """button[data-test-selector="download-without-license"]"""

let downloadDisplayed e =
    (driver.FindElementByCssSelector e).Displayed

let downloadDisapear e =
    (driver.FindElementByCssSelector e) = null

let download (e : IWebElement) =
    e.Click() |> ignore
    let button = driver.FindElementByCssSelector(downloadButtonInPanel)
    button.Click() |> ignore
    let btn2 = driver.FindElementByCssSelector(downloadButtonInPanel)
    while btn2 = null do
       sleep 1

let ctrlClickAll selector =
    driver.FindElementsByCssSelector selector
    |> Seq.iter (fun element -> 
    download element
    sleep 3
    )
    

//this is how you define a test
"Travel to envato" &&& fun _ ->
    //this is an F# function body, it's whitespace enforced

    //go to url
    driver.Navigate().GoToUrl "https://elements.envato.com/sign-in"

    while driver.Url = "https://elements.envato.com/sign-in" do
        sleep 3
    
    driver.Url <- "https://elements.envato.com/stock-video"

    ctrlClickAll """button[data-test-selector="item-card-download-button"]"""
    
run()

printfn "press [enter] to exit"
System.Console.ReadLine() |> ignore

quit()