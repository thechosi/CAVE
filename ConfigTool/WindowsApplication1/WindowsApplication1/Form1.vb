Imports System.IO

Public Class Form1
    Dim computers As New List(Of Computer)
    Dim act_computer As Computer


    Private Function GetComputerInfo(computerName As String)

    End Function


    Private Function GetCamInfo(camName As String)
        Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read)
        Dim xml As XDocument = XDocument.Load(fs)
        Dim computername As String

        computername = Me.ListBox1.SelectedItem.Substring(0, Me.ListBox1.SelectedItem.ToString.IndexOf("(") - 1)

        For Each attribute In xml.<config>.<slave>.Attributes
            If attribute.Name.ToString.Equals("ip") Then
                If attribute.Value.Trim().Equals(computername) Then

                End If
            End If
        Next


        fs.Close()

    End Function

    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListBox1.SelectedIndexChanged
        Me.GroupBox2.Text = Me.ListBox1.SelectedItem.ToString

        act_computer = computers.ElementAt(Me.ListBox1.SelectedIndex)

        txt_ipAddress.Text = act_computer.ip
        txt_computerName.Text = act_computer.name

        If TypeOf act_computer Is Master Then
            chk_master.Checked = True
            chk_master.Enabled = False
            grp_relation.Enabled = True
            grp_cam.Enabled = False

            txt_port.Enabled = True
            txt_port.Text = DirectCast(act_computer, Master).port
        Else
            txt_port.Text = ""
            txt_port.Enabled = False
            chk_master.Checked = False
            chk_master.Enabled = False
            grp_relation.Enabled = False
            grp_cam.Enabled = True

            If DirectCast(act_computer, Slave).camera.eye.Equals("left") Then
                combo_eye.SelectedIndex = 0
            Else
                combo_eye.SelectedIndex = 1
            End If


        End If

    End Sub

    Private Sub ÖffnenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ÖffnenToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            Dim fs As New FileStream(OpenFileDialog1.FileName, FileMode.Open, FileAccess.Read)
            ListBox1.Items.Clear()

            Dim xmlReader As New XMLReader
            computers = xmlReader.GetAllComputers(fs)


            For Each computer In computers
                ListBox1.Items.Add(computer.ToString)
            Next

            fs.Close()

        End If
    End Sub

    Private Sub list_rela_SelectedIndexChanged(sender As Object, e As EventArgs) Handles list_rela.SelectedIndexChanged

        Dim relation As RelationToOrigin = DirectCast(act_computer, Master).relationToOrigin

        If list_rela.SelectedItem.Equals("position") Then
            txt_relat_x.Text = relation.position.x
            txt_relat_y.Text = relation.position.y
            txt_relat_z.Text = relation.position.z
        ElseIf list_rela.SelectedItem.Equals("rotation") Then
            txt_relat_x.Text = relation.rotation.x
            txt_relat_y.Text = relation.rotation.y
            txt_relat_z.Text = relation.rotation.z
        End If


    End Sub

    Private Sub list_splane_SelectedIndexChanged(sender As Object, e As EventArgs) Handles list_splane.SelectedIndexChanged


        Dim screenplane As ScreenPlane = act_computer.screenplane

        If list_splane.SelectedItem.Equals("pa") Then
            txt_splane_x.Text = screenplane.pa.x
            txt_splane_y.Text = screenplane.pa.y
            txt_splane_z.Text = screenplane.pa.z
        ElseIf list_splane.SelectedItem.Equals("pb") Then
            txt_splane_x.Text = screenplane.pb.x
            txt_splane_y.Text = screenplane.pb.y
            txt_splane_z.Text = screenplane.pb.z
        ElseIf list_splane.SelectedItem.Equals("pc") Then
            txt_splane_x.Text = screenplane.pc.x
            txt_splane_y.Text = screenplane.pc.y
            txt_splane_z.Text = screenplane.pc.z
        ElseIf list_splane.SelectedItem.Equals("pe") Then
            txt_splane_x.Text = screenplane.pe.x
            txt_splane_y.Text = screenplane.pe.y
            txt_splane_z.Text = screenplane.pe.z
        End If
    End Sub

    Private Sub list_cam_SelectedIndexChanged(sender As Object, e As EventArgs) Handles list_cam.SelectedIndexChanged

        Dim camera As Camera = DirectCast(act_computer, Slave).camera

        If list_cam.SelectedItem.Equals("rotation") Then
            txt_cam_x.Text = camera.rotation.x
            txt_cam_y.Text = camera.rotation.y
            txt_cam_z.Text = camera.rotation.z
        End If
    End Sub

    Private Sub BeendenToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BeendenToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub btn_configSave_Click(sender As Object, e As EventArgs) Handles btn_configSave.Click

    End Sub
End Class
