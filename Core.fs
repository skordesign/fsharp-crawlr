module Core

open System.Threading
open OpenQA.Selenium.Chrome
open OpenQA.Selenium
open OpenQA.Selenium.Interactions

let mutable driver: ChromeDriver = null

let createDriver pathDownload = 
    
    let chromeOptions = OpenQA.Selenium.Chrome.ChromeOptions()
    chromeOptions.AddUserProfilePreference ("download.default_directory", pathDownload)
    chromeOptions.AddUserProfilePreference ("profile.default_content_settings.popups", 0)
    chromeOptions.AddArguments ("--start-maximized");
    chromeOptions.AddArgument (@"--user-data-dir=C:\Workspace\Empty\User Data")

    driver <- new ChromeDriver(".",chromeOptions)
    driver

let sleep seconds =
     Thread.Sleep(1000*seconds)


let ctrlClick selector =
    let ele = driver.FindElementByCssSelector selector
    let action = new Actions(driver)
    action.KeyDown(Keys.Control).Click(ele).KeyUp(Keys.Control).Build().Perform()

let elements selector = 
    driver.FindElementsByCssSelector selector

let element selector =
    driver.FindElementByCssSelector selector

let navigate url = 
    driver.Url <- url

let currentUrl ()=
    driver.Url
let quit ()= 
    driver.Quit ()
let start (browser: ChromeDriver) =
    browser.Navigate() |> ignore

let childrens (e: IWebElement, selector:string) = 
    e.FindElements (By.CssSelector selector)

let children (e: IWebElement, selector:string) =
    e.FindElement (By.CssSelector selector)

let childrensByTag (e: IWebElement, selector:string) = 
    e.FindElements (By.TagName selector)