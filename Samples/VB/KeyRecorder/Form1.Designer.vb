Namespace KeyRecorder
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Form1
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()> _
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer
        Private mainMenu1 As System.Windows.Forms.MainMenu

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Me.mainMenu1 = New System.Windows.Forms.MainMenu
            Me.btnHideShow = New System.Windows.Forms.Button
            Me.btnClear = New System.Windows.Forms.Button
            Me.txtKeys = New System.Windows.Forms.TextBox
            Me.InputPanel1 = New Microsoft.WindowsCE.Forms.InputPanel(Me.components)
            Me.SuspendLayout()
            '
            'btnHideShow
            '
            Me.btnHideShow.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnHideShow.Font = New System.Drawing.Font("Tahoma", 6.0!, System.Drawing.FontStyle.Regular)
            Me.btnHideShow.Location = New System.Drawing.Point(215, 3)
            Me.btnHideShow.Name = "btnHideShow"
            Me.btnHideShow.Size = New System.Drawing.Size(21, 10)
            Me.btnHideShow.TabIndex = 5
            Me.btnHideShow.Text = "_"
            '
            'btnClear
            '
            Me.btnClear.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnClear.Font = New System.Drawing.Font("Tahoma", 6.0!, System.Drawing.FontStyle.Regular)
            Me.btnClear.Location = New System.Drawing.Point(215, 13)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(21, 10)
            Me.btnClear.TabIndex = 4
            Me.btnClear.Text = "CLR"
            '
            'txtKeys
            '
            Me.txtKeys.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtKeys.ForeColor = System.Drawing.SystemColors.MenuText
            Me.txtKeys.Location = New System.Drawing.Point(3, 3)
            Me.txtKeys.Name = "txtKeys"
            Me.txtKeys.Size = New System.Drawing.Size(208, 21)
            Me.txtKeys.TabIndex = 3
            '
            'InputPanel1
            '
            '
            'Form1
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
            Me.AutoScroll = True
            Me.ClientSize = New System.Drawing.Size(240, 268)
            Me.Controls.Add(Me.btnHideShow)
            Me.Controls.Add(Me.btnClear)
            Me.Controls.Add(Me.txtKeys)
            Me.Menu = Me.mainMenu1
            Me.Name = "Form1"
            Me.Text = "Form1"
            Me.ResumeLayout(False)

        End Sub
        Private WithEvents btnHideShow As System.Windows.Forms.Button
        Private WithEvents btnClear As System.Windows.Forms.Button
        Private WithEvents txtKeys As System.Windows.Forms.TextBox
        Friend WithEvents InputPanel1 As Microsoft.WindowsCE.Forms.InputPanel
    End Class
End Namespace
