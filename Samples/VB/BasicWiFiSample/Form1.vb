Imports OpenNETCF.Net.NetworkInformation
Imports System.Text

Public Class Form1
    Private NIC As WirelessZeroConfigNetworkInterface = Nothing

    Public Sub New()
        InitializeComponent()

        'find the nic
        For Each intf In NetworkInterface.GetAllNetworkInterfaces()
            NIC = TryCast(intf, WirelessZeroConfigNetworkInterface)

            If Not NIC Is Nothing Then
                Exit For
            End If
        Next

        If NIC Is Nothing Then
            MsgBox("No WZC NIC found")
            Exit Sub
        End If

        nicName.Text = NIC.Name

        RefreshAPList()
    End Sub

    Sub RefreshAPList()
        If NIC Is Nothing Then
            MsgBox("No WZC NIC found")
            Exit Sub
        End If

        apList.Items.Clear()

        For Each ap In NIC.NearbyAccessPoints
            Dim strings As String() = New String() _
            { _
                ap.Name, _
                BytesToString(ap.PhysicalAddress.GetAddressBytes()), _
                String.Format("{0}db", ap.SignalStrength.Decibels) _
            }

            Dim lvi As New ListViewItem( strings)

            apList.Items.Add(lvi)
        Next
    End Sub

    Function BytesToString(ByVal data() As Byte) As String
        Dim sb As New StringBuilder
        Dim i As Integer

        For i = 0 To data.Length - 2
            sb.Append(String.Format("{0:X2}-", data(i)))
        Next

        sb.Append(String.Format("{0:X2}", data(i)))

        Return sb.ToString()
    End Function

    Private Sub refresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles refresh.Click
        RefreshAPList()
    End Sub
End Class

