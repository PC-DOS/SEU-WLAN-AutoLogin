Imports mshtml
Imports System.Text
Imports System.IO
Imports System.Net
Class MainWindow
    Dim IsConfigured As Boolean
    Dim WebBrowserLoadingOperationWaiter As New EventOrOperationWaiter
    Dim GeneralTimoutWaiter As New EventOrOperationWaiter
    Const LoginPageURL As String = "https://w.seu.edu.cn"
    Const ArgEnterConfigMode As String = "CONFIGMODE"
    Private Sub LockUi()
        txtUsername.IsEnabled = False
        txtPassword.IsEnabled = False
        cmbLoginNetwork.IsEnabled = False
        btnLogin.IsEnabled = False
    End Sub
    Private Sub UnlockUi()
        txtUsername.IsEnabled = True
        txtPassword.IsEnabled = True
        cmbLoginNetwork.IsEnabled = True
        btnLogin.IsEnabled = True
    End Sub
    Private Function EncryptString(OriginalString As String) As String
        Return Convert.ToBase64String(Encoding.UTF8.GetBytes(OriginalString))
    End Function
    Private Function DecryptString(EncryptedString As String) As String
        Return Encoding.UTF8.GetString(Convert.FromBase64String(EncryptedString))
    End Function
    Private Sub SaveSettings()
        SaveSetting(ApplicationName, SettingsSectionName, UsernameKey, txtUsername.Text)
        SaveSetting(ApplicationName, SettingsSectionName, PasswordKey, EncryptString(txtPassword.Password))
        SaveSetting(ApplicationName, SettingsSectionName, LoginNetworkKey, cmbLoginNetwork.SelectedIndex)
        SaveSetting(ApplicationName, SettingsSectionName, IsConfiguredKey, IsConfigured)
    End Sub
    Private Sub LoadSettings()
        txtUsername.Text = GetSetting(ApplicationName, SettingsSectionName, UsernameKey, UsernameDefVal)
        txtPassword.Password = DecryptString(GetSetting(ApplicationName, SettingsSectionName, PasswordKey, EncryptString(PasswordDefVal)))
        cmbLoginNetwork.SelectedIndex = Int(GetSetting(ApplicationName, SettingsSectionName, LoginNetworkKey, LoginNetworkDefVal))
        IsConfigured = CBool(GetSetting(ApplicationName, SettingsSectionName, IsConfiguredKey, IsConfiguredDefVal))
    End Sub
    Private Sub wbbWebContainer_LoadCompleted(sender As Object, e As NavigationEventArgs) Handles wbbWebContainer.LoadCompleted
        WebBrowserLoadingOperationWaiter.SetWaitingConditionSatisfied()
    End Sub
    Private Sub MainWindow_Closed(sender As Object, e As EventArgs) Handles Me.Closed
        WebBrowserLoadingOperationWaiter.SetWaitingConditionAborted()
        GeneralTimoutWaiter.SetWaitingConditionAborted()
        End
    End Sub
    Private Sub MainWindow_Closing(sender As Object, e As ComponentModel.CancelEventArgs) Handles Me.Closing
        SaveSettings()
    End Sub
    Private Sub MainWindow_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        Dim ActiveX = wbbWebContainer.GetType().InvokeMember("ActiveXInstance", Reflection.BindingFlags.GetProperty Or Reflection.BindingFlags.Instance Or Reflection.BindingFlags.NonPublic, Nothing, wbbWebContainer, Nothing)
        ActiveX.Silent = True
        LoadSettings()

        'Check for command line arguments
        Dim AppArguments() As String = Environment.GetCommandLineArgs()
        For Each a As String In AppArguments
            If a.ToUpper = "-" & ArgEnterConfigMode Or a.ToUpper = "/" & ArgEnterConfigMode Then
                IsConfigured = False
                SaveSettings()
            End If
        Next

        If IsConfigured Then
            Me.Visibility = Windows.Visibility.Hidden
            LockUi()
            IsConfigured = True
            SaveSettings()
            ExecuteLogin()
            UnlockUi()
            Application.Current.Shutdown()
        Else
            wbbWebContainer.Navigate(LoginPageURL)
        End If
    End Sub
    Private Sub ExecuteLogin()
        'Navigate to SEU-WLAN login page
        Dim RetryCounter As Integer
        For RetryCounter = 1 To 5
            WebBrowserLoadingOperationWaiter.SetWaitingConditionNotSatisfied()
            If RetryCounter = 1 Then
                wbbWebContainer.Navigate(LoginPageURL)
            Else
                wbbWebContainer.Refresh()
            End If
            If WebBrowserLoadingOperationWaiter.WaitForEventOrOperationFinished(60 * 1000) Then
                RetryCounter = 0
                Exit For
            End If
            DoEvents()
        Next

        'Create HTML document object and elements collection
        Dim CurrentHTMLDocument As HTMLDocument = Nothing
        Dim ChildrenHTMLDocument As HTMLDocument = Nothing
        Dim PossibleElements As IHTMLElementCollection = Nothing
        Dim ChildrenCollection As IHTMLElementCollection = Nothing
        Dim CurrentElement As IHTMLElement = Nothing
        Dim CurrentInputElement As IHTMLInputElement = Nothing
        Dim CurrentComboBoxElement As IHTMLSelectElement = Nothing
        Dim ChildElement As IHTMLElement = Nothing

        'Input user name
        CurrentHTMLDocument = wbbWebContainer.Document
        PossibleElements = CurrentHTMLDocument.getElementsByName("DDDDD")
        If PossibleElements.length > 0 Then
            CurrentInputElement = PossibleElements(1)
            If Not IsNothing(CurrentInputElement) Then
                CurrentInputElement.value = txtUsername.Text
            End If
        End If

        'Input password
        PossibleElements = CurrentHTMLDocument.getElementsByName("upass")
        If PossibleElements.length > 0 Then
            CurrentInputElement = PossibleElements(1)
            If Not IsNothing(CurrentInputElement) Then
                CurrentInputElement.value = txtPassword.Password
            End If
        End If

        'Select network
        PossibleElements = CurrentHTMLDocument.getElementsByName("ISP_select")
        If PossibleElements.length > 0 Then
            CurrentComboBoxElement = PossibleElements(0)
            If Not IsNothing(CurrentComboBoxElement) Then
                Dim SelectedNetwork As ComboBoxItem = cmbLoginNetwork.SelectedItem
                CurrentComboBoxElement.value = SelectedNetwork.Tag
            End If
        End If

        'Login
        PossibleElements = CurrentHTMLDocument.getElementsByName("0MKKey")
        If PossibleElements.length > 0 Then
            CurrentElement = PossibleElements(1)
            If Not IsNothing(CurrentInputElement) Then
                CurrentElement.click()
            End If
        End If
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As RoutedEventArgs) Handles btnLogin.Click
        LockUi()
        IsConfigured = True
        SaveSettings()

        ExecuteLogin()

        UnlockUi()
    End Sub
End Class
