Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports OpenNETCF.Windows.Forms
Imports OpenNETCF.Win32

Namespace KeyRecorder

    Public Class Form1
        Dim m_keyHook As KeyboardHook = New KeyboardHook()
        Dim m_hideshift As Integer = 0
        Dim m_hidden As Boolean = False
    
        Private Sub btnHideShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnHideShow.Click
            m_hidden = Not m_hidden
            
            If m_hidden Then                
                btnHideShow.BackColor = SystemColors.ActiveBorder
                btnHideShow.Top = 0
            Else
                btnHideShow.BackColor = SystemColors.Control
                btnHideShow.Top = 4
            End If

            m_hideshift = IIf(m_hideshift = 0, 27, 0)
            Move()
        End Sub

        Private Sub InputPanel1_EnabledChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InputPanel1.EnabledChanged
            Move()
        End Sub
        
        Private Sub Move()
            If InputPanel1.Enabled Then
                Me.Top = InputPanel1.Bounds.Top - 30 + m_hideshift
            Else
                Me.Top = Screen.PrimaryScreen.WorkingArea.Height - 30 + m_hideshift
            End If
        End Sub

        Protected Overloads Overrides Sub OnLoad(ByVal e As EventArgs)
            m_keyHook.Enabled = True
            AddHandler m_keyHook.KeyDetected, AddressOf OnKeyDetected
            Me.Width = Screen.PrimaryScreen.WorkingArea.Width
            Me.Height = 29
            Move()

            Dim keysWindow As New Win32Window(txtKeys.Handle)
            keysWindow.ExtendedStyle = keysWindow.ExtendedStyle Or WS_EX.NOACTIVATE

            MyBase.OnLoad(e)
        End Sub

        Protected Overloads Overrides Sub OnPaintBackground(ByVal e As PaintEventArgs)
            e.Graphics.FillRectangle(New SolidBrush(SystemColors.ControlDark), 0, 0, Me.Width, Me.Height)
        End Sub

        Private Sub btnClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClear.Click
            txtKeys.Text = String.Empty
        End Sub

        Private Sub OnKeyDetected(ByVal keyMessage As OpenNETCF.Win32.WM, ByVal keyData As KeyData)
            If Me.InvokeRequired Then
                Me.Invoke(New KeyHookEventHandler(AddressOf OnKeyDetected), New Object() {keyMessage, keyData})
                Return
            End If
            Dim c As Char = CChar(CStr(keyData.KeyCode))
            If keyMessage = WM.KEYUP Then
                If Char.IsLetterOrDigit(c) Then
                    txtKeys.Text += c
                Else
                    Select Case CType(AscW(c), Keys)
                        Case Keys.Up
                            txtKeys.Text += "{up}"
                            Exit Select
                        Case Keys.Down
                            txtKeys.Text += "{dn}"
                            Exit Select
                        Case Keys.Left
                            txtKeys.Text += "{lt}"
                            Exit Select
                        Case Keys.Right
                            txtKeys.Text += "{rt}"
                            Exit Select
                    End Select
                End If
            End If
            txtKeys.SelectionStart = txtKeys.Text.Length
            txtKeys.ScrollToCaret()
        End Sub        
    End Class

End Namespace