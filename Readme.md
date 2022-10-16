# 东南大学校园网（SEU-WLAN / SEU-ISP）自动登录工具

运行本工具，输入一卡通号、密码，并选择需要登录的网络类型，点击 [登录] 按钮，即可自动登录。

程序会自动记住您提供的登录凭据，下次运行程序时，将自动执行登录过程。登录凭据仅会在您的本地计算机上储存并用于登录网络，不会被用于其它用途。

## 维护资料

您可以使用`-ConfigMode`参数启动应用程序，此时程序不会执行自动登录流程，仅会显示主界面。

程序记录的参数被存储在注册表的下列位置：

```
HKEY_CURRENT_USER\SOFTWARE\VB and VBA Program Settings\SeuWlanAutoLogin\AppSettings
```