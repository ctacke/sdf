Imports OpenNETCF.Net

Public Class Form1
    Private m_manager As ConnectionManager
    Private m_destinations As DestinationInfoCollection
    Private m_detailCollection As ConnectionDetailCollection

    Public Sub New()
        InitializeComponent()

        Try
            m_manager = New OpenNETCF.Net.ConnectionManager()
        Catch ex As PlatformNotSupportedException

        End Try

        If (m_manager.SupportsStatusNotifications) Then
            m_detailCollection = m_manager.GetConnectionDetailItems()
            AddHandler m_detailCollection.ConnectionDetailItemsChanged, AddressOf m_detailCollection_ConnectionDetailItemsChanged
        Else
            ' TODO: notify user / maybe spawn a watcher thread?
            AddHandler m_manager.ConnectionStateChanged, AddressOf m_manager_ConnectionStateChanged
        End If

        RefreshDestinations()
    End Sub

    Private Sub m_detailCollection_ConnectionDetailItemsChanged(ByVal source As Object, ByVal newStatus As ConnectionStatus)
        ' TODO: Add something here
    End Sub

    Private Sub m_manager_ConnectionStateChanged(ByVal source As Object, ByVal newStatus As ConnectionStatus)
        ' TODO: Add something here
    End Sub

    Private Sub RefreshDestinations()
        m_destinations = m_manager.EnumDestinations()


        destinationList.Items.Clear()
        destinationList.BeginUpdate()

        For Each dest As DestinationInfo In m_destinations

            Dim lvi As ListViewItem = New ListViewItem( _
                            New String() _
                            { _
                                dest.Description, _
                                "<unknown>", _
                                dest.Guid.ToString() _
                            })

            lvi.Tag = dest

            destinationList.Items.Add(lvi)
        Next

        destinationList.EndUpdate()
    End Sub
End Class
