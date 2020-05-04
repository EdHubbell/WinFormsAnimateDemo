﻿Option Strict On
Option Explicit On

Imports System.Collections.ObjectModel
Imports System.Reactive.Concurrency
Imports System.Reactive.Disposables
Imports System.Reactive.Linq
Imports System.Reactive.Subjects
Imports System.Timers
Imports System.Collections.Concurrent


Public Class frmMain
    Implements IToolPositionStateEventSender

    Dim oTimer As Timer

    Dim dblXPos As Double = 0
    Public Event ToolPositionStateChange(sender As Object, state As ToolPositionState) Implements IToolPositionStateEventSender.ToolPositionStateChange

    Dim observableToolPositionState As IObservable(Of ToolPositionState)

    Private _subscribed As List(Of IObserver(Of ToolPositionState)) = New List(Of IObserver(Of ToolPositionState))()


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        'observableToolPositionState = Observable.Return(Of ToolPositionState)(New ToolPositionState(New PointF(1, 1), New PointF(2, 2)))

        'observableToolPositionState = Observable.Create(Of ToolPositionState)(Function(Of IObserver(Of ToolPositionState), IDisposable)())()()

        'observableToolPositionState = Observable.Create(Of ToolPositionState)(Function(ByVal observer As IObserver(Of ToolPositionState))
        '                                                                          Return Disposable.Create(Sub() Debug.WriteLine("Observer has unsubscribed"))
        '                                                                      End Function)

        observableToolPositionState = Observable.Create(Of ToolPositionState)(Function(o)
                                                                                  _subscribed.Add(o)
                                                                                  Return Function() _subscribed.Remove(o)
                                                                              End Function)


        'observableToolPositionState = Observable.Create(Of ToolPositionState)(Function(ByVal observer As IObserver(Of ToolPositionState))
        '                                                                          observer.OnNext(New ToolPositionState(New PointF(1, 1), New PointF(2, 2)))
        '                                                                          observer.OnCompleted()
        '                                                                          Return Disposable.Create(Sub() Debug.WriteLine("Observer has unsubscribed"))
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

        'Dim timerTicks = Observable.FromEventPattern(Of ElapsedEventHandler, ElapsedEventArgs) _
        '(Sub(h) AddHandler oTimer.Elapsed, h,
        ' Sub(h) RemoveHandler oTimer.Elapsed, h)

        'Dim timerTicks As IObservable(Of Reactive.EventPattern(Of ElapsedEventArgs)) =
        '    Observable.FromEventPattern(Of ElapsedEventHandler, ElapsedEventArgs) _
        '(Sub(h) AddHandler oTimer.Elapsed, h,
        ' Sub(h) RemoveHandler oTimer.Elapsed, h)

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

        ' This is a problem here, because _subscribed isn't type safe. 
        ' In any case, this raises a series of events, which 
        ' seems more complex than just raising one. Still working on 
        ' understanding reactive, observable, etc. Not many examples in VB - Mostly C#. 
        For Each o In _subscribed
            o.OnNext(oToolPositionState)
        Next

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
End Class
