Option Explicit On
Option Strict On

Imports System.ComponentModel
Imports System.Drawing
Imports System.Reactive.Disposables
Imports System.Reactive.Subjects

Public Class frmRxMiniMap
    Implements IObserver(Of ToolPositionState)

    Dim bFormBusy As Boolean = False

    Dim bmpBackground As Bitmap = My.Resources.EdProfile400x400

    Dim bmpHat As Bitmap = My.Resources.iconmonstr_school_22_240

    Dim bmpVolleyBall As Bitmap = My.Resources.iconmonstr_volleyball_2_32
    Dim bmpSoccerBall As Bitmap = My.Resources.iconmonstr_soccer_1_32

    ' This could also be configured as a Subject, but IObservable is a little bit lighter and has all the features we need here. 
    Dim oToolPosState As IObservable(Of ToolPositionState)
    Dim oToolStateSubscription As IDisposable


    Public Sub New(oToolPositionEventObservable As IObservable(Of ToolPositionState))

        ' This call is required by the designer.
        InitializeComponent()

        oToolPosState = oToolPositionEventObservable

        oToolStateSubscription = oToolPosState.Subscribe(Me)

    End Sub

    Private Sub frmMiniMap_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        MaximizePictureboxSize()

        OverlayOnBackgroundImage(bmpHat, New Point(70, 10))

    End Sub

    ''' <summary>
    ''' Used to set any static images that we won't want to draw very often. This will help us cut down on the number of draws that we have to do. 
    ''' These are the kinds of things that maybe change once in a while (like wafer size), not the things we want to track in real time (like chuck position). 
    ''' </summary>
    Public Sub OverlayOnBackgroundImage(bitmapOverlay As Bitmap, overlayLocation As Point)
        bFormBusy = True

        Try
            ' Refresh the background image to whatever the real background is.
            bmpBackground = My.Resources.EdProfile400x400

            ' Add whatever mostly static image we need to add.
            Using g As Graphics = Graphics.FromImage(bmpBackground)
                g.DrawImage(bitmapOverlay, overlayLocation)
            End Using

            pbxImage.Image = bmpBackground

        Catch ex As Exception
            bFormBusy = False
        End Try

        bFormBusy = False
    End Sub


    Private Sub frmMiniMap_Resize(sender As Object, e As EventArgs) Handles Me.Resize

        bFormBusy = True
        MaximizePictureboxSize()
        bFormBusy = False

    End Sub

    Private Sub MaximizePictureboxSize()
        Try
            ' Find the largest size square we can make in the tlpMain.
            Dim minSidePixels As Integer = Math.Min(tlpMain.Width, tlpMain.Height)

            ' Make the picturebox that size. The picturebox is in the middle cell of a tablelayoutpanel. The middle cell is set to autosize. The result is a 
            ' centered square picturebox that is as big as it can be, given the size of the form. This can be changed to do a desired aspect ratio instead of just a square.
            pbxImage.Width = minSidePixels
            pbxImage.Height = minSidePixels

        Catch ex As Exception

        End Try

    End Sub

    Public Sub OnNext(value As ToolPositionState) Implements IObserver(Of ToolPositionState).OnNext

        If bFormBusy Then
            ' If we're already busy, get out of here. You can add logging or a breakpoint here to see how often it happens. On some level, it will 
            ' always come down to processing speed. 
            Exit Sub
        End If

        ' We could add a check here for objectPosition1 and objectPosition2 - If all the drawn objects aren't changing position, then 
        ' don't bother redrawing the bitmap. Just depends on how long the check takes vs the bitmap redraw.
        bFormBusy = True

        Try

            ' First, copy the background image into a new bitmap.
            Dim bmpBackgroundCopy As Bitmap = New Bitmap(bmpBackground)

            ' Then draw the movable things onto the background image. 
            Using g As Graphics = Graphics.FromImage(bmpBackgroundCopy)
                g.DrawImage(bmpVolleyBall, value.Object1Position)
                g.DrawImage(bmpSoccerBall, value.Object2Position)
            End Using

            ' Then reset the image in the picturebox.
            pbxImage.Image = bmpBackgroundCopy

        Catch ex As Exception

        Finally
            bFormBusy = False
        End Try

    End Sub

    Public Sub OnError([error] As Exception) Implements IObserver(Of ToolPositionState).OnError
        Throw New NotImplementedException()
    End Sub

    Public Sub OnCompleted() Implements IObserver(Of ToolPositionState).OnCompleted
        Throw New NotImplementedException()
    End Sub

    Private Sub frmRxMiniMap_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        ' This is akin to RemoveHandler. Disposing of the subscription causes the number of observers in the 
        ' subject to decrement.
        oToolStateSubscription.Dispose()
    End Sub
End Class
