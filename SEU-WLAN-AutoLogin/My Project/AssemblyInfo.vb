Imports System
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Globalization
Imports System.Resources
Imports System.Windows

' 組件的一般資訊是由下列的屬性集控制。
' 變更這些屬性的值即可修改組件的相關
' 資訊。

' 檢閱組件屬性的值

<Assembly: AssemblyTitle("SEU-WLAN AutoLogin")> 
<Assembly: AssemblyDescription("SEU-WLAN AutoLogin")> 
<Assembly: AssemblyCompany("PC-DOS Workshop")> 
<Assembly: AssemblyProduct("SEU-WLAN AutoLogin")> 
<Assembly: AssemblyCopyright("Copyright © 2013-2022 PC-DOS Workshop")> 
<Assembly: AssemblyTrademark("PC-DOS Workshop")> 
<Assembly: ComVisible(false)>

'為了建置可當地語系化的應用程式，請設定 
'<UICulture>CultureYouAreCodingWith</UICulture> 在您的 .vbproj 檔案中
'的 <PropertyGroup> 內。例如，假設您要在原始程式檔中
'使用美式英文，請將 <UICulture> 設定為 [en-US]。然後取消註解下方的
'NeutralResourceLanguage 屬性。更新下面一行中的 [en-US]
'以配合專案檔中的 UICulture 設定。

'<Assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)> 


'ThemeInfo 屬性說明在哪裡可以找到任何主題專屬及泛型資源字典。
'第 1 個參數:  主題專屬資源字典的所在位置
'(使用於資源不在此頁面、
' 或應用程式資源字典)

'第 2 個參數:  泛型資源字典所在的位置
'(使用於資源不在此頁面、
'應用程式，及任何主題專屬資源字典中的情況)
<Assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)>



'下列 GUID 為專案公開 (Expose) 至 COM 時所要使用的 typelib ID
<Assembly: Guid("6b129cb0-bf81-4b3b-b3f8-eeb40f1b510f")> 

' 組件的版本資訊是由下列四項值構成: 
'
'      主要版本
'      次要版本
'      組建編號
'      修訂編號
'
' 您可以指定所有的值，也可以依照以下的方式，使用 '*' 將組建和修訂編號
' 指定為預設值: 
' <Assembly: AssemblyVersion("1.0.*")> 

<Assembly: AssemblyVersion("1.0.0.0")> 
<Assembly: AssemblyFileVersion("1.0.0.0")> 
