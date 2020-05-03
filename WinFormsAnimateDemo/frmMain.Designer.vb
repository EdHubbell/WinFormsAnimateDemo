<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnLaunchMiniMap = New System.Windows.Forms.Button()
        Me.btnSendDataStart = New System.Windows.Forms.Button()
        Me.btnSendDataEnd = New System.Windows.Forms.Button()
        Me.txtTimerInterval = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtXPositionDelta = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'btnLaunchMiniMap
        '
        Me.btnLaunchMiniMap.Location = New System.Drawing.Point(29, 139)
        Me.btnLaunchMiniMap.Name = "btnLaunchMiniMap"
        Me.btnLaunchMiniMap.Size = New System.Drawing.Size(159, 23)
        Me.btnLaunchMiniMap.TabIndex = 0
        Me.btnLaunchMiniMap.Text = "Launch MiniMap"
        Me.btnLaunchMiniMap.UseVisualStyleBackColor = True
        '
        'btnSendDataStart
        '
        Me.btnSendDataStart.Location = New System.Drawing.Point(29, 181)
        Me.btnSendDataStart.Name = "btnSendDataStart"
        Me.btnSendDataStart.Size = New System.Drawing.Size(159, 23)
        Me.btnSendDataStart.TabIndex = 1
        Me.btnSendDataStart.Text = "Send Data Start"
        Me.btnSendDataStart.UseVisualStyleBackColor = True
        '
        'btnSendDataEnd
        '
        Me.btnSendDataEnd.Location = New System.Drawing.Point(29, 225)
        Me.btnSendDataEnd.Name = "btnSendDataEnd"
        Me.btnSendDataEnd.Size = New System.Drawing.Size(159, 23)
        Me.btnSendDataEnd.TabIndex = 2
        Me.btnSendDataEnd.Text = "Send Data End"
        Me.btnSendDataEnd.UseVisualStyleBackColor = True
        '
        'txtTimerInterval
        '
        Me.txtTimerInterval.Location = New System.Drawing.Point(437, 182)
        Me.txtTimerInterval.Name = "txtTimerInterval"
        Me.txtTimerInterval.Size = New System.Drawing.Size(65, 22)
        Me.txtTimerInterval.TabIndex = 3
        Me.txtTimerInterval.Text = "100"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(321, 185)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(110, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Timer Interval (msec)"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(233, 214)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(198, 13)
        Me.Label2.TabIndex = 6
        Me.Label2.Text = "X Position change per interval (pixels)"
        '
        'txtXPositionDelta
        '
        Me.txtXPositionDelta.Location = New System.Drawing.Point(437, 211)
        Me.txtXPositionDelta.Name = "txtXPositionDelta"
        Me.txtXPositionDelta.Size = New System.Drawing.Size(65, 22)
        Me.txtXPositionDelta.TabIndex = 5
        Me.txtXPositionDelta.Text = ".2"
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(644, 399)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtXPositionDelta)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtTimerInterval)
        Me.Controls.Add(Me.btnSendDataEnd)
        Me.Controls.Add(Me.btnSendDataStart)
        Me.Controls.Add(Me.btnLaunchMiniMap)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "frmMain"
        Me.Text = "Main Form"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnLaunchMiniMap As Button
    Friend WithEvents btnSendDataStart As Button
    Friend WithEvents btnSendDataEnd As Button
    Friend WithEvents txtTimerInterval As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtXPositionDelta As TextBox
End Class
