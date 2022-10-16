Imports System.Windows.Threading
Module ThreadDoEvents
    Public Sub DoEvents()
        Dim Frame As DispatcherFrame = New DispatcherFrame
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, New DispatcherOperationCallback(AddressOf ExitFrame), Frame)
        Dispatcher.PushFrame(Frame)
    End Sub
    Private Function ExitFrame(State As DispatcherFrame) As Object
        State.Continue = False
        Return Nothing
    End Function
End Module
