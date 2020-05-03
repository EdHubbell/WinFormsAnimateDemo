Option Strict On
Option Explicit On

Imports System.Timers

Public Class frmMain
    Implements IToolPositionStateEventSender

    Dim oTimer As Timer

    Dim dblXPos As Double = 0
    Public Event ToolPositionStateChange(sender As Object, state As ToolPositionState) Implements IToolPositionStateEventSender.ToolPositionStateChange


    Private Sub btnLaunchMiniMap_Click(sender As Object, e As EventArgs) Handles btnLaunchMiniMap.Click

        Dim frmMap = New frmMiniMap(Me)
        frmMap.Show()

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

    End Sub

    Private Sub btnSendDataEnd_Click(sender As Object, e As EventArgs) Handles btnSendDataEnd.Click
        oTimer.Stop()

        txtTimerInterval.Enabled = True
        txtXPositionDelta.Enabled = True

    End Sub
End Class
