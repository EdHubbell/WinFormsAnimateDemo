Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Reactive.Concurrency
Imports System.Reactive.Disposables
Imports System.Reactive.Linq
Imports System.Reactive.Subjects
Imports System.Timers
Imports System.Collections.Concurrent
Imports System.Reactive


Public Class frmMain
    Implements IToolPositionStateEventSender

    Dim oTimer As Timer

    Dim dblXPos As Double = 0

    ' 2 different ways to communicate to the other form. Either via events, or via iObservables'
    ' There don't seem to be any real difference in performance (at least in this app). 

    Public Event ToolPositionStateChange(sender As Object, state As ToolPositionState) Implements IToolPositionStateEventSender.ToolPositionStateChange

    '    Dim observableToolPositionState As IObservable(Of ToolPositionState)
    Dim observableToolPositionState As New Subject(Of ToolPositionState)

    ' Thanks to this site, for pointing out that subject has a list of observers already in it. 
    ' So there is no threadsafe observer list implementation needed. 
    ' https://blogs.endjin.com/2018/11/understanding-rx-making-interfaces-subscribing-And-other-subjects-click/


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        'observableToolPositionState = Observable.Create(Of ToolPositionState)(Function(o)
        '                                                                          _subscribed.Add(o)
        '                                                                          Return Function() _subscribed.Remove(o)
        '                                                                      End Function)

    End Sub



    Private Sub btnSendData_Click(sender As Object, e As EventArgs) Handles btnSendDataStart.Click

        If oTimer IsNot Nothing AndAlso oTimer.Enabled Then
            MsgBox("Data is already being sent")
            Exit Sub
        End If

        txtTimerInterval.Enabled = False
        txtXPositionDelta.Enabled = False

        oTimer = New Timer(CInt(txtTimerInterval.Text))
        oTimer.AutoReset = True
        AddHandler oTimer.Elapsed, AddressOf HandleTimer

        oTimer.Start()

    End Sub

    'Private Function GetToolStateObservable() As IObservable(Of ToolPositionState)
    '    Return Observable.Create(Of ToolPositionState)(Function(ByVal observer As IObserver(Of ToolPositionState))
    '                                                       observer.OnNext(New ToolPositionState(New PointF(1, 1), New PointF(2, 2)))
    '                                                       observer.OnCompleted()
    '                                                       Return Disposable.Create(Sub() Debug.WriteLine("Observer has unsubscribed"))
    '                                                   End Function)
    'End Function

    Private Sub HandleTimer(source As Object, e As ElapsedEventArgs)

        ' If we go off the screen, then we need to get back on it.
        If dblXPos > 410 Then dblXPos = -10

        dblXPos += CDbl(txtXPositionDelta.Text)

        ' Marshal all the tool position state variables into a single argument. 
        ' In the case of a real tool, we would probably just want to send along whatever
        ' argument was sent when the position of the stages changed.
        ' A flaw in this example is that we're sending pixels. Pixel awareness should 
        ' only exist on the side doing the presentation - In this case, the mini map. 
        Dim oToolPositionState As New ToolPositionState(New Point(CInt(dblXPos), 15), New Point(CInt(dblXPos), 320))

        RaiseEvent ToolPositionStateChange(Me, oToolPositionState)

        ' This is a problem here, because _subscribed isn't thread safe. 
        ' In any case, this raises a series of events, which 
        ' seems more complex than just raising one. Still working on 
        ' understanding reactive, observable, etc. Not many examples in VB - Mostly C#. 
        'For Each o In _subscribed
        '    o.OnNext(oToolPositionState)
        'Next

        observableToolPositionState.OnNext(oToolPositionState)

    End Sub



    Private Sub btnSendDataEnd_Click(sender As Object, e As EventArgs) Handles btnSendDataEnd.Click
        oTimer.Stop()

        txtTimerInterval.Enabled = True
        txtXPositionDelta.Enabled = True

    End Sub

    Private Sub btnLaunchMiniMap_Click(sender As Object, e As EventArgs) Handles btnLaunchMiniMap.Click
        Dim frmMap = New frmMiniMap(Me)
        frmMap.Show()
    End Sub

    Private Sub btnLaunchRxMiniMap_Click(sender As Object, e As EventArgs) Handles btnLaunchRxMiniMap.Click
        Dim frmMap = New frmRxMiniMap(observableToolPositionState)
        frmMap.Show()
    End Sub

    Private Sub frmMain_MouseMove(sender As Object, e As MouseEventArgs) Handles Me.MouseMove
        lblMouseEvent.Text = String.Format("Mouse Event: {0},{1}", e.X, e.Y)
    End Sub


    Private Sub btnAddRxMouse_Click(sender As Object, e As EventArgs) Handles btnAddRxMouse.Click

        ' Abandon all hope of finding VB samples of this type of thing. It's all C#, all the way. These work, tho. That's a start.

        ' Note that the order of operations here is important, which was not expected.
        ' The .ObserveOn operator has to happen after .Sample
        ' ObserveOnDispatcher() will yield exception 'The current thread has no Dispatcher associated with it.'
        ' ObserveOn(RxApp.MainThreadScheduler) isn't really available.
        ' Without the ObserveOn, you'll need to call Invoke to get onto the UI thread in order to change the label.

        Observable.FromEventPattern(Of MouseEventHandler, MouseEventArgs)(
            Sub(handler As MouseEventHandler) AddHandler Me.MouseMove, handler,
            Sub(handler As MouseEventHandler) RemoveHandler Me.MouseMove, handler).
            Select(Function(oEventPattern) oEventPattern.EventArgs).
            ObserveOn(Me).
            Subscribe(Sub(oMouseEventArgs)
                          lblMouseFast.Text = String.Format("Mouse Fast: {0},{1}", oMouseEventArgs.X, oMouseEventArgs.Y)
                      End Sub)


        Observable.FromEventPattern(Of MouseEventHandler, MouseEventArgs)(
            Sub(handler As MouseEventHandler) AddHandler Me.MouseMove, handler,
            Sub(handler As MouseEventHandler) RemoveHandler Me.MouseMove, handler).
            Select(Function(oEventPattern) oEventPattern.EventArgs).
            Sample(TimeSpan.FromMilliseconds(250)).
            ObserveOn(Me).
            Subscribe(Sub(oMouseEventArgs)
                          lblMouseMedium.Text = String.Format("Mouse Med: {0},{1}", oMouseEventArgs.X, oMouseEventArgs.Y)
                      End Sub)


        Observable.FromEventPattern(Of MouseEventHandler, MouseEventArgs)(
            Sub(handler As MouseEventHandler) AddHandler Me.MouseMove, handler,
            Sub(handler As MouseEventHandler) RemoveHandler Me.MouseMove, handler).
            Select(Function(oEventPattern) oEventPattern.EventArgs).
            Sample(TimeSpan.FromMilliseconds(1000)).
            ObserveOn(Me).
            Subscribe(Sub(oMouseEventArgs)
                          lblMouseSlow.Text = String.Format("Mouse Slow: {0},{1}", oMouseEventArgs.X, oMouseEventArgs.Y)
                      End Sub)

    End Sub
End Class

'Dim timerTicks = Observable.FromEventPattern(Of ElapsedEventHandler, ElapsedEventArgs) _
'(Sub(h) AddHandler oTimer.Elapsed, h,
' Sub(h) RemoveHandler oTimer.Elapsed, h)

'Dim timerTicks As IObservable(Of Reactive.EventPattern(Of ElapsedEventArgs)) =
'    Observable.FromEventPattern(Of ElapsedEventHandler, ElapsedEventArgs) _
'(Sub(h) AddHandler oTimer.Elapsed, h,
' Sub(h) RemoveHandler oTimer.Elapsed, h)
