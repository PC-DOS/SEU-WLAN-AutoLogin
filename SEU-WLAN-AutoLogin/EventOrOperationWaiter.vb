Imports System.Windows.Threading
Public Enum WaitingStatus
    WaitingConditionSatisified = 0
    WaitingConditionNotSatisified = 1
    TimedOut = 2
    Failed = 3
    Aborted = 4
End Enum
''' <summary>
''' Wait for a waiting condition, without blocking the Event Loop.
''' </summary>
''' <remarks></remarks>
Public Class EventOrOperationWaiter
    ''' <summary>
    ''' Marks if waiting condition has been satisfied.
    ''' </summary>
    ''' <remarks></remarks>
    Private WaitingStatusIndicator As WaitingStatus
    ''' <summary>
    ''' Timer used to count elapsed time.
    ''' </summary>
    ''' <remarks></remarks>
    Private ElapsedTimeTimer As Timers.Timer
    ''' <summary>
    ''' Elapsed time, in millisecond.
    ''' </summary>
    ''' <remarks></remarks>
    Private ElapsedTime As UInteger
    ''' <summary>
    ''' Create a new object, waiting condition is satisfied by default
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        ElapsedTimeTimer = New Timers.Timer(100)
        ElapsedTime = 0
        AddHandler ElapsedTimeTimer.Elapsed, AddressOf ElapsedTimeTimer_Tick
        SetWaitingConditionSatisfied()
    End Sub
    ''' <summary>
    ''' Wait for the waiting condition without blocking the Event Loop.
    ''' </summary>
    ''' <returns>Returns True if waiting has succeeded. Otherwise returns false</returns>
    ''' <remarks></remarks>
    Public Function WaitForEventOrOperationFinished(Optional TimeoutMillisecond As UInteger = 0) As Boolean
        'Start counting elapsed time
        ElapsedTime = 0
        If TimeoutMillisecond > 0 Then
            ElapsedTimeTimer.Start()
        End If

        'Wait
        While WaitingStatusIndicator <> WaitingStatus.WaitingConditionSatisified
            DoEvents()
            If TimeoutMillisecond > 0 And ElapsedTime >= TimeoutMillisecond Then
                SetWaitingConditionTimedOut()
            End If
            If WaitingStatusIndicator = WaitingStatus.TimedOut Or WaitingStatusIndicator = WaitingStatus.Failed Or WaitingStatusIndicator = WaitingStatus.Aborted Then
                ElapsedTimeTimer.Stop()
                ElapsedTime = 0
                Return False
            End If
        End While

        'Wait succeeded
        ElapsedTimeTimer.Stop()
        ElapsedTime = 0
        Return True
    End Function
    ''' <summary>
    ''' Marks the waiting condition as not satisifed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetWaitingConditionNotSatisfied()
        WaitingStatusIndicator = WaitingStatus.WaitingConditionNotSatisified
    End Sub
    ''' <summary>
    ''' Marks the waiting condition as satisifed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetWaitingConditionSatisfied()
        WaitingStatusIndicator = WaitingStatus.WaitingConditionSatisified
    End Sub
    ''' <summary>
    ''' Marks the waiting condition as timedout.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetWaitingConditionTimedOut()
        WaitingStatusIndicator = WaitingStatus.TimedOut
    End Sub
    ''' <summary>
    ''' Marks the waiting condition as failed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetWaitingConditionFailed()
        WaitingStatusIndicator = WaitingStatus.Failed
    End Sub
    ''' <summary>
    ''' Marks the waiting condition as aborted.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetWaitingConditionAborted()
        WaitingStatusIndicator = WaitingStatus.Aborted
    End Sub
    Private Sub DoEvents()
        Dim Frame As DispatcherFrame = New DispatcherFrame
        Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Background, New DispatcherOperationCallback(AddressOf ExitFrame), Frame)
        Dispatcher.PushFrame(Frame)
    End Sub
    Private Function ExitFrame(State As DispatcherFrame) As Object
        State.Continue = False
        Return Nothing
    End Function
    Private Sub ElapsedTimeTimer_Tick()
        ElapsedTime += 100
    End Sub
End Class
